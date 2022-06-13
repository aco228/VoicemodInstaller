using VoicemodPowertools.Core.Domain.InternalStorage.Entries;
using VoicemodPowertools.Core.Infrastructure.Http;
using VoicemodPowertools.Core.Services.Gitlab;
using VoicemodPowertools.Services.Application.GitlabConsole;

namespace VoicemodPowertools.Application.GitlabConsole;

public class GitlabLogout : IGitlabLogout
{
    private readonly IGitlabAuthorization _gitlabAuthorization;
    private readonly IGitlabAuthorizationService _authorizationService;
    
    public GitlabLogout(
        IGitlabAuthorization gitlabAuthorization,
        IGitlabAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
        _gitlabAuthorization = gitlabAuthorization;
    }
    
    public async Task Execute(string[] args)
    {
        var auth = _gitlabAuthorization.GetCurrent();
        if (!auth.IsValid())
        {
            Console.WriteLine("You are not logged in currently");
            return;
        }

        try
        {
            await _authorizationService.RevokeToken(auth.Token);
            _gitlabAuthorization.Clear();
            Console.WriteLine("You logged out");
        }
        catch (RequestException ex)
        {
            Console.WriteLine($"Error login out");
        }
    }
}