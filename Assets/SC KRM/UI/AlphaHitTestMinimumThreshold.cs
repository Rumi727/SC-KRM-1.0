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
        [SerializeField] float _alphaHitTestMinimumThreshold = 0;

        protected override void Awake() => image.alphaHitTestMinimumThreshold = alphaHitTestMinimumThreshold;
    }
}
