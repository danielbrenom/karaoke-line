using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using KaraokeLine.Interfaces;
using KaraokeLine.Models;

namespace KaraokeLine.Services
{
    public class SessionService : ISessionService
    {
        private ICache Cache { get; }

        public SessionService(ICache cache)
        {
            Cache = cache;
        }

        public Task<KaraokeSession> InitializeSession()
        {
            return Task.Run(() =>
            {
                var session = new KaraokeSession
                {
                    SessionId = Guid.NewGuid().ToString("N"),
                    RegisteredSingers = new List<Singer>()
                };
                Cache.AddUpdateCache(session.SessionId, session, TimeSpan.FromHours(1));
                return session;
            });
        }

        public Task<KaraokeSession> RetrieveSession(string sessionId)
        {
            return Task.Run(() => Cache.GetCache<KaraokeSession>(sessionId));
        }

        public Task<KaraokeSession> UpdateSession(KaraokeSession session)
        {
            return Task.Run(() =>
            {
                Cache.AddUpdateCache(session.SessionId, session, TimeSpan.FromHours(1));
                return session;
            });
        }

        public async Task SaveSessionBackup(KaraokeSession session)
        {
            var sessionData = JsonSerializer.Serialize(session);
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            await using var arquivo = File.Open(Path.Combine(folderPath, $"karaokeBackup{DateTime.Now:ddMMyyyy}.txt"), FileMode.OpenOrCreate);
            await arquivo.WriteAsync(Encoding.UTF8.GetBytes(sessionData));
        }

        public async Task<KaraokeSession> LoadSessionBackup()
        {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            await using var arquivo = File.Open(Path.Combine(folderPath, $"karaokeBackup{DateTime.Now:ddMMyyyy}.txt"), FileMode.OpenOrCreate);
            using var reader = new StreamReader(arquivo);
            var dados = await reader.ReadToEndAsync();
            return JsonSerializer.Deserialize<KaraokeSession>(dados);
        }

        public bool ExistSessionBackup()
        {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return File.Exists(Path.Combine(folderPath, $"karaokeBackup{DateTime.Now:ddMMyyyy}.txt"));
        }
    }
}