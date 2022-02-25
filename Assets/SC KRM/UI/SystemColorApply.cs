using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI
{
    [AddComponentMenu("커널/UI/시스템 컬러 적용")]
    public class SystemColorApply : UI
    {
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
        [SerializeField] Color _offset = Color.white; public Color offset { get => _offset; set => _offset = value; }

        void Update() => graphic.color = Kernel.SaveData.systemColor * offset;
    }
}
