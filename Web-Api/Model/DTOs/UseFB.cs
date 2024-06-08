using Newtonsoft.Json;

namespace Pickfc.UI.ViewModels
{
    public class UserFB
    {
        [JsonProperty("data")] public Data Data { get; set; } = new Data();
    }

    public partial class Data {
        [JsonProperty("app_id")] public string AppId { get; set; } = string.Empty;

        [JsonProperty("type")] public string Type { get; set; } = string.Empty;

        [JsonProperty("application")] public string Application { get; set; } = string.Empty;

        [JsonProperty("data_access_expires_at")] public long DataAccessExpiresAt { get; set; }

        [JsonProperty("expires_at")]  public long ExpiresAt { get; set; }

        [JsonProperty("is_valid")] public bool IsValid { get; set; }

        [JsonProperty("scopes")] public List<string> Scopes { get; set; } = new List<string>();

        [JsonProperty("user_id")] public string UserId { get; set; } = string.Empty; 
    }
}
