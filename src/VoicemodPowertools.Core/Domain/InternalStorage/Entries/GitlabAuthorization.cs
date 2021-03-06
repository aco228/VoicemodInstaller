using VoicemodPowertools.Domain.Storage;

namespace VoicemodPowertools.Core.Domain.InternalStorage.Entries;

[Serializable]
public class GitlabAuthorization : StorageEntryBase
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public string Username { get; set; }

    public void EmptyData()
    {
        Token = string.Empty;
        RefreshToken = string.Empty;
        Username = string.Empty;
    }

    public void Print()
    {
        Console.WriteLine($"{"Token", 15} : {Token}");
        Console.WriteLine($"{"Username", 15} : {Username}");
        Console.WriteLine($"{"RefreshToken", 15} : {RefreshToken}");
    }
}

public static class GitlabAuthorizationExtensions
{
    public static bool IsValid(this GitlabAuthorization? authorization)
    {
        return (authorization != null &&
                !string.IsNullOrEmpty(authorization.Token)
                && !string.IsNullOrEmpty(authorization.RefreshToken)
                && !string.IsNullOrEmpty(authorization.Username));
    }

    public static bool IsTokenValid(this GitlabAuthorization? authorization)
    {

        return (authorization != null &&
                !string.IsNullOrEmpty(authorization.Token)
                && !string.IsNullOrEmpty(authorization.RefreshToken));
    }
}