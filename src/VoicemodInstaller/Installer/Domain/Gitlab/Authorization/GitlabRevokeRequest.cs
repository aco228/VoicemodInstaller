using Newtonsoft.Json;

namespace Installer.Domain.Gitlab.Authorization.Models;

public record GitlabRevokeRequest
{
    [JsonProperty("client_id")]
    public string ClientId { get; set; }
    
    [JsonProperty("client_secret")]
    public string ClientSecret { get; set; }
    
    [JsonProperty("client_token")]
    public string Token { get; set; }
}