using Newtonsoft.Json;

namespace VoicemodPowertools.Domain.Gitlab.Models;

public record GitlabPagination
{
    [JsonProperty("page")] 
    public int Page { get; set; } = 1;

    [JsonProperty("per_page")]
    public int PerPage { get; set; } = 20;
}