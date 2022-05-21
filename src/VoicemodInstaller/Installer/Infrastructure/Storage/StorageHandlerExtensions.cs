using System.Runtime.Serialization.Formatters.Binary;
using Installer.Domain.Storage;
using Installer.Domain.Storage.Encryption;
using Installer.Infrastructure.Helpers;

namespace Installer.Infrastructure.Storage;

public static class StorageHandlerExtensions
{
    public static void WriteFile<T>(string filePath, T obj) where T : class
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
        WriteToBinaryFile(filePath, encFile);
    }

    public static T ReadFile<T>(string filePath) where T : class
    {
        try
        {
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
    
    public static T FromByteArray<T>(byte[] data)
    {
        if (data == null)
            return default(T);
        
        var bf = new BinaryFormatter();
        
        using MemoryStream ms = new MemoryStream(data);
        object obj = bf.Deserialize(ms);
        return (T)obj;
    }
    
    private static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
    {
        using Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create);
        try
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            binaryFormatter.Serialize(stream, objectToWrite);
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    private static T ReadFromBinaryFile<T>(string filePath)
    {
        using Stream stream = File.Open(filePath, FileMode.Open);
        try
        {
            stream.Seek(0, SeekOrigin.Begin);
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            return (T)binaryFormatter.Deserialize(stream);
        }
        catch(Exception e)
        {
            throw e;
        }
    }
}