using UnityEngine;
using UnityEditor;
using SCKRM.SaveLoad;
using SCKRM.ProjectSetting;
using UnityEngine.UIElements;
using SCKRM.Resource;

namespace SCKRM.Editor
{
    public class DefaultNameSpaceProjectSetting : SettingsProvider
    {
        public DefaultNameSpaceProjectSetting(string path, SettingsScope scopes) : base(path, scopes) { }

        [SettingsProvider] public static SettingsProvider CreateSettingsProvider() => new DefaultNameSpaceProjectSetting("SC KRM/기본 네임스페이스", SettingsScope.Project);



        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            if (!Kernel.isPlaying)
            {
                if (resourceProjectSetting == null)
                    SaveLoadManager.Initialize<ProjectSettingSaveLoadAttribute>(typeof(ResourceManager.Data), out resourceProjectSetting);

                SaveLoadManager.Load(resourceProjectSetting, Kernel.projectSettingPath);
            }
        }

        bool deleteSafety = true;
        public override void OnGUI(string searchContext) => DrawGUI(ref deleteSafety);

        public static SaveLoadClass resourceProjectSetting;
        public static void DrawGUI(ref bool deleteSafety)
        {
            //GUI
            {
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("안전 삭제 모드 (삭제 할 리스트가 빈 값이 아니면 삭제 금지)", GUILayout.Width(330));
                deleteSafety = EditorGUILayout.Toggle(deleteSafety);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
            }

            CustomInspectorEditor.DrawList(ResourceManager.Data.nameSpaces, "네임스페이스", 0, 0, deleteSafety);

            if (GUI.changed && !Kernel.isPlaying)
                SaveLoadManager.Save(resourceProjectSetting, Kernel.projectSettingPath);
        }
    }
}
