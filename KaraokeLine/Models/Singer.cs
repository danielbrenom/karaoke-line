namespace KaraokeLine.Models
{
    public class Singer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int TimesSang { get; set; }
        public int GlobalOrder { get; set; }
        public bool Penalty { get; set; }
        public bool OptOut { get; set; }
    }
}