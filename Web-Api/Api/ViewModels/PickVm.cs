namespace Pickfc.UI.ViewModels
{
    public class PickVm
    {
        public int ID { get; set; }
        public int RoundID { get; set; }
        public int GameID { get; set; }
        public int PlayerID { get; set; }
        public int TeamID { get; set; }
        public int RoundNumber { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public string PlayerPic { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public DateTime Time { get; set; }
        public DateTime Timestamped { get; set; }
        public TeamVm Team { get; set; } = new TeamVm();
    }
}
