using SCKRM.Json;
using SCKRM.Language;
using SCKRM.Resource;
using SCKRM.Threads;
using SCKRM.Tool;
using System;
using UnityEngine;

namespace SCKRM.Renderer
{
    [AddComponentMenu("")]
    public class CustomAllTextRenderer : CustomAllRenderer
    {
        public string[] replaceOld { get; set; }
        public string[] replaceNew { get; set; }



        public string GetText()
        {
#if UNITY_EDITOR
            string text;
            if (!ThreadManager.isMainThread || Application.isPlaying)
                text = ResourceManager.SearchLanguage(path, nameSpace);
            else
                text = LanguageManager.LanguageLoad(path, nameSpace, "en_us");
#else
            string text = ResourceManager.SearchLanguage(path, nameSpace);
#endif
            if (replaceOld != null && replaceNew != null && replaceOld.Length == replaceNew.Length)
            {
                for (int i = 0; i < replaceOld.Length; i++)
                    text = text.Replace(replaceOld[i], replaceNew[i]);
            }

            if (text != "")
                return text;
            else
                return path;
        }
    }
}