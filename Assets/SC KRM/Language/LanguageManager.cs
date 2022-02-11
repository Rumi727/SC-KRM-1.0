using Newtonsoft.Json;
using SCKRM.Json;
using SCKRM.Resource;
using SCKRM.SaveLoad;
using SCKRM.Tool;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Language
{
    public static class LanguageManager
    {
        [SaveLoad("language")]
        public sealed class SaveData
        {
            [JsonProperty] public static string currentLanguage { get; set; } = "en_us";
        }


        public static event Action currentLanguageChange;

        public static void LanguageChangeEventInvoke() => currentLanguageChange?.Invoke();

        /// <summary>
        /// 리소스팩에서 언어 파일을 가져온뒤, 키 값으로 텍스트를 찾고 반환합니다
        /// After importing the language file from the resource pack, it finds and returns the text as a key value.
        /// </summary>
        /// <param name="key">
        /// 키
        /// Key</param>
        /// <param name="nameSpace">
        /// 네임스페이스
        /// Name Space
        /// </param>
        /// <param name="language">
        /// 언어
        /// </param>
        /// <returns></returns>
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

            try
            {
                string value = JsonManager.JsonReadDictionary<string, string>(key, PathTool.Combine(ResourceManager.languagePath, language), nameSpace).ConstEnvironmentVariable();
                if (value == default)
                    return key;

                return value;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return key;
            }
        }
    }
}