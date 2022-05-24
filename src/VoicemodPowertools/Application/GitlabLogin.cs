using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Infrastructure;
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
    private readonly IGitlabJobService _jobService;
    private readonly IGitlabSecretsService _gitlabSecrets;
    
    public GitlabLogin(
        IGitlabAuthorizationService authorizationService,
        IGitlabUserService userService,
        IStorageHandler storageHandler,
        IGitlabJobService gitlabJobService,
        IGitlabSecretsService gitlabSecretsService)
    {
        _authorizationService = authorizationService;
        _gitlabUserService = userService;
        _storageHandler = storageHandler;
        _jobService = gitlabJobService;
        _gitlabSecrets = gitlabSecretsService;
    }
    
    public async Task PerformLogin(string code, string state)
    {
        var authorization = new GitlabAuthorization();
        try
        {
            var response = await _authorizationService.GetToken(code);
            authorization.Token = response.AccessToken;
            authorization.RefreshToken = response.RefreshToken;
            _storageHandler.Save(authorization);
            Console.WriteLine("Received token: " + response.AccessToken);
            
            var user = await _gitlabUserService.GetUser();
            authorization.Username = user.Username;
            Console.WriteLine($"Logged in as {user.Username}");

            // TODO: Make better
            Console.WriteLine($"Checking access to Voicemod...");
            await _jobService.GetJobs(_gitlabSecrets.Get().ProjectId, 1, false).FirstOrDefault();
            Console.WriteLine($"You have access to Voicemod. That is great news :)");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error retrieving token");
            authorization.EmptyData();
        }
        finally
        {
            _storageHandler.Save(authorization);
            Console.WriteLine();
        }
    }
}
