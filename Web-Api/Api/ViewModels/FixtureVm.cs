namespace Pickfc.UI.ViewModels
{
    public class FixtureVm
    {
        public int ID { get; set; }
        public int RoundID { get; set; }
        public int HomeID { get; set; }
        public int AwayID { get; set; }
        public string HomeResult { get; set; } = string.Empty;
        public string AwayResult { get; set; } = string.Empty;
        public bool ResultReset { get; set; }
        public DateTime TimeStamped { get; set; }
        public TeamVm Home { get; set; } = new TeamVm();
        public TeamVm Away { get; set; } = new TeamVm();
    }
}
