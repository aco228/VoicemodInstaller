using VoicemodPowertools.Application;
using VoicemodPowertools.Domain.Installation;
using VoicemodPowertools.Domain.Storage;
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

        RegisterSecrets(app.Environment.IsDevelopment(), app.Services);
        
        new Thread(() =>
        {
            InitializeServer(app);
        }).Start();
        
        Thread.Sleep(250);
        var consoleManager = new ConsoleManager(args, app.Services);
        consoleManager.Run();
    }
    
    private static void RegisterServices(IServiceCollection services)
    {
        services.AddTransient<IRequestClient, RequestClient>();
        services.AddSingleton<IStorageHandler, StorageHandler>();
        services.AddSingleton<IGitlabSecretsService, GitlabSecretsService>();
        
        services.RegisterInstallationServices();
        services.RegisterGitlabServices();
        services.RegisterApplicationServices();
    }

    private static void InitializeServer(WebApplication app)
    {
        if (app.Environment.IsDevelopment()) { }
        // app.UseHttpsRedirection();
        app.MapControllers();
        app.Run();
    }

    private static void RegisterSecrets(bool isDevelopment, IServiceProvider provider)
    {
        var secretsService = provider.GetService<IGitlabSecretsService>();
        if (!isDevelopment)
        {
            var s = secretsService.Get();
            if (!s.IsValid())
            {
                Console.WriteLine("Error with SConfiguraion. App will close");
                Environment.Exit(1);
                return;
            }
        }
        
        var configuration = provider.GetService<IConfiguration>();
        var clientId = configuration.GetValue<string>("GitlabApplicationId");
        var clientSecret = configuration.GetValue<string>("GitlabApplicationSecret");
        var projectId = configuration.GetValue<long>("GitlabVoicemodDesktopPID");
        
        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret) || projectId == 0)
            return;

        var model = new GitlabSecrets
        {
            ClientId = clientId,
            ClientSecret = clientSecret,
            ProjectId = projectId
        };
        
        secretsService.Save(model);
    }
}
