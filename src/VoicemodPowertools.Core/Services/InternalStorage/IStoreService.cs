using VoicemodPowertools.Core.Domain.InternalStorage;

namespace VoicemodPowertools.Core.Services.InternalStorage;

public interface IStoreService
{
    IGeneralStorageData GetCurrent();
    T? Get<T>();
    void Save<T>(T enty);
}