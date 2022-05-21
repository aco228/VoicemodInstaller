using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Infrastructure.Consoles;
using VoicemodPowertools.Services.Application;
using VoicemodPowertools.Services.Gitlab;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Application;

public class GitlabLogin : IGitlabLogin
{
    private readonly IGitlabAuthorizationService _authorizationService;
    private readonly IGitlabUserService _gitlabUserService;
    private readonly IStorageHandler _storageHandler;
    
    public GitlabLogin(
        IGitlabAuthorizationService authorizationService,
        IGitlabUserService userService,
        IStorageHandler storageHandler)
    {
        _authorizationService = authorizationService;
        _gitlabUserService = userService;
        _storageHandler = storageHandler;
    }
    
    public async Task PerformLogin(string code, string state)
    {
        var gitlabAuthorization = await GetToken(code);
        if (gitlabAuthorization == null)
            return;

        await GetUser(gitlabAuthorization);
        Console.WriteLine();
    }

    private async Task<GitlabAuthorization?> GetToken(string code)
    {
        try
        {
            var response = await _authorizationService.GetToken(code);
            var gitlabAuthorization = new GitlabAuthorization
            {
                Token = response.AccessToken,
                RefreshToken = response.RefreshToken
            };

            Console.WriteLine("Received token: " + response.AccessToken);
            _storageHandler.Save(gitlabAuthorization);
            return gitlabAuthorization;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error retrieving token");
            return null;
        }
    }

    private async Task GetUser(GitlabAuthorization auth)
    {
        try
        {
            var user = await _gitlabUserService.GetUser();
            auth.Username = user.Username;
            Console.WriteLine($"Logged in as {user.Username}");
        }
        catch (Exception ex)
        {
            auth.EmptyData();
            Console.WriteLine($"Error getting user data");
        }
        finally
        {
            _storageHandler.Save(auth);
        }
    }
}