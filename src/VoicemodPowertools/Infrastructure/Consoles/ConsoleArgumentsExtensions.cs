﻿namespace VoicemodPowertools.Infrastructure.Consoles;

public static class ConsoleArgumentsExtensions
{
    public static T GetValue<T>(this string[] args, string search, T defaultReturn = default) where T: IComparable
    {
        if (args.Length == 0)
            return defaultReturn;

        var searchParam = search.Trim().ToLower().Replace("--", string.Empty);
        foreach (string arg in args)
        {
            var argSplit = arg.Split('=');
            
            if (argSplit.Length == 1 && typeof(T) == typeof(bool)) 
                return (T)(object)true;
            
            if(argSplit.Length != 2) continue;
            var argName = argSplit[0].Trim().ToLower().Replace("--", string.Empty);
            
            if (!argName.Equals(searchParam)) continue;

            return (T)Convert.ChangeType(argSplit[1].Trim(), typeof(T));

        }

        return defaultReturn;
    }
}