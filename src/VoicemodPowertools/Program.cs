using System.Reflection;
using VoicemodPowertools.Application;
using VoicemodPowertools.Application.Github;
using VoicemodPowertools.Domain;
using VoicemodPowertools.Domain.Installation;
using VoicemodPowertools.Domain.Storage;
using VoicemodPowertools.Domain.Storage.Entries;
using VoicemodPowertools.Infrastructure.Consoles;
using VoicemodPowertools.Infrastructure.Github;
using VoicemodPowertools.Infrastructure.Gitlab;
using VoicemodPowertools.Infrastructure.Http;
using VoicemodPowertools.Infrastructure.InternalStorage;
using VoicemodPowertools.Infrastructure.Storage;
using VoicemodPowertools.Services.Http;
using VoicemodPowertools.Services.InternalStorage;
using VoicemodPowertools.Services.Storage;
using ConsoleManager = VoicemodPowertools.Infrastructure.Consoles.ConsoleManager;
using StorageManager = VoicemodPowertools.Infrastructure.Storage.StorageManager;

namespace VoicemodPowertools;

static class Program
{
    private static bool _isDebug = false;
    private static string _absoluthLocation = string.Empty;
    public static bool OnDebug { get => _isDebug; }
    public static string Location { get => _absoluthLocation; }
    
    public static void Main(string[] args)
    {
        _absoluthLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        RegisterServices(builder.Services);
        var app = builder.Build();

        RegisterSecrets(app.Environment.IsDevelopment(), app.Services);

        _isDebug = (!args.ValueExists(ProgramConstants.DebugAttribute) && app.Environment.IsDevelopment()) 
                   || ConsoleArgumentsExtensions.GetValueAndRemove(ref args, "debug", false);
        
        if (!args.GetValue(ProgramConstants.IgnoreAttribute, false))
        {
            new Thread(() => InitializeServer(app)).Start();
            new Thread( () =>  app.Services.GetService<ICheckForNewRelease>().Run()).Start();
        }

        Thread.Sleep(150);
        var consoleManager = new ConsoleManager(args, app.Services);
        consoleManager.Run();
    }
    
    private static void RegisterServices(IServiceCollection services)
    {
        services.AddTransient<IDownloadClient, DownloadClient>();
        services.AddTransient<IZipStorage, ZipStorage>();
        services.AddTransient<ICryptionService, CryptionService>();
        services.AddTransient<IRequestClient, RequestClient>();
        services.AddTransient<IStorageManager, StorageManager>();
        services.AddSingleton<IStoreService, StoreService>();
        services.AddSingleton<IGitlabSecretsService, GitlabSecretsService>();
        
        services.RegisterInstallationServices();
        services.RegisterGitlabServices();
        services.RegisterGithubServices();
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
