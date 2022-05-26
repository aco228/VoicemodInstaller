using System.IO.Compression;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Infrastructure.Storage;

public class ZipStorage : IZipStorage
{
    public byte[]? Read(string zipFile, string file)
    {
        if (!File.Exists(zipFile))
        {
            ConsoleDebug.WriteLine($"Zip file does not exists {zipFile}");
            return null;   
        }

        using var zip = ZipFile.Open(zipFile, ZipArchiveMode.Read);
        var insideFile = zip.Entries.FirstOrDefault(x => x.Name.Equals(file));
        if (insideFile == null)
        {   
            ConsoleDebug.WriteLine($"InsideFile {file} does not exists");
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
}