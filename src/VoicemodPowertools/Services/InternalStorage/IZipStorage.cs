namespace VoicemodPowertools.Services.InternalStorage;

public interface IZipStorage
{
    byte[]? Read(string zipFile, string file);
    void Write(string zipFile, string file, byte[] data);
    T ReadFromBinaryFile<T>(string zipFile, string filePath) where T : class;
}