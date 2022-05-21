using VoicemodPowertools.Services.Http;

namespace VoicemodPowertools.Services.Gitlab;

public interface IGitlabHttpClient : IRequestClient
{
    void LoadToken();
}