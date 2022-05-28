using Newtonsoft.Json;

namespace VoicemodPowertools.Domain.Gitlab.Projects;

public record ProjectResponse
{
    [JsonProperty("id")]
    public long Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("name_with_namespace")]
    public string Namespace { get; set; }
    
    [JsonProperty("path_with_namespace")]
    public string PathWithNamespace { get; set; }
}