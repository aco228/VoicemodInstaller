using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using VoicemodPowertools.Core.Services.InternalStorage;

namespace VoicemodPowertools.Core.Infrastructure.InternalStorage;

public class ZipStorage : IZipStorage
{
    public byte[]? Read(string zipFile, string file)
    {
        if (!File.Exists(zipFile))
        {
            // ConsoleDebug.WriteLine($"Zip file does not exists {zipFile}");
            return null;   
        }

        using var zip = ZipFile.Open(zipFile, ZipArchiveMode.Read);
        var insideFile = zip.Entries.FirstOrDefault(x => x.Name.Equals(file));
        if (insideFile == null)
        {   
            // ConsoleDebug.WriteLine($"InsideFile {file} does not exists");
            return null;   
        }

        var stream = insideFile.Open();
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        var result =  memoryStream.ToArray();
        stream.Close();
        return result;
    }

    public void Write(string zipFile, string file, byte[] data)
    {
        var mode = File.Exists(zipFile)
            ? ZipArchiveMode.Update
            : ZipArchiveMode.Create;
        
        using var zip = ZipFile.Open(zipFile, mode);

        var entry = mode == ZipArchiveMode.Update
            ? zip.Entries.FirstOrDefault(x => x.Name.Equals(file))
            : null;

        if (entry == null)
            entry = zip.CreateEntry(file);
        
        using var entryStream = entry.Open();
        entryStream.Write(data, 0, data.Length);
    }

    public T ReadFromBinaryFile<T>(string zipFile, string filePath) where T : class
    {
        var bytes = Read(zipFile, filePath);
        if (bytes == null || bytes.Length == 0)
        {
            // ConsoleDebug.WriteLine($"zip bytes is null for {filePath}");
            return default;
        }

        try
        {
            using var stream = new MemoryStream(bytes);
            stream.Seek(0, SeekOrigin.Begin);
            var binaryFormatter = new BinaryFormatter();
            return (T)binaryFormatter.Deserialize(stream);
        }
        catch(Exception e)
        {
            throw e;
        }
    }
}