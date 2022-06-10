using SCKRM.UI;
using SCKRM.UI.Layout;
using UnityEditor;
using UnityEngine;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GraphicBeatColor))]
    public class GraphicBeatColorEditor : UIEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawLine();

            UseProperty("_alpha");

            if (Kernel.isPlaying)
                GUI.enabled = false;

            UseProperty("_dropPartMode");

            GUI.enabled = true;
        }
    }
}