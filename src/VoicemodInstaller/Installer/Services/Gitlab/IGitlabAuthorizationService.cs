using Installer.Domain.Gitlab.Authorization.Models;

namespace Installer.Domain.Gitlab.Authorization;

public interface IGitlabAuthorizationService
{
    string GetRedirectUrl();
    Task<GitlabTokenResponse> GetToken(string receivedCode);
    Task<GitlabTokenResponse> RefreshToken(string refreshToken);
}