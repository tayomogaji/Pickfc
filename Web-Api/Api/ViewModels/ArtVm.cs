namespace Pickfc.UI.ViewModels
{
    public class ArtVm
    {
        public int ID { get; set; }
        public int Index { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public DateTime Timestamped { get; set; }
    }
}
