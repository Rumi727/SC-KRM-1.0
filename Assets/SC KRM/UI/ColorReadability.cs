using System.Diagnostics;
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
    [AddComponentMenu("커널/UI/색상 가독성 향상")]
    public class ColorReadability : UIAni
    {
        [SerializeField] CanvasRenderer _targetCanvasRenderer; public CanvasRenderer targetCanvasRenderer => _targetCanvasRenderer;
        [SerializeField] Graphic _targetGraphic; public Graphic targetGraphic => _targetGraphic;

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

        Color color = Color.white;
        protected override void LayoutRefresh()
        {
            if (targetCanvasRenderer != null && targetGraphic != null && graphic != null && targetCanvasRenderer != graphic)
                color = GetReadbilityColor(targetGraphic.color * targetCanvasRenderer.GetColor());
        }

        protected override void SizeUpdate(bool onEnable = false)
        {
#if UNITY_EDITOR
            if (!lerp || !Application.isPlaying || onEnable)
#else
            if (!lerp || onEnable)
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