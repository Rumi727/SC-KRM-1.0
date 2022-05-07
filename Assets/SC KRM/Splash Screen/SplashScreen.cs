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

namespace SCKRM.Splash
{
    [AddComponentMenu("커널/스플래시/스플래시 스크린")]
    public static class SplashScreen
    {
        [GeneralSaveLoad]
        public sealed class SaveData
        {
            [JsonProperty] public static bool allowProgressBarShow { get; set; } = false;
        }

        public static bool isAniPlayed { get; set; } = true;
    }
}