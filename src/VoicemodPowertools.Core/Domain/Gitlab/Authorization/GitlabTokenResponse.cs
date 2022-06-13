using Newtonsoft.Json;

namespace VoicemodPowertools.Core.Domain.Gitlab.Authorization;

public record GitlabTokenResponse
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
    
    [JsonProperty("token_type")]
    public string TokenType { get; set; }
    
    [JsonProperty("expires_in")]
    public long ExpiresIn { get; set; }
    
    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; }
    
    [JsonProperty("created_at")]
    public long CreatedAtTimestamp { get; set; }
}