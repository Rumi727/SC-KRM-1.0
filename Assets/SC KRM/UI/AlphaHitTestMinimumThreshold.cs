using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI
{
    [ExecuteAlways, RequireComponent(typeof(Image))]
    public sealed class AlphaHitTestMinimumThreshold : UIBase
    {
        public Image image => _image = this.GetComponentFieldSave(_image); [SerializeField] Image _image;

        public float alphaHitTestMinimumThreshold
        {
            get => _alphaHitTestMinimumThreshold;
            set
            {
                _alphaHitTestMinimumThreshold = value;
                image.alphaHitTestMinimumThreshold = value;
            }
        }
        [SerializeField, Range(0, 1)] float _alphaHitTestMinimumThreshold = 0.5f;

        protected override void Awake() => image.alphaHitTestMinimumThreshold = alphaHitTestMinimumThreshold;

        protected override void OnDestroy() => image.alphaHitTestMinimumThreshold = 0;
    }
}
