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

        public override void Rerender()
        {
            while (queue.TryDequeue(out object sprite))
                image.sprite = (Sprite)sprite;
        }
    }
}