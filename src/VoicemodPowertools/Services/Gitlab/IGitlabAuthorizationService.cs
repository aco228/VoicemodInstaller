using VoicemodPowertools.Domain.Gitlab.Authorization;

namespace VoicemodPowertools.Services.Gitlab;

public interface IGitlabAuthorizationService
{
    string GetRedirectUrl();
    Task<GitlabTokenResponse> GetToken(string receivedCode);
    Task<GitlabTokenResponse> RefreshToken(string refreshToken);
    Task RevokeToken(string currentToken);
}