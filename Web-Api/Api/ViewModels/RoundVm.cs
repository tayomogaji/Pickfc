namespace Pickfc.UI.ViewModels
{
    public class RoundVm
    {
        public int ID { get; set; }
        public int CompID { get; set; }
        public int Number { get; set; }
        public int DeadlineMsecs { get; set; }
        public int DeadlineDays { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Show { get; set; }
        public bool Current { get; set; }
        public bool StartNotified { get; set; }
        public bool DeadlineNotified { get; set; }
        public DateTime Start { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime Timestamped { get; set; }
        public List<FixtureVm> Fixtures { get; set; } = new List<FixtureVm>();
        public List<PickVm> Picks { get; set; } = new List<PickVm>();
    }
}
