using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Services.Application;
using VoicemodPowertools.Services.Gitlab;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Application;

public class GitlabRefreshToken : IGitlabRefreshToken
{
    private readonly IGitlabAuthorization _authorization;
    private readonly IGitlabAuthorizationService _gitlabAuthorizationService;
    
    public GitlabRefreshToken(
        IGitlabAuthorization gitlabAuthorization,
        IGitlabAuthorizationService gitlabAuthorizationService)
    {
        _authorization = gitlabAuthorization;
        _gitlabAuthorizationService = gitlabAuthorizationService;
    }
    
    public async Task RefreshToken()
    {
        var auth = _authorization.GetAuthorization();
        if (!auth.IsValid())
        {
            Console.WriteLine("Cannot refresh when user is not logged in");
            return;
        }

        try
        {
            var newToken = await _gitlabAuthorizationService.RefreshToken(auth.RefreshToken);
            auth.Token = newToken.AccessToken;
            auth.RefreshToken = newToken.RefreshToken;
            _authorization.Save(auth);

            Console.WriteLine($"Refresh token: {auth.Token}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error refreshing token, {ex}");
        }
    }
}