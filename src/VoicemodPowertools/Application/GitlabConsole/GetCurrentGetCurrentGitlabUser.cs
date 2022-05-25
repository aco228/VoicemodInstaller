using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Services.Application.GitlabConsole;
using VoicemodPowertools.Services.Gitlab;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Application.GitlabConsole;

public class GetCurrentGetCurrentGitlabUser : IGetCurrentGitlabUser
{
    private readonly IGitlabAuthorization _authorization;
    
    public GetCurrentGetCurrentGitlabUser(IGitlabAuthorization gitlabAuthorization)
    {
        _authorization = gitlabAuthorization;
    }
    
    public async Task Execute(string[] args)
    {
        var auth = _authorization.GetCurrent();
        if (!auth.IsValid())
        {
            Console.WriteLine("You are not logged in");
            return;
        }
        
        Console.WriteLine($"{"Token", 15} : {auth.Token}");
        Console.WriteLine($"{"Username", 15} : {auth.Username}");
    }
}