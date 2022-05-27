using VoicemodPowertools.Domain;
using VoicemodPowertools.Domain.Storage;
using VoicemodPowertools.Services.InternalStorage;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Infrastructure.Storage;

public class StoreService : IStoreService
{
    private readonly IStorageManager _storageManager;
    private IGeneralStorageData _data = null;
    
    public StoreService(
        IStorageManager storageManager)
    {
        _storageManager = storageManager;
    }

    public IGeneralStorageData GetCurrent()
    {
        if (_data != null)
            return _data;

        _data = _storageManager.Read<GeneralStorageData>(
                    ProgramConstants.File.General.Zip,
                    ProgramConstants.File.General.GeneralStorageFile) 
                ?? new GeneralStorageData();
        return _data;
    }
    
    public T? Get<T>() 
    {
        var storage = GetCurrent();
        return storage.Get<T>();
    }
    
    public void Save<T>(T enty)
    {
        var storage = GetCurrent();
        storage.Add<T>(enty);
        _storageManager.Write(
            ProgramConstants.File.General.Zip,
            ProgramConstants.File.General.GeneralStorageFile, storage);
    }
    
}