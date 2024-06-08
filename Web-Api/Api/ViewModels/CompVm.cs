namespace Pickfc.UI.ViewModels
{
    public class CompVm
    {
        public int ID { get; set; }
        public int AdminID { get; set; }
        public int RoundID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Pic { get; set; } = string.Empty;
        public int TeamsCount { get; set; }
        public int TeamsRemaining { get; set; }
        public int TeamsTotal { get; set; }
        public bool Legacy { get; set; }
        public bool Active { get; set; }
        public bool HasTeam { get; set; }
        public bool Default { get; set; }
        public bool OpenNotified { get; set; }
        public List<TeamVm> Teams { get; set; } = new List<TeamVm>();
        public DateTime Open { get; set; }
        public DateTime Timestamped { get; set; }
        public UserVm Admin { get; set; } = new UserVm();
    }
}
