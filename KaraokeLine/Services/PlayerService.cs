using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KaraokeLine.Interfaces;
using KaraokeLine.Models;

namespace KaraokeLine.Services
{
    public class PlayerService : IPlayerService
    {
        public PlayerService(ISessionService sessionService)
        {
            SessionService = sessionService;
        }

        private ISessionService SessionService { get; }
        public async Task<IReadOnlyCollection<Singer>> ListPlayers(string sessionId)
        {
            var session = await SessionService.RetrieveSession(sessionId);
            return session?.RegisteredSingers;
        }

        public async Task<Singer> AddPlayer(string sessionId, Singer player)
        {
            var session = await SessionService.RetrieveSession(sessionId);
            if (session is null)
                throw new System.NullReferenceException("Session doesn't exist");
            if (session.RegisteredSingers.Any(players => players.Id == player.Id || players.Name.Equals(player.Name)))
                return null;
            if(player.Id is null || player.Id == string.Empty)
                player.Id = System.Guid.NewGuid().ToString("N");
            session.RegisteredSingers.Add(player);
            await SessionService.UpdateSession(session);
            return player;
        }

        public async Task<Singer> RemovePlayer(string sessionId ,Singer player)
        {
            var session = await SessionService.RetrieveSession(sessionId);
            if (session is null)
                throw new System.NullReferenceException("Session doesn't exist");
            if (session.RegisteredSingers.All(players => players.Id != player.Id))
                return null;
            var playerToRemove = session.RegisteredSingers.First(players => players.Id == player.Id);
            session.RegisteredSingers.Remove(playerToRemove);
            await SessionService.UpdateSession(session);
            return playerToRemove;
        }
    }
}