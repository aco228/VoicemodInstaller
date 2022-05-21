using Microsoft.AspNetCore.Mvc;
using VoicemodPowertools.Services.Application;

namespace VoicemodPowertools.Infrastructure.Controllers;

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