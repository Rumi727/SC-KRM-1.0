using SCKRM.Language;
using SCKRM.Resource;
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

            string text = ResourceManager.SearchLanguage(path, nameSpace);
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