using Newtonsoft.Json;

namespace VoicemodPowertools.Core.Domain.Gitlab.Authorization;

public record GitlabRefreshTokenRequest
{
    [JsonProperty("client_id")]
    public string ClientId { get; set; }
    
    [JsonProperty("client_secret")]
    public string ClientSecret { get; set; }
    
    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; }
    
    [JsonProperty("grant_type")]
    public string GrantType { get; set; } = "authorization_code";
    
    [JsonProperty("redirect_uri")]
    public string RedirectUrl { get; set; }
}