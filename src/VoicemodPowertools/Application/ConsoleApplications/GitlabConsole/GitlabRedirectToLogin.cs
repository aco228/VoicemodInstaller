using System.Diagnostics;
using VoicemodPowertools.Services.Application.ConsoleApplications.GitlabConsole;
using VoicemodPowertools.Services.Gitlab;

namespace VoicemodPowertools.Application.ConsoleApplications.GitlabConsole;

public class GitlabRedirectToLogin : IGitlabRedirectToLogin
{
    private readonly IGitlabAuthorizationService _authorizationService;
    
    public GitlabRedirectToLogin(IGitlabAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }
    
    public async Task Execute(string[] args)
    {
        var openWebpageProcess = new ProcessStartInfo
        {
            FileName = _authorizationService.GetRedirectUrl(),
            UseShellExecute = true
        };
        Process.Start(openWebpageProcess);
        Console.WriteLine("Redirecting to gitlab login page");
    }
}