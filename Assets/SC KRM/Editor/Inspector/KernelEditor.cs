using SCKRM.Sound;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Kernel), true)]
    public sealed class KernelEditor : CustomInspectorEditor
    {
        public override void OnInspectorGUI() => KernelWindowEditor.Default();
    }
}