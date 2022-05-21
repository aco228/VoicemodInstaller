using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Infrastructure.Http;
using VoicemodPowertools.Services.Application.ConsoleApplications.GitlabConsole;
using VoicemodPowertools.Services.Gitlab;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Application.ConsoleApplications.GitlabConsole;

public class GitlabLogout : IGitlabLogout
{
    private readonly IStorageHandler _storageHandler;
    private readonly IGitlabAuthorizationService _authorizationService;
    
    public GitlabLogout(
        IStorageHandler storageHandler,
        IGitlabAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
        _storageHandler = storageHandler;
    }
    
    public async Task Execute(string[] args)
    {
        var auth = _storageHandler.Get<GitlabAuthorization>();
        if (!auth.IsValid())
        {
            Console.WriteLine("You are not logged in currently");
            return;
        }

        try
        {
            await _authorizationService.RevokeToken(auth.Token);
            auth.EmptyData();
            _storageHandler.Save(auth);
            Console.WriteLine("You logged out");
        }
        catch (RequestException ex)
        {
            Console.WriteLine($"Error login out");
        }
    }
}