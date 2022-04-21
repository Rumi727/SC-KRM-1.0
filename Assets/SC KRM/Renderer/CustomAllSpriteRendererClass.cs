using Cysharp.Threading.Tasks;
using SCKRM.Resource;
using SCKRM.Threads;
using SCKRM.Tool;
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

        /*public override void ResourceReload()
        {
            base.ResourceReload();
            queue.Enqueue();
        }*/

        public async UniTask<Sprite> SpriteReload(string type, string name, int index, string nameSpace = "")
        {
#if UNITY_EDITOR
            if (!ThreadManager.isMainThread || Application.isPlaying)
            {
                Sprite[] sprites = ResourceManager.SearchSprites(type, name, nameSpace);
                if (sprites != null && index < sprites.Length)
                    return sprites[index];

                return null;
            }
            else
#endif
            {
                Sprite[] sprites = await ResourceManager.GetSprites(Kernel.streamingAssetsPath, type, name, nameSpace, TextureFormat.DXT5);
                if (sprites != null && index < sprites.Length)
                    return sprites[index];
                else
                    return null;
            }
        }
    }

    public struct NameSpaceTypePathPair
    {
        public string type;
        public string path;
        public string nameSpace;

        public NameSpaceTypePathPair(string type, string path, string nameSpace = "")
        {
            this.type = type;
            this.path = path;
            this.nameSpace = nameSpace;
        }

        public static implicit operator string(NameSpaceTypePathPair value) => value.nameSpace + ":" + PathTool.Combine(value.type, value.path);

        public static implicit operator NameSpaceTypePathPair(string value)
        {
            string nameSpace = ResourceManager.GetNameSpace(value, out value);
            int index = value.LastIndexOf('/');
            return new NameSpaceTypePathPair(value.Remove(index), value.Substring(index + 1), nameSpace);
        }
    }
}