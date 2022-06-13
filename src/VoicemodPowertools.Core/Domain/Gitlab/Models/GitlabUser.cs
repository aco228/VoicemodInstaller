using Newtonsoft.Json;

namespace VoicemodPowertools.Core.Domain.Gitlab.Models;

public record GitlabUser
{
    [JsonProperty("id")]
    public long Id { get; set; }
    
    [JsonProperty("username")]
    public string Username { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
}