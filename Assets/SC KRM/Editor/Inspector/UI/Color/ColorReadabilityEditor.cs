using SCKRM.UI;
using SCKRM.UI.Layout;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ColorReadability))]
    public sealed class ColorReadabilityEditor : UIAniEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawLine();

            UseProperty("_targetCanvasRenderer");
            UseProperty("_targetGraphic");
        }
    }
}