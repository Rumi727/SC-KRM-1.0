using SCKRM.Sound;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Kernel), true)]
    public class KernelEditor : CustomInspectorEditor
    {
        public override void OnInspectorGUI() => KernelWindowEditor.Default();
    }
}