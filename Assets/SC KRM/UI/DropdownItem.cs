using SCKRM.Renderer;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI
{
    [AddComponentMenu("커널/UI/드롭다운 아이템")]
    public class DropdownItem : ObjectPoolingUI
    {
        [SerializeField] TMP_Text _label;
        public TMP_Text label { get => _label; }

        [SerializeField] Toggle _toggle;
        public Toggle toggle { get => _toggle; }
    }
}