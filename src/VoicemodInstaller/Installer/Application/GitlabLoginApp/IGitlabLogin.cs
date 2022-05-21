namespace Installer.Application.GitlabLoginApp;

public interface IGitlabLogin
{
    Task PerformLogin(string code, string state);
}