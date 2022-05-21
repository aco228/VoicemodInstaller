using System.Diagnostics;
using Installer.Application;
using Installer.Domain.Gitlab.Authorization;
using Installer.Domain.Gitlab.Jobs;
using Installer.Domain.Storage;
using Installer.Domain.Storage.Entries;
using Installer.Infrastructure.Consoles;
using Installer.Infrastructure.Gitlab;
using Installer.Infrastructure.Gitlab.Authorization;
using Installer.Infrastructure.Gitlab.Jobs;
using Installer.Infrastructure.Http;
using Installer.Infrastructure.Storage;
using Installer.Services.Storage;

namespace Installer;

static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        RegisterServices(builder.Services);
        var app = builder.Build();
        
        new Thread(() =>
        {
            var consoleManager = new ConsoleManager(args, app.Services);
            consoleManager.Run();
        }).Start();
        InitializeServer(app);
    }
    
    private static void RegisterServices(IServiceCollection services)
    {
        services.AddTransient<IRequestClient, RequestClient>();
        services.AddSingleton<IStorageHandler, StorageHandler>();
        
        services.RegisterGitlabServices();
        services.RegisterApplicationServices();
    }

    private static void InitializeServer(WebApplication app)
    {
        if (app.Environment.IsDevelopment()) { }
        app.UseHttpsRedirection();
        //app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }

    private static bool CheckIfUserIsLoggedIn(IServiceProvider serviceProvider)
    {
        var storageHandler = new StorageHandler();
        var data = storageHandler.Get<GitlabAuthorization>();
        if (data == null)
        {
            return false;
        }
        
        Console.WriteLine($"Logged in as ${data.Username}");
        
        
        return true;
    }
}
