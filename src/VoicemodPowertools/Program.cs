using VoicemodPowertools.Application;
using VoicemodPowertools.Domain.Installation;
using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Infrastructure.Gitlab;
using VoicemodPowertools.Infrastructure.Http;
using VoicemodPowertools.Infrastructure.Storage;
using VoicemodPowertools.Services.Http;
using VoicemodPowertools.Services.Storage;
using ConsoleManager = VoicemodPowertools.Infrastructure.Consoles.ConsoleManager;

namespace VoicemodPowertools;

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
        
        services.RegisterInstallationServices();
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
