using System.Runtime.Serialization.Formatters.Binary;
using VoicemodPowertools.Domain.Storage.Encryption;
using VoicemodPowertools.Infrastructure.Helpers;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Infrastructure.Storage;

public class StorageFileManager : IStorageFileManager
{
    public void Write<T>(string filePath, T obj) where T : class
    {
        var formatter = new BinaryFormatter();
        using var stream = new MemoryStream();
        formatter.Serialize(stream, obj);
        
        var array =  stream.ToArray();
        var content = Convert.ToHexString(array);
        var sha = content.Base64Encode();
        var encFile = new EncryptionFile
        {
            Sha = sha,
            FileContent = content
        };
        
        using Stream fileOpenStream = File.Open(filePath, FileMode.Create); 
        var binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileOpenStream, encFile);
    }

    public T? Read<T>(string filePath) where T : class
    {
        try
        {
            if (!File.Exists(filePath))
                return null;
            
            var encFile = ReadFromBinaryFile<EncryptionFile>(filePath);
            if (!encFile.Sha.Equals(encFile.FileContent.Base64Encode()))
            {
                Console.WriteLine($"Error reading {filePath}, enc");
                return default;
            }

            var bytes = Convert.FromHexString(encFile.FileContent);
            var content = FromByteArray<T>(bytes);
            return content;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception reading file {filePath}, {ex}");
            return null;
        }
    }
    
    private T FromByteArray<T>(byte[] data)
    {
        if (data == null)
            return default(T);
        
        var bf = new BinaryFormatter();
        
        using MemoryStream ms = new MemoryStream(data);
        object obj = bf.Deserialize(ms);
        return (T)obj;
    }
    
    private T ReadFromBinaryFile<T>(string filePath)
    {
        using Stream stream = File.Open(filePath, FileMode.Open);
        try
        {
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