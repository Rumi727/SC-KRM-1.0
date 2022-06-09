using SCKRM.Object;
using SCKRM.Renderer;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI
{
    [AddComponentMenu("SC KRM/UI/Dropdown/Dropdown Item")]
    public class DropdownItem : UIObjectPooling
    {
        [SerializeField, NotNull] TMP_Text _label;
        public TMP_Text label { get => _label; }

        [SerializeField, NotNull] Toggle _toggle;
        public Toggle toggle { get => _toggle; }
    }
}