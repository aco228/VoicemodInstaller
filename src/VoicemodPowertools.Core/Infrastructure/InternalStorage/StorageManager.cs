using System.Runtime.Serialization.Formatters.Binary;
using VoicemodPowertools.Core.Domain.InternalStorage;
using VoicemodPowertools.Core.Infrastructure.Helpers;
using VoicemodPowertools.Core.Services.InternalStorage;

namespace VoicemodPowertools.Core.Infrastructure.InternalStorage;

public class StorageManager : IStorageManager
{
    private readonly ICryptionService _cryptionService;
    private readonly IZipStorage _zipStorage;
    
    public StorageManager(
        ICryptionService cryptionService,
        IZipStorage zipStorageStorage)
    {
        _cryptionService = cryptionService;
        _zipStorage = zipStorageStorage;
    }
    
    public void Write<T>(string zipFile, string filePath, T obj) where T : class
    {
        zipFile = zipFile.GetAbsolutPath();
        
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
        
        using var memoryStream = new MemoryStream();
        var binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(memoryStream, encFile);
        
        _zipStorage.Write(zipFile, filePath, memoryStream.ToArray());
    }

    public T? Read<T>(string zipFile, string filePath) where T : class
    {
        try
        {
            zipFile = zipFile.GetAbsolutPath();
            
            if (!File.Exists(zipFile))
                return null;
            
            var encFile = _zipStorage.ReadFromBinaryFile<WriteFileModel>(zipFile, filePath);
            if (encFile == null)
            {
                // ConsoleDebug.WriteLine("encFile is null");
                return default;
            }

            var publicKeySha = ProgramConstants.PublicKey.Base64Encode();
            var fileContent = _cryptionService.Decrypt(encFile.FileContent, publicKeySha);
            if (!encFile.Sha.Equals(fileContent.Base64Encode()))
            {
                Console.WriteLine($"Error reading {filePath}, enc");
                // ConsoleDebug.WriteLine("Enc sha does not match with content");
                return default;
            }

            var bytes = Convert.FromHexString(fileContent);
            var content = FromByteArray<T>(bytes);
            return content;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception reading file {filePath}");
            // ConsoleDebug.WriteLine(ex.ToString());
            return null;
        }
    }
    
    private T FromByteArray<T>(byte[] data)
    {
        if (data == null)
        {
            // ConsoleDebug.WriteLine("FromByteArray data is null");
            return default(T);   
        }
        
        var bf = new BinaryFormatter();
        using MemoryStream ms = new MemoryStream(data);
        var obj = bf.Deserialize(ms);
        return (T)obj;
    }
}