namespace VoicemodPowertools.Domain;

public static class ProgramConstants
{
    public static string DownloadsFolderName = "Downloads";
    public static string IgnoreAttribute = "ignore-sec";

    public static class FileLocations
    {
        public static string GeneralStorageFile = "state-gs.rg";
        public static string GitlabSecretsFile = "state-gl.rg";
        public static string ApplicationSecretsFile = "state-app.rg";
    }
}