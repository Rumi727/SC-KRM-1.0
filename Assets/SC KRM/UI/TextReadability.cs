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
    public class TextReadability : UIBehaviour
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

        void Update()
        {
            if (targetCanvasRenderer != null && graphic != null && targetCanvasRenderer != graphic)
            {
                Color color = targetCanvasRenderer.GetColor();
                float average = (color.r + color.g + color.b) / 3;

                if (average <= 0.5f)
                    graphic.color = graphic.color.Lerp(Color.white, 0.2f * Kernel.fpsDeltaTime);
                else
                    graphic.color = graphic.color.Lerp(Color.black, 0.2f * Kernel.fpsDeltaTime);
            }
        }
    }
}