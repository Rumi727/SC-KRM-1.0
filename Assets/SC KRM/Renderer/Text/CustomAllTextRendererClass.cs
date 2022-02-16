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

        public override void ResourceReload()
        {
            base.ResourceReload();

#if UNITY_EDITOR
            string text;
            if (!ThreadManager.isMainThread || Application.isPlaying)
                text = ResourceManager.SearchLanguage(path, nameSpace);
            else
            {
                try
                {
                    string value = JsonManager.JsonReadDictionary<string, string>(path, PathTool.Combine(ResourceManager.languagePath, LanguageManager.SaveData.currentLanguage), nameSpace).ConstEnvironmentVariable();
                    if (value == default)
                        text = path;
                    else
                        text = value;
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    text = path;
                }
            }
#else
            string text = ResourceManager.SearchLanguage(path, nameSpace);
#endif
            if (replaceOld != null && replaceNew != null && replaceOld.Length == replaceNew.Length)
            {
                for (int i = 0; i < replaceOld.Length; i++)
                    text = text.Replace(replaceOld[i], replaceNew[i]);
            }

            if (text != "")
                queue.Enqueue(text);
            else
                queue.Enqueue(path);
        }
    }
}