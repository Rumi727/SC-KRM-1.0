using SCKRM.SaveLoad;
using Newtonsoft.Json;
using SCKRM.ProjectSetting;

namespace SCKRM.Splash
{
    [WikiDescription("스플래시 스크린을 관리하는 클래스 입니다")]
    public static class SplashScreen
    {
        [ProjectSettingSaveLoad]
        public sealed class Data
        {
            [JsonProperty] public static string splashScenePath { get; set; } = "Assets/SC KRM/Splash Screen/Splash Screen.unity";
            [JsonProperty] public static string sceneLoadingScenePath { get; set; } = "Assets/SC KRM/Scene/Scene Load Scene.unity";

            [JsonProperty] public static string kernelPrefabPath { get; set; } = "Assets/SC KRM/Kernel.prefab";
        }

        [GeneralSaveLoad]
        public sealed class SaveData
        {
            [JsonProperty] public static bool allowProgressBarShow { get; set; } = false;
        }

        [WikiDescription("스플래시 스크린이 재생 중인지 여부")]
        public static bool isAniPlaying { get; set; } = true;
    }
}