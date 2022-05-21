namespace Installer.Services.Application;

public interface IGitlabRefreshToken
{
    Task RefreshToken();
}