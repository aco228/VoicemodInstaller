using VoicemodPowertools.Domain.Storage;

namespace VoicemodPowertools.Services.Storage;

public interface IGeneralStorageService
{
    IGeneralStorageData GetCurrent();
    T? Get<T>();
    void Save<T>(T enty);
}