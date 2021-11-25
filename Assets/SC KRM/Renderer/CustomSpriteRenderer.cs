using UnityEngine;

namespace SCKRM.Renderer
{
    [AddComponentMenu("커널/Renderer/Sprite Renderer")]
    [RequireComponent(typeof(SpriteRenderer))]
    public class CustomSpriteRenderer : CustomAllSpriteRenderer
    {
        public SpriteRenderer spriteRenderer { get; private set; }

        [SerializeField] Vector2 _size = Vector2.zero;
        public Vector2 size { get => _size; set => _size = value; }

        [SerializeField] SpriteDrawMode _drawMode = SpriteDrawMode.Simple;
        public SpriteDrawMode drawMode { get => _drawMode; set => _drawMode = value; }

        public override void Rerender()
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();

            spriteRenderer.sprite = (Sprite)queue.Dequeue();
            spriteRenderer.drawMode = drawMode;
            spriteRenderer.size = size;
        }
    }
}