namespace VoicemodPowertools.Services.Storage;

public interface IStorageFileManager
{
    void Write<T>(string filePath, T obj) where T : class;
    T? Read<T>(string filePath) where T : class;
}