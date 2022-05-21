using Installer.Domain.Gitlab.Authorization;
using Installer.Domain.Storage;
using Installer.Domain.Storage.Entries;
using Installer.Services.Gitlab;
using Installer.Services.Storage;

namespace Installer.Application.GitlabLoginApp;

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
        var response = await _authorizationService.GetToken(code);

        var gitlabAuthorization = new GitlabAuthorization
        {
            Token = response.AccessToken,
            RefreshToken = response.RefreshToken
        };
        
        Console.WriteLine("TK " + response.AccessToken);
        _storageHandler.Save(gitlabAuthorization);

        var user = await _gitlabUserService.GetUser();
        gitlabAuthorization.Username = user.Username;
        
        _storageHandler.Save(gitlabAuthorization);
        
        Console.WriteLine($"Logged in as {user.Username}");;
    }
}