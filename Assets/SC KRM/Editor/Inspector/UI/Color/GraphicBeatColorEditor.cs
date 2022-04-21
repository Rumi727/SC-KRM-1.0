using SCKRM.UI;
using SCKRM.UI.Layout;
using UnityEditor;
using UnityEngine;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GraphicBeatColor))]
    public sealed class GraphicBeatColorEditor : UIEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawLine();

            UseProperty("_alpha");

            if (Application.isPlaying)
                GUI.enabled = false;

            UseProperty("_dropPartMode");

            GUI.enabled = true;
        }
    }
}