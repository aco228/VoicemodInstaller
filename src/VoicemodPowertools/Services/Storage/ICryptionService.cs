namespace VoicemodPowertools.Services.Storage;

public interface ICryptionService
{
    string Encrypt(string plainText, string passPhrase);
    string Decrypt(string cipherText, string passPhrase);
}