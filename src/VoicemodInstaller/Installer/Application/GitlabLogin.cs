using Installer.Domain.Gitlab.Authorization;
using Installer.Domain.Storage.Entries;
using Installer.Services.Application;
using Installer.Services.Gitlab;
using Installer.Services.Storage;

namespace Installer.Application;

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