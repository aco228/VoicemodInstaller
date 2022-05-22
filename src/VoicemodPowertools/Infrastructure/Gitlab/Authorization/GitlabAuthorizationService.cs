using System.Web;
using VoicemodPowertools.Domain.Gitlab.Authorization;
using VoicemodPowertools.Infrastructure.Helpers;
using VoicemodPowertools.Services.Gitlab;
using VoicemodPowertools.Services.Http;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Infrastructure.Gitlab.Authorization;

public class GitlabAuthorizationService : IGitlabAuthorizationService
{
    private readonly IConfiguration _configuration;
    private readonly IRequestClient _requestClient;
    private readonly IGitlabSecretsService _gitlabSecrets;
    
    public GitlabAuthorizationService(
        IConfiguration configuration,
        IRequestClient requestClient,
        IGitlabSecretsService gitlabSecretsService)
    {
        _configuration = configuration;
        _requestClient = requestClient;
        _gitlabSecrets = gitlabSecretsService;
    }

    public string GetRedirectUrl()
    {
        var gitlabBaseUrl = _configuration.GetValue<string>("GitlabBaseUrl");
        var scopes = "api";
        var endpoint = _configuration.GetSection("Kestrel:Endpoints:Https").GetValue<string>("Url");
        var codeVerifier = "ks02i3jdikdo2k0dkfodf3m39rjfjsdk0wk349rj3jrhf".Base64Encode();
        
        return gitlabBaseUrl + _configuration.GetValue<string>("GitlabRedirect")
            .Replace("[APP_ID]", _gitlabSecrets.Get().ClientId)
            .Replace("[REDIRECT_URI]", HttpUtility.UrlEncode($"{endpoint}/GitlabCallback/"))
            .Replace("[STATE]", "state")
            .Replace("[REQUESTED_SCOPES]", scopes)
            .Replace("[CODE_CHALLENGE]", codeVerifier);
    }

    public Task<GitlabTokenResponse> GetToken(string receivedCode)
    {
        var gitlabBaseUrl = _configuration.GetValue<string>("GitlabBaseUrl");
        var endpoint = _configuration.GetSection("Kestrel:Endpoints:Https").GetValue<string>("Url");
        
        var request = new GitlabTokenRequest
        {
            Code = receivedCode,
            ClientId = _gitlabSecrets.Get().ClientId,
            RedirectUrl = $"{endpoint}/GitlabCallback/",
            CodeVerifier = "ks02i3jdikdo2k0dkfodf3m39rjfjsdk0wk349rj3jrhf",
        };

        return _requestClient.Post<GitlabTokenResponse>($"{gitlabBaseUrl}oauth/token", request);
    }

    public Task<GitlabTokenResponse> RefreshToken(string refreshToken)
    {
        var gitlabBaseUrl = _configuration.GetValue<string>("GitlabBaseUrl");
        var endpoint = _configuration.GetSection("Kestrel:Endpoints:Https").GetValue<string>("Url");
        
        var request = new GitlabRefreshTokenRequest
        {
            ClientId = _gitlabSecrets.Get().ClientId,
            ClientSecret = _gitlabSecrets.Get().ClientSecret,
            RefreshToken = refreshToken,
            GrantType = "refresh_token",
            RedirectUrl = $"{endpoint}/GitlabCallback/",
        };
        return _requestClient.Post<GitlabTokenResponse>($"{gitlabBaseUrl}oauth/token", request);
    }

    public Task RevokeToken(string currentToken)
    {
        var gitlabBaseUrl = _configuration.GetValue<string>("GitlabBaseUrl");

        var request = new GitlabRevokeRequest
        {
            ClientId = _gitlabSecrets.Get().ClientId,
            ClientSecret = _gitlabSecrets.Get().ClientSecret,
            Token = currentToken,
        };
        return _requestClient.Post($"{gitlabBaseUrl}oauth/revoke", request);
    }
}