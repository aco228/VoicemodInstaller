using Installer.Application.GitlabLoginApp;
using Microsoft.AspNetCore.Mvc;

namespace Installer.Infrastructure.Controllers;

[Route("gitlabCallback")]
public class GitlabCallbackController : ControllerBase
{
    private readonly IGitlabLogin _gitlabLogin;

    public GitlabCallbackController(IGitlabLogin gitlabLogin)
    {
        _gitlabLogin = gitlabLogin;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index(string code, string state)
    {
        await _gitlabLogin.PerformLogin(code, state);
        return Content("");
    }
}