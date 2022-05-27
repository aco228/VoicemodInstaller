using VoicemodPowertools.Services.Storage;

namespace VoicemodPowertools.Domain.Storage;

public interface IGeneralStorageData
{
    T? Get<T>();
    void Add<T>(T entry);
    void Print();
}

[Serializable]
public class GeneralStorageData : IGeneralStorageData
{
    public Dictionary<string, object> Data { get; set; } = new();

    public bool ContainsKey<T>() 
        => Data.ContainsKey(typeof(T).Name);

    public T? Get<T>()
    {
        if (!ContainsKey<T>())
            return default;

        return (T)Data[typeof(T).Name];
    }

    public void Add<T>(T entry)
    {
        var key = typeof(T).Name;
        if (Data.ContainsKey(key))
            Data[key] = entry;
        else
            Data.Add(key, entry);
    }

    public void Print()
    {
        Console.WriteLine("Storage data entries");
        foreach (var entry in Data)
        {
            Console.WriteLine($"\t{entry.Key}");
        }
    }
}