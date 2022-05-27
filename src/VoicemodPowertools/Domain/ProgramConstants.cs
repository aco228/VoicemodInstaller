namespace VoicemodPowertools.Domain;

public static class ProgramConstants
{
    public static string DownloadsFolderName = "Downloads";
    public static string IgnoreAttribute = "ignore-sec";
    public static string PublicKey = "SKkWtfPUb3aq3Sej";
    public static string NameOfAutoInstallBat = "auto-update.cmd";

    public static class File
    {
        public static class App
        {
            public static string Zip = "app.rg";
            public static string GitlabSecretsFile = "state-gl.rg";
            public static string ApplicationSecretsFile = "state-app.rg";
        }
        
        public static class General
        {
            public static string Zip = "general.rg";
            public static string GeneralStorageFile = "state-gs.rg";
        }
    }

    public static class AutoUpdate
    {
        public static string NameOfTheFile = "_current";
    }
}