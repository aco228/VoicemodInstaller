using VoicemodPowertools.Core.Domain.InternalStorage.Entries;
using VoicemodPowertools.Core.Services.Gitlab;
using VoicemodPowertools.Infrastructure;
using VoicemodPowertools.Services.Application;
using VoicemodPowertools.Services.Gitlab;

namespace VoicemodPowertools.Application.GitlabConsole;

public class GitlabLogin : IGitlabLogin
{
    private readonly IGitlabAuthorizationService _authorizationService;
    private readonly IGitlabUserService _gitlabUserService;
    private readonly IGitlabAuthorization _gitlabAuthorization;
    private readonly IGitlabSecretsService _gitlabSecrets;
    private readonly IGitlabProjectsService _projectsService;
    
    public GitlabLogin(
        IGitlabAuthorizationService authorizationService,
        IGitlabUserService userService,
        IGitlabAuthorization gitlabAuthorization,
        IGitlabSecretsService gitlabSecretsService,
        IGitlabProjectsService projectsService)
    {
        _authorizationService = authorizationService;
        _gitlabUserService = userService;
        _gitlabAuthorization = gitlabAuthorization;
        _gitlabSecrets = gitlabSecretsService;
        _projectsService = projectsService;
    }
    
    public async Task PerformLogin(string code, string state)
    {
        var authorization = new GitlabAuthorization();
        try
        {
            var response = await _authorizationService.GetToken(code);
            authorization.Token = response.AccessToken;
            authorization.RefreshToken = response.RefreshToken;
            _gitlabAuthorization.Save(authorization);
            Console.WriteLine("Received token: " + response.AccessToken);
            
            var user = await _gitlabUserService.GetUser();
            authorization.Username = user.Username;
            Console.WriteLine($"Logged in as {user.Username}");

            Console.WriteLine($"Checking access to Voicemod...");
            var project = await _projectsService.GetProject(_gitlabSecrets.Get().ProjectId);
            Console.WriteLine($"Confirmed access to {project.PathWithNamespace}");
            Console.WriteLine($"That is great news :)");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving token");
            authorization.EmptyData();
            ConsoleDebug.WriteLine(ex.ToString());
        }
        finally
        {
            _gitlabAuthorization.Save(authorization);
            Console.WriteLine();
        }
    }
}
