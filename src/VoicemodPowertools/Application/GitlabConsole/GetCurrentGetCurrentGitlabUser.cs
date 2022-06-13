using VoicemodPowertools.Core.Domain.InternalStorage.Entries;
using VoicemodPowertools.Core.Services.Gitlab;
using VoicemodPowertools.Services.Application.GitlabConsole;

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
        
        auth.Print();
    }
}