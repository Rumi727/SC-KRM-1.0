using UnityEngine;
using SCKRM.SaveLoad;
using Newtonsoft.Json;
using SCKRM.ProjectSetting;

namespace SCKRM.Splash
{
    public static class SplashScreen
    {
        [ProjectSettingSaveLoad]
        public sealed class Data
        {
            [JsonProperty] public static string splashScreenPath { get; set; } = "Assets/SC KRM/Splash Screen";
            [JsonProperty] public static string splashScreenName { get; set; } = "Splash Screen";
        }

        [GeneralSaveLoad]
        public sealed class SaveData
        {
            [JsonProperty] public static bool allowProgressBarShow { get; set; } = false;
        }

        public static bool isAniPlayed { get; set; } = true;
    }
}