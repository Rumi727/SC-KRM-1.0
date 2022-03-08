using HSVPicker;
using SCKRM;
using SCKRM.Tool;
using SCKRM.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HSVPicker
{
    public class SVBoxSliderHandleColor : UIAni
    {
        public Graphic graphic;
        public ColorPicker colorPicker;
        public Type type;

        protected override void OnEnable()
        {
            colorPicker?.onValueChanged.AddListener(OnValueChanged);
            OnValueChanged(colorPicker.CurrentColor);
        }

        protected override void OnDisable() => colorPicker?.onValueChanged.RemoveListener(OnValueChanged);

        Color color = Color.white;
        public void OnValueChanged(Color color)
        {
            if (graphic != null)
            {
                if (type == Type.a)
                    this.color = ColorReadability.GetReadbilityColor(color.a);
                else if (type == Type.h)
                    this.color = ColorReadability.GetReadbilityColor(HSVUtil.ConvertHsvToRgb(colorPicker.H * 360, 1, 1, 1));
                else
                    this.color = ColorReadability.GetReadbilityColor(color);
            }
        }



        protected override void SizeUpdate()
        {
#if UNITY_EDITOR
            if (!lerp || !Application.isPlaying)
#else
            if (!lerp)
#endif
                graphic.color = color;
            else
                graphic.color = graphic.color.Lerp(color, lerpValue * Kernel.fpsUnscaledDeltaTime);
        }

        public enum Type
        {
            all,
            a,
            h
        }
    }
}