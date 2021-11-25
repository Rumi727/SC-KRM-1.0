using SCKRM.Resource;
using SCKRM.Threads;
using UnityEngine;

namespace SCKRM.Renderer
{
    [AddComponentMenu("")]
    public class CustomAllSpriteRenderer : CustomAllRenderer
    {
        [SerializeField] string _type = "";
        public string type { get => _type; set => _type = value; }

        [SerializeField, Min(0)] int _index = 0;
        public int index { get => _index; set => _index = value; }

        /*protected virtual void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying && UnityEditor.Selection.activeObject != null)
            {
                if (UnityEditor.Selection.activeGameObject.GetComponentInParent<CustomAllRenderer>())
                    
            }
#endif
        }*/

        public override void ResourceReload()
        {
            base.ResourceReload();
            queue.Enqueue(SpriteReload(type, path, index, nameSpace));
        }

        public Sprite SpriteReload(string type, string name, int index, string nameSpace = "")
        {
#if !UNITY_EDITOR
            Sprite[] sprites = ResourceManager.SearchSprites(type, path, nameSpace);
            if (sprites != null && index < sprites.Length)
                return sprites[index];

            return null;
#else
            if (!ThreadManager.isMainThread || Application.isPlaying)
            {
                Sprite[] sprites = ResourceManager.SearchSprites(type, path, nameSpace);
                if (sprites != null && index < sprites.Length)
                    return sprites[index];

                return null;
            }
            else
            {
                Sprite[] sprites = ResourceManager.GetSprites(Kernel.streamingAssetsPath, type, path, nameSpace, TextureFormat.DXT5);
                if (sprites != null && index < sprites.Length)
                    return sprites[index];
                else
                    return null;
            }
#endif
        }
    }
}