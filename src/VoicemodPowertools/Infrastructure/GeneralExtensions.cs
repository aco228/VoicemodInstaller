using System.Text.RegularExpressions;
using VoicemodPowertools.Domain;

namespace VoicemodPowertools.Infrastructure;

public static class GeneralExtensions
{
    public static string GetVersion(this string? input)
    {
        if (string.IsNullOrEmpty(input)) 
            return string.Empty;
        
        var rgx = new Regex(@"v[1-9]+\.[0-9]+\.[0-9]+");
        var match = rgx.Match(input);

        return !match.Success
            ? string.Empty
            : match.Value;
    }

    public static string GetAbsolutPath(this string path)
    {
        if (path.Contains(Program.Location))
            return path;
        return Path.Combine(Program.Location, path);
    }
}