using Installer.Infrastructure.Http;

namespace Installer.Services.Gitlab;

public interface IGitlabHttpClient : IRequestClient
{
    void LoadToken();
}