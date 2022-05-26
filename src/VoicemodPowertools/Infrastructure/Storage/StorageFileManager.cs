using System.Runtime.Serialization.Formatters.Binary;
using VoicemodPowertools.Domain;
using VoicemodPowertools.Domain.Storage;
using VoicemodPowertools.Infrastructure.Helpers;
using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Infrastructure.Storage;

public class StorageFileManager : IStorageFileManager
{
    private readonly ICryptionService _cryptionService;
    private readonly IZipStorage _zip;
    
    public StorageFileManager(
        ICryptionService cryptionService,
        IZipStorage zipStorage)
    {
        _cryptionService = cryptionService;
        _zip = zipStorage;
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
        
        // using Stream fileOpenStream = File.Open(filePath, FileMode.Create);
        using var memoryStream = new MemoryStream();
        var binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(memoryStream, encFile);
        
        _zip.Write(ProgramConstants.FileLocations.ZipFileName, filePath, memoryStream.ToArray());
    }

    public T? Read<T>(string filePath) where T : class
    {
        try
        {
            // if (!File.Exists(filePath))
            //     return null;
            
            var encFile = ReadFromBinaryFile<WriteFileModel>(filePath);
            if (encFile == null)
            {
                ConsoleDebug.WriteLine("encFile is null");
                return default;
            }

            var publicKeySha = ProgramConstants.PublicKey.Base64Encode();
            var fileContent = _cryptionService.Decrypt(encFile.FileContent, publicKeySha);
            if (!encFile.Sha.Equals(fileContent.Base64Encode()))
            {
                Console.WriteLine($"Error reading {filePath}, enc");
                ConsoleDebug.WriteLine("Enc sha does not match with content");
                return default;
            }

            var bytes = Convert.FromHexString(fileContent);
            var content = FromByteArray<T>(bytes);
            return content;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception reading file {filePath}");
            ConsoleDebug.WriteLine(ex.ToString());
            return null;
        }
    }
    
    private T FromByteArray<T>(byte[] data)
    {
        if (data == null)
        {
            ConsoleDebug.WriteLine("FromByteArray data is null");
            return default(T);   
        }
        
        var bf = new BinaryFormatter();
        using MemoryStream ms = new MemoryStream(data);
        var obj = bf.Deserialize(ms);
        return (T)obj;
    }
    
    private T ReadFromBinaryFile<T>(string filePath)
    {   
        var bytes = _zip.Read(ProgramConstants.FileLocations.ZipFileName, filePath);
        if (bytes == null || bytes.Length == 0)
        {
            ConsoleDebug.WriteLine($"zip bytes is null for {filePath}");
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