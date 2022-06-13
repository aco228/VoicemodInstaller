using Newtonsoft.Json;

namespace VoicemodPowertools.Core.Domain.InternalStorage;

[Serializable]
public class WriteFileModel
{
    [JsonProperty("s")]
    public string Sha { get; set; }
    
    [JsonProperty("fc")]
    public string FileContent { get; set; }
}