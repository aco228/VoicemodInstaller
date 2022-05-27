using VoicemodPowertools.Domain.Github;
using VoicemodPowertools.Infrastructure.Http;
using VoicemodPowertools.Services.Github;

namespace VoicemodPowertools.Infrastructure.Github;

public class GithubReleaseService : IGithubReleaseService
{
    private readonly IConfiguration _configuration;
    
    public GithubReleaseService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<GithubReleaseResponse> GetLatestRelease()
    {
        var baseUrl = _configuration.GetValue<string>("GithubBaseUrl");
        var owner = _configuration.GetValue<string>("GithubOwner");
        var repo = _configuration.GetValue<string>("GithubRepo");
        
        var httpClient = new RequestClient(baseUrl);
        httpClient.AddDefaultHeader("User-Agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 11_3_1 like Mac OS X) AppleWebKit/603.1.30 (KHTML, like Gecko)");
        
        var list = await httpClient.Get<List<GithubReleaseResponse>>($"repos/{owner}/{repo}/releases?per_page=1");
        return list.FirstOrDefault();
    }
}