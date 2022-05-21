using Installer.Domain.Gitlab.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Installer.Infrastructure.Controllers;

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