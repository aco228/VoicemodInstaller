using System.Runtime.Serialization.Formatters.Binary;
using VoicemodPowertools.Domain;
using VoicemodPowertools.Domain.Storage;
using VoicemodPowertools.Infrastructure.Helpers;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Infrastructure.Storage;

public class StorageFileManager : IStorageFileManager
{
    private readonly ICryptionService _cryptionService;
    
    public StorageFileManager(ICryptionService cryptionService)
    {
        _cryptionService = cryptionService;
    }
    
    public void Write<T>(string filePath, T obj) where T : class
    {
        var formatter = new BinaryFormatter();
        using var stream = new MemoryStream();
        formatter.Serialize(stream, obj);
        
        var array =  stream.ToArray();
        var hexContent = Convert.ToHexString(array);
        var sha = hexContent.Base64Encode();

        var publicKeySha = ProgramConstants.PublicKey.Base64Encode();
        var content = _cryptionService.Encrypt(hexContent, publicKeySha);
        var encFile = new WriteFileModel
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
            
            var encFile = ReadFromBinaryFile<WriteFileModel>(filePath);

            var publicKeySha = ProgramConstants.PublicKey.Base64Encode();
            var fileContent = _cryptionService.Decrypt(encFile.FileContent, publicKeySha);
            if (!encFile.Sha.Equals(fileContent.Base64Encode()))
            {
                Console.WriteLine($"Error reading {filePath}, enc");
                return default;
            }

            var bytes = Convert.FromHexString(fileContent);
            var content = FromByteArray<T>(bytes);
            return content;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception reading file {filePath}");
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