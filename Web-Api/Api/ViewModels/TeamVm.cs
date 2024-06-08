namespace Pickfc.UI.ViewModels
{
    public class TeamVm
    {
        public int ID { get; set; }
        public int CompID { get; set; }//placeholder identifier
        public string Name { get; set; } = string.Empty;
        public string Pic { get; set; } = string.Empty;
        public int Rating { get; set; }
        public int CompsCount { get; set; }
        public bool Club { get; set; }
        public bool HasFixture { get; set; }
        public List<CompVm> Comps { get; set; } = new List<CompVm>();
        public DateTime Timestamped { get; set; }
    }
}
