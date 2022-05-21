namespace Installer.Services.Application.ConsoleApplications;

public interface IConsoleApplication
{
    Task Execute(string[] args);
}