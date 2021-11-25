using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.Renderer
{
    [AddComponentMenu("커널/Renderer/UI/Image")]
    [RequireComponent(typeof(Image))]
    public class CustomImageRenderer : CustomAllSpriteRenderer
    {
        public Image image { get; private set; }

        public override void Rerender()
        {
            if (image == null)
                image = GetComponent<Image>();

            image.sprite = (Sprite)queue.Dequeue();
        }
    }
}