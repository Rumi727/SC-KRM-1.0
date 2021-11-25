using Newtonsoft.Json;
using SCKRM.Json;
using SCKRM.Resource;
using SCKRM.SaveLoad;
using System;
using System.Collections.Generic;

namespace SCKRM.Language
{
    public static class LanguageManager
    {
        [SaveLoad("Language")]
        public sealed class SaveData
        {
            [JsonProperty] public static string currentLanguage { get; set; } = "en_us";
        }


        public static event Action currentLanguageChange;

        public static void LanguageChangeEventInvoke() => currentLanguageChange();

        public static string TextLoad(string key, string nameSpace = "", string language = "")
        {
            if (key == null)
                key = "";
            if (language == null)
                language = "";

            if (nameSpace == "")
                nameSpace = ResourceManager.defaultNameSpace;
            if (language == "")
                language = SaveData.currentLanguage;

            Dictionary<string, string> jObject = JsonManager.JsonRead<Dictionary<string, string>>(KernelMethod.PathCombine(ResourceManager.languagePath, language), nameSpace);
            if (jObject != null && jObject.ContainsKey(key))
                return jObject[key].EnvironmentVariable();
            else
                return "";
        }
    }
}