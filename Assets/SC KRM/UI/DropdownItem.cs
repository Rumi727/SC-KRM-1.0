using SCKRM.Renderer;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI
{
    public class DropdownItem : MonoBehaviour
    {
        [SerializeField] CustomAllRenderer[] _renderers;
        public CustomAllRenderer[] renderers { get => _renderers; }

        [SerializeField] TMP_Text _label;
        public TMP_Text label { get => _label; }

        [SerializeField] Toggle _toggle;
        public Toggle toggle { get => _toggle; }
    }
}