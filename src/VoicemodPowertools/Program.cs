using System.Reflection;
using VoicemodPowertools.Application;
using VoicemodPowertools.Core.Domain.InternalStorage.Entries;
using VoicemodPowertools.Core.Infrastructure;
using VoicemodPowertools.Core.Infrastructure.Github;
using VoicemodPowertools.Core.Infrastructure.Gitlab;
using VoicemodPowertools.Core.Infrastructure.Http;
using VoicemodPowertools.Core.Infrastructure.InternalStorage;
using VoicemodPowertools.Core.Services.Gitlab;
using VoicemodPowertools.Core.Services.Http;
using VoicemodPowertools.Core.Services.InternalStorage;
using VoicemodPowertools.Domain.Installation;
using VoicemodPowertools.Domain.Storage;
using VoicemodPowertools.Infrastructure.Consoles;

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
        ProgramConstants.ProgramLocation = Location;
        
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Configuration.AddJsonFile(Path.Combine(_absoluthLocation, "appsettings.json"));
        RegisterServices(builder.Services);
        var app = builder.Build();

        RegisterSecrets(app.Environment.IsDevelopment(), app.Services);

        _isDebug = (!args.ValueExists(ProgramConstants.DebugAttribute) && app.Environment.IsDevelopment()) 
                   || ConsoleArgumentsExtensions.GetValueAndRemove(ref args, "debug", false);
        
        if (!args.GetValue(ProgramConstants.IgnoreAttribute, false))
        {
            new Thread(() => InitializeServer(app)).Start();
            // new Thread( () =>  app.Services.GetService<ICheckForNewRelease>().Run()).Start();
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
