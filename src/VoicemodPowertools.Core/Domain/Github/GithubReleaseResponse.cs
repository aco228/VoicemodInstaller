using Newtonsoft.Json;

namespace VoicemodPowertools.Core.Domain.Github;

public record GithubReleaseResponse
{
    [JsonProperty("tag_name")]
    public string Version { get; set; }
    
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [JsonProperty("assets")]
    public List<GithubReleaseAssetResponse> Assets { get; set; }
}

public record GithubReleaseAssetResponse
{
    [JsonProperty("browser_download_url")]
    public string Url { get; set; }
}