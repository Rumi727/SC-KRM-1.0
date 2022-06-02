using SCKRM.Json;
using SCKRM.Language;
using SCKRM.Resource;
using SCKRM.Threads;
using System;
using UnityEngine;

namespace SCKRM.Renderer
{
    [AddComponentMenu("")]
    public abstract class CustomAllTextRenderer : CustomAllRenderer
    {
        public NameSpacePathReplacePair nameSpacePathReplacePair
        {
            get => new NameSpacePathReplacePair(nameSpace, path, replace);
            set
            {
                nameSpace = value.nameSpace;
                path = value.path;

                replace = value.replace;
            }
        }

        public ReplaceOldNewPair[] replace { get; set; } = new ReplaceOldNewPair[0];



        public string GetText()
        {
#if UNITY_EDITOR
            string text;
            if (!ThreadManager.isMainThread || Kernel.isPlaying)
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

    public struct NameSpacePathReplacePair
    {
        public string path;
        public string nameSpace;

        public ReplaceOldNewPair[] replace;

        public NameSpacePathReplacePair(string path)
        {
            nameSpace = "";
            this.path = path;

            replace = new ReplaceOldNewPair[0];
        }

        public NameSpacePathReplacePair(string nameSpace, string path)
        {
            this.nameSpace = nameSpace;
            this.path = path;

            replace = new ReplaceOldNewPair[0];
        }

        public NameSpacePathReplacePair(string nameSpace, string path, params ReplaceOldNewPair[] replace)
        {
            this.nameSpace = nameSpace;
            this.path = path;

            this.replace = replace;
        }

        public static implicit operator string(NameSpacePathReplacePair value) => value.ToString();

        public static implicit operator NameSpacePathReplacePair(string value)
        {
            string nameSpace = ResourceManager.GetNameSpace(value, out value);
            return new NameSpacePathReplacePair(nameSpace, value);
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(nameSpace))
                return path;
            else
                return nameSpace + ":" + path;
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