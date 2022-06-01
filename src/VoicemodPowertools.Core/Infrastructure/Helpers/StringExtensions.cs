using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace VoicemodPowertools.Core.Infrastructure.Helpers;

public static class StringExtensions
{
    public static string Base64Encode(this string input)
    {
        var crypt = new SHA256Managed();
        var hash = crypt.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Base64UrlTextEncoder.Encode(hash);
    }
}