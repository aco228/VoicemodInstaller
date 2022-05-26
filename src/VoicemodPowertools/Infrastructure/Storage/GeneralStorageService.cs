using VoicemodPowertools.Domain;
using VoicemodPowertools.Domain.Storage;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Infrastructure.Storage;

public class GeneralStorageService : IGeneralStorageService
{
    private readonly IStorageFileManager _fileManager;
    private IGeneralStorageData _data = null;
    
    public GeneralStorageService(
        IStorageFileManager fileManager)
    {
        _fileManager = fileManager;
    }

    public IGeneralStorageData GetCurrent()
    {
        if (_data != null)
            return _data;

        _data = _fileManager.Read<GeneralStorageData>(
                    ProgramConstants.FileLocations.Zip.General,
                    ProgramConstants.FileLocations.GeneralStorageFile) 
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
        _fileManager.Write(
            ProgramConstants.FileLocations.Zip.General,
            ProgramConstants.FileLocations.GeneralStorageFile, storage);
    }
    
}