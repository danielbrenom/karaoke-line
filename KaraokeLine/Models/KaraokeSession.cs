using System.Collections.Generic;

namespace KaraokeLine.Models
{
    public class KaraokeSession
    {
        public string SessionId { get; set; }
        public int CurrentIteration { get; set; } = 1;
        public List<Singer> RegisteredSingers { get; set; } = new ();
    }
}