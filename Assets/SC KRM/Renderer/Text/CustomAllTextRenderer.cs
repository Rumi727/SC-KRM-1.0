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
            queue.Enqueue(LanguageManager.TextLoad(path, nameSpace));
        }
    }
}