using Humanizer;
using Newtonsoft.Json;
using VoicemodPowertools.Domain.Gitlab.Models;

namespace VoicemodPowertools.Domain.Gitlab.Jobs;

public record JobResponse
{
    [JsonProperty("id")]
    public long Id { get; set; }
    
    [JsonProperty("stage")]
    public string Stage { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("ref")]
    public string Reference { get; set; }
    
    [JsonProperty("user")]
    public GitlabUser User { get; set; }
    
    [JsonProperty("artifacts_file")]
    public JobArtifactResponse ArtifactsFile { get; set; }
    
    [JsonProperty("created_at")]
    public DateTime Created { get; set; }
    
    [JsonProperty("finished_at")]
    public DateTime Finished { get; set; }

    public string IsExpired() => ArtifactsFile == null ? "YES" : "NO";

    public void Print()
    {
        Console.WriteLine("\tFound version:");
        Console.WriteLine($"\t\t{"JobId", 5} - {Id}");
        Console.WriteLine($"\t\t{"UserId", 5} - {User.Username}");
        Console.WriteLine($"\t\t{"Branch", 5} - {Reference}");
        Console.WriteLine($"\t\t{"Created", 5} - {Finished.Humanize()}");
        Console.WriteLine();
    }
}

public record JobArtifactResponse
{
    [JsonProperty("filename")]
    public string FileName { get; set; }
    
    [JsonProperty("size")]
    public long Size { get; set; }
}