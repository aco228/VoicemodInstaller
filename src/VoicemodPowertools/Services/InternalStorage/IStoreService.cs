using VoicemodPowertools.Domain.Storage;

namespace VoicemodPowertools.Services.Storage;

public interface IStoreService
{
    IGeneralStorageData GetCurrent();
    T? Get<T>();
    void Save<T>(T enty);
}