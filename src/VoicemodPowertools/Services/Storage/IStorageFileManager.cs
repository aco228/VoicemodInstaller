namespace VoicemodPowertools.Services.Storage;

public interface IStorageFileManager
{
    void Write<T>(string zipFile, string filePath, T obj) where T : class;
    T? Read<T>(string zipFile, string filePath) where T : class;
}