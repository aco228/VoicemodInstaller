namespace Installer.Domain.Storage.Entries;

[Serializable]
public class GitlabAuthorization : StorageEntryBase
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public string Username { get; set; }
}