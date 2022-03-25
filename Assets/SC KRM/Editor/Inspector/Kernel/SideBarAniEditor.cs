using SCKRM.NBS;
using SCKRM.UI.SideBar;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SideBarAni), true)]
    public sealed class SideBarAniEditor : UIAniEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawLine();

            UseProperty("_showControlKey");
            UseProperty("_inputLockName");
        }
    }
}