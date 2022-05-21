using Installer.Domain.Storage.Entries;
using Installer.Services.Application.ConsoleApplications.GitlabConsole;
using Installer.Services.Storage;

namespace Installer.Application.ConsoleApplications.GitlabConsole;

public class GetCurrentGetCurrentGitlabUser : IGetCurrentGitlabUser
{
    private readonly IStorageHandler _storageHandler;
    
    public GetCurrentGetCurrentGitlabUser(IStorageHandler storageHandler)
    {
        _storageHandler = storageHandler;
    }
    
    public async Task Execute(string[] args)
    {
        var auth = _storageHandler.Get<GitlabAuthorization>();
        if (!auth.IsValid())
        {
            Console.WriteLine("You are not logged in");
            return;
        }
        
        Console.WriteLine($"{"Token", 15} : {auth.Token}");
        Console.WriteLine($"{"Username", 15} : {auth.Username}");
    }
}