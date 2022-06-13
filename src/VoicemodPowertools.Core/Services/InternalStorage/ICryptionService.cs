namespace VoicemodPowertools.Core.Services.InternalStorage;

public interface ICryptionService
{
    string Encrypt(string plainText, string passPhrase);
    string Decrypt(string cipherText, string passPhrase);
}