namespace Pickfc.UI.ViewModels
{
    public class GameVm
    {
        public int ID { get; set; }
        public int CreatorID { get; set; }
        public int CompID { get; set; }
        public int Bonus { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Pic { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool Public { get; set; }
        public bool Legacy { get; set; }
        public bool Active { get; set; }
        public bool RoundDeadlined { get; set; }
        public bool Deadline { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public DateTime Timestamped { get; set; }
        public CompVm Comp { get; set; } = new CompVm();
        public UserVm Creator { get; set; } = new UserVm();
        public PlayerVm CurrentPlayer { get; set; } = new PlayerVm();
        public List<PlayerVm> Players { get; set; } = new List<PlayerVm>();
        public List<PlayerVm> Admins { get; set; } = new List<PlayerVm>();
    }
}
