using Microsoft.AspNetCore.Mvc;
using VoicemodPowertools.Core.Services.Gitlab;
using VoicemodPowertools.Services.Gitlab;

namespace VoicemodPowertools.Infrastructure.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private IGitlabAuthorizationService _gitlabAuthorization;

    
    public TestController(IGitlabAuthorizationService gitlabAuthorizationService)
    {
        _gitlabAuthorization = gitlabAuthorizationService;
    }
    
    [HttpGet]
    public string Index()
    {
        return _gitlabAuthorization.GetRedirectUrl();
    }
}