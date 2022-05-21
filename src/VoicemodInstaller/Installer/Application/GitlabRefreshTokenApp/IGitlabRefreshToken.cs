namespace Installer.Application.GitlabRefreshTokenApp;

public interface IGitlabRefreshToken
{
    Task RefreshToken();
}