using System.Collections.Generic;
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
                    SessionId = System.Guid.NewGuid().ToString("N"),
                    RegisteredSingers = new List<Singer>()
                };
                Cache.AddUpdateCache(session.SessionId, session, System.TimeSpan.FromHours(1));
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
                Cache.AddUpdateCache(session.SessionId, session, System.TimeSpan.FromHours(1));
                return session;
            });
        }
    }
}