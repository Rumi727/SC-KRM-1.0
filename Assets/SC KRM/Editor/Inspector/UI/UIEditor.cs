using SCKRM.UI;
using UnityEditor;

namespace SCKRM.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UI.UI))]
    public class UIEditor : CustomInspectorEditor
    {
        UI.UI editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (UI.UI)target;
        }

        /// <summary>
        /// Please put base.OnInspectorGUI() when overriding
        /// </summary>
        public override void OnInspectorGUI()
        {
            if (editor.rectTransform.gameObject != editor.gameObject)
            {
                EditorGUILayout.HelpBox("이 게임 오브젝트에 있는 RectTramsform 컴포넌트를 넣어야합니다!", MessageType.Error);
                UseProperty("_rectTransform");
            }

            if (editor.graphic != null && editor.graphic.gameObject != editor.gameObject)
            {
                EditorGUILayout.HelpBox("이 게임 오브젝트에 있는 그래픽 컴포넌트를 넣어야합니다!", MessageType.Error);
                UseProperty("_graphic");
            }

            EditorGUILayout.LabelField("Anchored Position: " + editor.rectTransform.anchoredPosition);
            EditorGUILayout.LabelField("Size Delta: " + editor.rectTransform.sizeDelta);

            Space();

            EditorGUILayout.LabelField("Offset Min: " + editor.rectTransform.offsetMin);
            EditorGUILayout.LabelField("Offset Max: " + editor.rectTransform.offsetMax);

            Space();

            EditorGUILayout.LabelField("Rect: " + editor.rectTransform.rect);
        }
    }

    [CanEditMultipleObjects]
    [CustomEditor(typeof(ObjectPoolingUI))]
    public class ObjectPoolingUIEditor : CustomInspectorEditor
    {
        ObjectPoolingUI editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (ObjectPoolingUI)target;
        }

        public override void OnInspectorGUI()
        {
            if (editor.rectTransform == null)
            {
                EditorGUILayout.HelpBox("이 게임 오브젝트의 RectTransform 컴포넌트를 넣어주세요", MessageType.Warning);
                UseProperty("_rectTransform");
            }
            else if (editor.rectTransform.gameObject != editor.gameObject)
            {
                EditorGUILayout.HelpBox("이 게임 오브젝트에 있는 RectTramsform 컴포넌트를 넣어야합니다!", MessageType.Error);
                UseProperty("_rectTransform");
            }

            if (editor.graphic == null)
            {
                EditorGUILayout.HelpBox("이 게임 오브젝트에 그래픽 컴포넌트가 있다면 넣어주세요", MessageType.Warning);
                UseProperty("_graphic");
            }
            else if (editor.graphic.gameObject != editor.gameObject)
            {
                EditorGUILayout.HelpBox("이 게임 오브젝트에 있는 그래픽 컴포넌트를 넣어야합니다!", MessageType.Error);
                UseProperty("_graphic");
            }

            EditorGUILayout.LabelField("Anchored Position: " + editor.rectTransform.anchoredPosition);
            EditorGUILayout.LabelField("Size Delta: " + editor.rectTransform.sizeDelta);

            Space();

            EditorGUILayout.LabelField("Offset Min: " + editor.rectTransform.offsetMin);
            EditorGUILayout.LabelField("Offset Max: " + editor.rectTransform.offsetMax);

            Space();

            EditorGUILayout.LabelField("Rect: " + editor.rectTransform.rect);
        }
    }

    [CanEditMultipleObjects]
    [CustomEditor(typeof(UIAni), true)]
    public class UIAniEditor : UIEditor
    {
        UIAni editor;

        protected override void OnEnable()
        {
            base.OnEnable();
            editor = (UIAni)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawLine();

            UseProperty("_lerp", "애니메이션 사용");

            if (editor.lerp)
                UseProperty("_lerpValue", "애니메이션 속도");
        }
    }
}