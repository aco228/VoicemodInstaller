namespace VoicemodPowertools.Domain.Storage.Entries;

[Serializable]
public class GitlabSecrets : StorageEntryBase
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public long ProjectId { get; set; }

    public void Print()
    {
        Console.WriteLine($"ClientID: {ClientId}");
        Console.WriteLine($"ClientSecret: {ClientSecret}");
        Console.WriteLine($"ProjectId: {ProjectId}");
    }    
}

public static class GitlabSecretsExtensions
{
    public static bool IsValid(this GitlabSecrets? secrets)
    {
        return secrets != null && !string.IsNullOrEmpty(secrets.ClientId) && !string.IsNullOrEmpty(secrets.ClientSecret) && secrets.ProjectId != 0;
    }
}