namespace Pickfc.Model.DTOs
{
    public class UserToken
    {
        public string? Token { get; set; }
        public DateTime? Created { get; set; }
        public DateTime Expires { get; set; }
    }
}
