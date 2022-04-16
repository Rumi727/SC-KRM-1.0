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
        [SerializeField] Color _offset = Color.white; public Color offset { get => _offset; set => _offset = value; }

        void Update() => graphic.color = Kernel.SaveData.systemColor * offset;
    }
}
