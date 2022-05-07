using Cysharp.Threading.Tasks;
using SCKRM.Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SCKRM.UI;
using TMPro;
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