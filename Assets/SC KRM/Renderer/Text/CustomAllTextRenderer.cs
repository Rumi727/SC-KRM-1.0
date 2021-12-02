using SCKRM.Language;
using UnityEngine;

namespace SCKRM.Renderer
{
    [AddComponentMenu("")]
    public class CustomAllTextRenderer : CustomAllRenderer
    {
        public override void ResourceReload()
        {
            base.ResourceReload();
            string text = LanguageManager.TextLoad(path, nameSpace);
            if (text != "")
                queue.Enqueue(text);
            else
                queue.Enqueue(path);
        }
    }
}