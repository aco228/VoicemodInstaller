namespace VoicemodPowertools.Infrastructure.Consoles;

public static class ConsoleArgumentsExtensions
{

    public static bool ValueExists(this string[] args, string search)
    {
        if (string.IsNullOrEmpty(search))
            return false;
        var searchparam = search.GetSearchParam();
        return args.GetAllParams().Any((x) => x.Item1.Trim().ToLower().Equals(searchparam));
    }

    public static IEnumerable<Tuple<string, string>> GetAllParams(this string[] args)
    {
        foreach (string arg in args)
        {
            var argSplit = arg.Split('=');
            var attrName = argSplit[0].GetSearchParam();
            var attrValue = argSplit.Length == 1 ? string.Empty : argSplit[1].Trim();

            yield return new(attrName, attrValue);
        }
    }
    
    private static string GetSearchParam(this string searchParam)
        => searchParam.Trim().ToLower().Replace("--", string.Empty);

    public static T GetValue<T>(this string[] args, string search, T defaultReturn = default) where T: IComparable
    {
        if (args.Length == 0)
            return defaultReturn;

        var searchParam = search.GetSearchParam();
        foreach (var (name, value) in args.GetAllParams())
        {
            if (!name.Equals(searchParam))
                continue;
            
            if (string.IsNullOrEmpty(value) && typeof(T) == typeof(bool))
                return (T) (object) true;

            if (string.IsNullOrEmpty(value))
                return defaultReturn;

            return (T) Convert.ChangeType(value, typeof(T));
        }
        
        return defaultReturn;
    }

    public static T GetValueAndRemove<T>(ref string[] args, string search, T defaultReturn = default) where T : IComparable
    {
        var result = args.GetValue(search, defaultReturn);
        args = args.ToList().Where(x => !x.Equals(search)).ToArray();
        return result;
    }
}