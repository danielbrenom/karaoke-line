using System.Collections.Generic;
using System.Threading.Tasks;
using KaraokeLine.Models;

namespace KaraokeLine.Interfaces
{
    public interface IPlayerService
    {
        public Task<IReadOnlyCollection<Singer>> ListPlayers(string sessionId);
        public Task<Singer> AddPlayer(string sessionId, Singer player);
        public Task<Singer> RemovePlayer(string sessionId, Singer player);
    }
}