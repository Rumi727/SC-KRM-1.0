using SCKRM.Json;
using SCKRM.Language;
using SCKRM.Resource;
using SCKRM.Threads;
using System;
using UnityEngine;

namespace SCKRM.Renderer
{
    [AddComponentMenu("")]
    public class CustomAllTextRenderer : CustomAllRenderer
    {
        public ReplaceOldNewPair[] replace { get; set; } = new ReplaceOldNewPair[0];



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
            if (replace != null)
            {
                for (int i = 0; i < replace.Length; i++)
                    text = text.Replace(replace[i].replaceOld, replace[i].replaceNew);
            }

            if (text != "")
                return text;
            else
                return path;
        }
    }

    public struct ReplaceOldNewPair
    {
        public string replaceOld;
        public string replaceNew;

        public ReplaceOldNewPair(string replaceOld, string replaceNew)
        {
            this.replaceOld = replaceOld;
            this.replaceNew = replaceNew;
        }
    }
}