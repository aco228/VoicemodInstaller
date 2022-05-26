﻿namespace VoicemodPowertools.Domain;

public static class ProgramConstants
{
    public static string DownloadsFolderName = "Downloads";
    public static string IgnoreAttribute = "ignore-sec";
    public static string PublicKey = "SKkWtfPUb3aq3Sej";

    public static class FileLocations
    {
        public static class Zip
        {
            public static string General = "general.rg";
            public static string Application = "app.rg";
        }
        
        public static string GeneralStorageFile = "state-gs.rg";
        public static string GitlabSecretsFile = "state-gl.rg";
        public static string ApplicationSecretsFile = "state-app.rg";
    }
}