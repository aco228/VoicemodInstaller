using Newtonsoft.Json;

namespace VoicemodPowertools.Domain.Gitlab.Authorization;

public record GitlabTokenRequest
{
    [JsonProperty("client_id")]
    public string ClientId { get; set; }
    
    [JsonProperty("code")]
    public string Code { get; set; }
    
    [JsonProperty("grant_type")]
    public string GrantType { get; set; } = "authorization_code";
    
    [JsonProperty("redirect_uri")]
    public string RedirectUrl { get; set; }
    
    [JsonProperty("code_verifier")]
    public string CodeVerifier { get; set; }
}