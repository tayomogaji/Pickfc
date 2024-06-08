namespace Pickfc.UI.ViewModels
{
    public class AuthVm
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool ActivationCode { get; set; }
        public bool RememberMe { get; set; }
    }
}
