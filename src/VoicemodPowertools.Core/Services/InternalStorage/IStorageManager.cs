namespace VoicemodPowertools.Core.Services.InternalStorage;

public interface IStorageManager
{
    void Write<T>(string zipFile, string fileName, T obj) where T : class;
    T? Read<T>(string zipFile, string fileName) where T : class;
}