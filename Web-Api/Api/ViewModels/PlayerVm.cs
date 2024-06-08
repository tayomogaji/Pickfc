namespace Pickfc.UI.ViewModels
{
    public class PlayerVm
    {
        public int ID { get; set; }
        public int PickID { get; set; }
        public int GameID { get; set; }
        public int HitByID { get; set; }
        public int HitsTotal { get; set; }
        public int HitsPlayed { get; set; }
        public int BoostTotal { get; set; }
        public int BoostPlayed { get; set; }
        public int Life { get; set; }
        public int Pos { get; set; }
        public int Pts { get; set; }
        public int RoundPts { get; set; }
        public int Streak { get; set; }
        public int Champs { get; set; }
        public double PickTime { get; set; }
        public double RoundPickTime { get; set; }
        public string Name { get; set; } = string.Empty;
        public string HitByName { get; set; } = string.Empty;
        public bool Eliminated { get; set; }
        public bool Admin { get; set; }
        public bool Active { get; set; }
        public DateTime Timestamped { get; set; }
        public UserVm User { get; set; } = new UserVm();
        public PickVm Pick { get; set; } = new PickVm();
        public List<PickVm> Picks { get; set; } = new List<PickVm>();
    }
}
