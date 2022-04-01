using K4.Threading;
using SCKRM.Threads;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.Renderer
{
    [AddComponentMenu("커널/Renderer/UI/Image")]
    [RequireComponent(typeof(Image))]
    public class CustomImageRenderer : CustomAllSpriteRenderer
    {
        [SerializeField, HideInInspector] Image _image;
        public Image image
        {
            get
            {
                if (_image == null)
                    _image = GetComponent<Image>();

                return _image;
            }
        }

        public override async void Refresh()
        {
            Sprite sprite = await SpriteReload(type, path, index, nameSpace);

            if (ThreadManager.isMainThread)
                image.sprite = sprite;
            else
                await K4UnityThreadDispatcher.Execute(() => image.sprite = sprite);
        }
    }
}