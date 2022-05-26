namespace VoicemodPowertools.Services.Storage;

public interface IZipStorage
{
    byte[]? Read(string zipFile, string file);
    void Write(string zipFile, string file, byte[] data);
}