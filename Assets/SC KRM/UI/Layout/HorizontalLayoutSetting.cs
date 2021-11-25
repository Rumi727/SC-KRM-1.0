using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.UI
{
    public class HorizontalLayoutSetting : MonoBehaviour
    {
        [SerializeField] Mode _mode = Mode.none;
        public Mode mode { get => _mode; }

        public enum Mode
        {
            none,
            center,
            right
        }
    }
}