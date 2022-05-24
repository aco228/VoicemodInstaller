namespace VoicemodPowertools.Infrastructure;

public static class IAsyncEnumerableExtensions
{
    public static async Task<T> FirstOrDefault<T>(this IAsyncEnumerable<T> enumerable)
    {
        var list = await enumerable.ToList();
        return list.FirstOrDefault();
    }
    
    public static async Task<List<T>> ToList<T>(this IAsyncEnumerable<T> enumerable)
    {
        var result = new List<T>();
        await foreach(var entity in enumerable)
            result.Add(entity);
        return result;
    }
}