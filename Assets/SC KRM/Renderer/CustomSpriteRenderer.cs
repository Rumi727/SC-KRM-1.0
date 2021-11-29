using UnityEngine;

namespace SCKRM.Renderer
{
    [AddComponentMenu("커널/Renderer/Sprite Renderer")]
    [RequireComponent(typeof(SpriteRenderer))]
    public class CustomSpriteRenderer : CustomAllSpriteRenderer
    {
        SpriteRenderer _spriteRenderer;
        public SpriteRenderer spriteRenderer
        {
            get
            {
                if (_spriteRenderer == null)
                    _spriteRenderer = GetComponent<SpriteRenderer>();

                return _spriteRenderer;
            }
        }

        [SerializeField] Vector2 _size = Vector2.zero;
        public Vector2 size { get => _size; set => _size = value; }

        [SerializeField] SpriteDrawMode _drawMode = SpriteDrawMode.Simple;
        public SpriteDrawMode drawMode { get => _drawMode; set => _drawMode = value; }

        public override void Rerender()
        {
            spriteRenderer.sprite = (Sprite)queue.Dequeue();
            spriteRenderer.drawMode = drawMode;
            spriteRenderer.size = size;
        }
    }
}