using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.UI
{
    [AddComponentMenu("커널/UI/Layout/수평 레이아웃 설정")]
    public sealed class HorizontalLayoutSetting : UI
    {
        [SerializeField] bool _custom = false;
        public bool custom => _custom;

        [SerializeField] Mode _mode = Mode.none;
        public Mode mode => _mode;

        public enum Mode
        {
            none,
            center,
            right
        }
    }
}