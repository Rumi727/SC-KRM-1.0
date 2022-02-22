using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SCKRM.Tool;

namespace SCKRM.UI
{
    [ExecuteAlways]
    public class ColorReadability : UIAni
    {
        [SerializeField] CanvasRenderer _targetCanvasRenderer; public CanvasRenderer targetCanvasRenderer => _targetCanvasRenderer;
        [NonSerialized] Graphic _graphic; public Graphic graphic
        {
            get
            {
                if (_graphic == null)
                {
                    _graphic = GetComponent<Graphic>();
                    if (_graphic == null)
                        _graphic = gameObject.AddComponent<Graphic>();
                }

                return _graphic;
            }
        }

        Color color;
        protected override void LayoutRefresh()
        {
            if (targetCanvasRenderer != null && graphic != null && targetCanvasRenderer != graphic)
                color = GetReadbilityColor(targetCanvasRenderer.GetColor());
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

        public static Color GetReadbilityColor(float color)
        {
            if (color <= 0.5f)
                return Color.white;
            else
                return Color.black;
        }

        public static Color GetReadbilityColor(Color color)
        {
            float average = (color.r + color.g + color.b) / 3;

            if (average <= 0.5f)
                return Color.white;
            else
                return Color.black;
        }
    }
}