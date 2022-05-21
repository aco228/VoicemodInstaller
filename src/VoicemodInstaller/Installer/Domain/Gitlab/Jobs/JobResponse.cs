﻿using Installer.Domain.Gitlab.Models;
using Newtonsoft.Json;

namespace Installer.Domain.Gitlab.Jobs.Models;

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
}

public record JobArtifactResponse
{
    [JsonProperty("filename")]
    public string FileName { get; set; }
    
    [JsonProperty("size")]
    public long Size { get; set; }
}