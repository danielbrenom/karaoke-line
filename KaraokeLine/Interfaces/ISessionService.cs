using System.Threading.Tasks;
using KaraokeLine.Models;

namespace KaraokeLine.Interfaces
{
    public interface ISessionService
    {
        public Task<KaraokeSession> InitializeSession();
        public Task<KaraokeSession> RetrieveSession(string sessionId);
        public Task<KaraokeSession> UpdateSession(KaraokeSession session);
        public Task SaveSessionBackup(KaraokeSession session);
        public Task<KaraokeSession> LoadSessionBackup();
        public bool ExistSessionBackup();

    }
}