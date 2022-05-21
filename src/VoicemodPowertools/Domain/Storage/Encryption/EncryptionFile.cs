using Newtonsoft.Json;

namespace VoicemodPowertools.Domain.Storage.Encryption;

[Serializable]
public class EncryptionFile
{
    [JsonProperty("sh")]
    public string Sha { get; set; }
    
    [JsonProperty("fc")]
    public string FileContent { get; set; }
}