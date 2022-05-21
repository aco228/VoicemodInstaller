namespace Installer.Services.Application;

public interface IGitlabLogin
{
    Task PerformLogin(string code, string state);
}