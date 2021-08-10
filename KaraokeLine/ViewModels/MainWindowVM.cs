using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using KaraokeLine.Architecture;
using KaraokeLine.Interfaces;
using KaraokeLine.Models;
using MaterialDesignThemes.Wpf;

namespace KaraokeLine.ViewModels
{
    public class MainWindowVm : IBaseViewModel, INotifyPropertyChanged
    {
        private ISessionService Service { get; }
        private IPlayerService PlayerService { get; }
        public ISnackbarMessageQueue MessageQueue { get; set; }
        public ICommand AdicionaParticipante { get; }
        public ICommand ProximoParticipanteComando { get; }
        public ICommand ProximoTurnoComando { get; }
        public ICommand ComecaSessaoComando { get; }

        public MainWindowVm(ISessionService service, IPlayerService playerService, ISnackbarMessageQueue queue)
        {
            Service = service;
            PlayerService = playerService;
            MessageQueue = queue;
            AdicionaParticipante = new Command(CriaParticipante);
            ProximoParticipanteComando = new Command(ProximoParticipante);
            ProximoTurnoComando = new Command(ProximoTurno);
        }

        public string EditSinger { get; set; }
        public ObservableCollection<Singer> Singers { get; set; } = new();
        public ObservableCollection<Singer> SessionSingers { get; set; } = new();
        public bool SessionStarted { get; set; }
        private KaraokeSession GameSession { get; set; }
        private int GlobalCounter { get; set; } = 1;

        public async Task Initialize()
        {
            GameSession = await Service.InitializeSession();
            SessionStarted = true;
            NotificaProperiedade(nameof(SessionStarted));
        }

        private void CriaParticipante()
        {
            if (EditSinger == string.Empty)
            {
                MostraMensagem("Participante precisa de um nome");
                return;
            }

            if (Singers.Any(s => s.Name.ToLowerInvariant().Equals(EditSinger.ToLowerInvariant())))
            {
                MostraMensagem("Participante já existe");
                return;
            }

            var singer = new Singer
            {
                Name = EditSinger,
                Id = System.Guid.NewGuid().ToString("N"),
                GlobalOrder = GlobalCounter
            };
            if (GameSession.CurrentIteration > 1)
                singer.Penalty = true;
            Singers.Add(singer);
            GameSession.RegisteredSingers.Add(singer);
            EditSinger = string.Empty;
            GlobalCounter++;
            AtualizaListaGlobal();
        }

        private void ProximoParticipante()
        {
            if (!Singers.Any())
            {
                MostraMensagem("Fila atual vazia");
                return;
            }

            var singer = Singers.ElementAt(0);
            singer.TimesSang++;
            GameSession
               .RegisteredSingers[GameSession
                                 .RegisteredSingers
                                 .IndexOf(GameSession
                                         .RegisteredSingers
                                         .First(s => s.Id == singer.Id))] = singer;
            AtualizaListaGlobal();
            Singers.RemoveAt(0);
        }

        private void ProximoTurno()
        {
            if (!GameSession.RegisteredSingers.Any())
            {
                MostraMensagem("Não existem participantes na sessão");
                return;
            }

            if (Singers.Any())
            {
                MostraMensagem("Fila ainda possui participantes");
                return;
            }

            Singers.Clear();
            GameSession.CurrentIteration++;
            var cantoresDoTurno = GameSession.RegisteredSingers
                                             .Where(cantores => cantores.TimesSang < GameSession.CurrentIteration)
                                             .ToList();
            var cantores = cantoresDoTurno.OrderBy(cantor => cantor.TimesSang)
                                           //.ThenByDescending(s => s.GlobalOrder)
                                          .ToList();
            var cantoresPenalizados = cantores.Where(c => c.Penalty).ToList();
            if (cantoresPenalizados.Any())
            {
                foreach (var cantorPenalizado in cantoresPenalizados)
                {
                    cantores.Remove(cantorPenalizado);
                }
                GameSession.RegisteredSingers.ForEach(c => c.Penalty = false);
            }


            cantores.ForEach(Singers.Add);
        }

        private void AtualizaListaGlobal()
        {
            SessionSingers.Clear();
            GameSession.RegisteredSingers.ForEach(SessionSingers.Add);
            Service.UpdateSession(GameSession);
        }

        private void MostraMensagem(string mensagem)
        {
            MessageQueue.Enqueue(mensagem,
                                 null,
                                 null,
                                 null,
                                 false,
                                 true,
                                 System.TimeSpan.FromSeconds(5));
        }

        public string Title => "Karaoke Line";
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotificaProperiedade(string propriedade)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propriedade));
        }
    }
}