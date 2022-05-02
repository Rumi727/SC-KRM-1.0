using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using SCKRM.Resource;
using SCKRM.ProjectSetting;

namespace SCKRM.Editor
{
    public class CustomInspectorEditor : UnityEditor.Editor
    {
        [System.NonSerialized] bool repaint = false;

        /// <summary>
        /// Please put base.OnEnable() when overriding
        /// </summary>
        protected virtual void OnEnable()
        {
            if (Application.isPlaying)
            {
                repaint = true;
                Repainter();
            }
        }

        /// <summary>
        /// Please put base.OnDisable() when overriding
        /// </summary>
        protected virtual void OnDisable() => repaint = false;

        async void Repainter()
        {
            while (repaint)
            {
                Repaint();
                await Task.Delay(100);
            }
        }

        public override void OnInspectorGUI()
        {

        }

        public void UseProperty(string propertyName)
        {
            SerializedProperty tps = serializedObject.FindProperty(propertyName);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(tps, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }

        public void UseProperty(string propertyName, string label)
        {
            GUIContent GUIContent = new GUIContent();
            GUIContent.text = label;

            SerializedProperty tps = serializedObject.FindProperty(propertyName);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(tps, GUIContent, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }

        public static void DrawLine(int thickness = 1, int padding = 10) => DrawLine(new Color(0.4980392f, 0.4980392f, 0.4980392f), thickness, padding);

        public static void DrawLine(Color color, int thickness = 2, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 18;
            r.width += 22;
            EditorGUI.DrawRect(r, color);
        }

        public static void Space() => EditorGUILayout.Space();

        public static void DrawList(List<int> list, string label, int tab = 0, int tab2 = 0, bool deleteSafety = true) => drawList(list, label, false, Vector2.zero, tab, tab2, deleteSafety);
        public static Vector2 DrawList(List<int> list, string label, Vector2 scrollViewPos, int tab = 0, int tab2 = 0, bool deleteSafety = true) => drawList(list, label, true, scrollViewPos, tab, tab2, deleteSafety);
        static Vector2 drawList(List<int> list, string label, bool scrollView, Vector2 scrollViewPos, int tab, int tab2, bool deleteSafety)
        {
            //GUI
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(30 * tab);

                {
                    if (GUILayout.Button("추가", GUILayout.ExpandWidth(false)))
                        list.Add(0);
                }

                {
                    if (list.Count <= 0 || (list[list.Count - 1] != 0 && deleteSafety))
                        GUI.enabled = false;

                    if (GUILayout.Button("삭제", GUILayout.ExpandWidth(false)) && list.Count > 0)
                        list.RemoveAt(list.Count - 1);

                    GUI.enabled = true;
                }

                {
                    int count = EditorGUILayout.IntField("리스트 길이", list.Count, GUILayout.Height(21));
                    //변수 설정
                    if (count < 0)
                        count = 0;

                    if (count > list.Count)
                    {
                        for (int i = list.Count; i < count; i++)
                            list.Add(0);
                    }
                    else if (count < list.Count)
                    {
                        for (int i = list.Count - 1; i >= count; i--)
                        {
                            if (list.Count > 0 && (list[list.Count - 1] == 0 || !deleteSafety))
                                list.RemoveAt(list.Count - 1);
                            else
                                count++;
                        }
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();

            if (scrollView)
                scrollViewPos = EditorGUILayout.BeginScrollView(scrollViewPos);

            for (int i = 0; i < list.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(30 * (tab + tab2));

                GUILayout.Label(label, GUILayout.ExpandWidth(false));
                list[i] = EditorGUILayout.IntField(list[i]);

                {
                    if (i - 1 < 0)
                        GUI.enabled = false;

                    if (GUILayout.Button("위로", GUILayout.ExpandWidth(false)))
                        list.Move(i, i - 1);

                    GUI.enabled = true;
                }

                {
                    if (i + 1 >= list.Count)
                        GUI.enabled = false;

                    if (GUILayout.Button("아래로", GUILayout.ExpandWidth(false)))
                        list.Move(i, i + 1);

                    GUI.enabled = true;
                }

                {
                    if (list[i] != 0)
                        GUI.enabled = false;

                    if (GUILayout.Button("삭제", GUILayout.ExpandWidth(false)))
                        list.RemoveAt(i);

                    GUI.enabled = true;
                }

                EditorGUILayout.EndHorizontal();
            }

            if (scrollView)
                EditorGUILayout.EndScrollView();

            return scrollViewPos;
        }

        public static void DrawList(List<float> list, string label, int tab = 0, int tab2 = 0, bool deleteSafety = true) => drawList(list, label, false, Vector2.zero, tab, tab2, deleteSafety);
        public static Vector2 DrawList(List<float> list, string label, Vector2 scrollViewPos, int tab = 0, int tab2 = 0, bool deleteSafety = true) => drawList(list, label, true, scrollViewPos, tab, tab2, deleteSafety);
        static Vector2 drawList(List<float> list, string label, bool scrollView, Vector2 scrollViewPos, int tab, int tab2, bool deleteSafety)
        {
            //GUI
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(30 * tab);

                {
                    if (GUILayout.Button("추가", GUILayout.ExpandWidth(false)))
                        list.Add(0);
                }

                {
                    if (list.Count <= 0 || (list[list.Count - 1] != 0 && deleteSafety))
                        GUI.enabled = false;

                    if (GUILayout.Button("삭제", GUILayout.ExpandWidth(false)) && list.Count > 0)
                        list.RemoveAt(list.Count - 1);

                    GUI.enabled = true;
                }

                {
                    int count = EditorGUILayout.IntField("리스트 길이", list.Count, GUILayout.Height(21));
                    //변수 설정
                    if (count < 0)
                        count = 0;

                    if (count > list.Count)
                    {
                        for (int i = list.Count; i < count; i++)
                            list.Add(0);
                    }
                    else if (count < list.Count)
                    {
                        for (int i = list.Count - 1; i >= count; i--)
                        {
                            if (list.Count > 0 && (list[list.Count - 1] == 0 || !deleteSafety))
                                list.RemoveAt(list.Count - 1);
                            else
                                count++;
                        }
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();

            if (scrollView)
                scrollViewPos = EditorGUILayout.BeginScrollView(scrollViewPos);

            for (int i = 0; i < list.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(30 * (tab + tab2));

                GUILayout.Label(label, GUILayout.ExpandWidth(false));
                list[i] = EditorGUILayout.FloatField(list[i]);

                {
                    if (i - 1 < 0)
                        GUI.enabled = false;

                    if (GUILayout.Button("위로", GUILayout.ExpandWidth(false)))
                        list.Move(i, i - 1);

                    GUI.enabled = true;
                }

                {
                    if (i + 1 >= list.Count)
                        GUI.enabled = false;

                    if (GUILayout.Button("아래로", GUILayout.ExpandWidth(false)))
                        list.Move(i, i + 1);

                    GUI.enabled = true;
                }

                {
                    if (list[i] != 0)
                        GUI.enabled = false;

                    if (GUILayout.Button("삭제", GUILayout.ExpandWidth(false)))
                        list.RemoveAt(i);

                    GUI.enabled = true;
                }

                EditorGUILayout.EndHorizontal();
            }

            if (scrollView)
                EditorGUILayout.EndScrollView();

            return scrollViewPos;
        }

        public static void DrawList(List<double> list, string label, int tab = 0, int tab2 = 0, bool deleteSafety = true) => drawList(list, label, false, Vector2.zero, tab, tab2, deleteSafety);
        public static Vector2 DrawList(List<double> list, string label, Vector2 scrollViewPos, int tab = 0, int tab2 = 0, bool deleteSafety = true) => drawList(list, label, true, scrollViewPos, tab, tab2, deleteSafety);
        static Vector2 drawList(List<double> list, string label, bool scrollView, Vector2 scrollViewPos, int tab, int tab2, bool deleteSafety)
        {
            //GUI
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(30 * tab);

                {
                    if (GUILayout.Button("추가", GUILayout.ExpandWidth(false)))
                        list.Add(0);
                }

                {
                    if (list.Count <= 0 || (list[list.Count - 1] != 0 && deleteSafety))
                        GUI.enabled = false;

                    if (GUILayout.Button("삭제", GUILayout.ExpandWidth(false)) && list.Count > 0)
                        list.RemoveAt(list.Count - 1);

                    GUI.enabled = true;
                }

                {
                    int count = EditorGUILayout.IntField("리스트 길이", list.Count, GUILayout.Height(21));
                    //변수 설정
                    if (count < 0)
                        count = 0;

                    if (count > list.Count)
                    {
                        for (int i = list.Count; i < count; i++)
                            list.Add(0);
                    }
                    else if (count < list.Count)
                    {
                        for (int i = list.Count - 1; i >= count; i--)
                        {
                            if (list.Count > 0 && (list[list.Count - 1] == 0 || !deleteSafety))
                                list.RemoveAt(list.Count - 1);
                            else
                                count++;
                        }
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();

            if (scrollView)
                scrollViewPos = EditorGUILayout.BeginScrollView(scrollViewPos);

            for (int i = 0; i < list.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(30 * (tab + tab2));

                GUILayout.Label(label, GUILayout.ExpandWidth(false));
                list[i] = EditorGUILayout.DoubleField(list[i]);

                {
                    if (i - 1 < 0)
                        GUI.enabled = false;

                    if (GUILayout.Button("위로", GUILayout.ExpandWidth(false)))
                        list.Move(i, i - 1);

                    GUI.enabled = true;
                }

                {
                    if (i + 1 >= list.Count)
                        GUI.enabled = false;

                    if (GUILayout.Button("아래로", GUILayout.ExpandWidth(false)))
                        list.Move(i, i + 1);

                    GUI.enabled = true;
                }

                {
                    if (list[i] != 0)
                        GUI.enabled = false;

                    if (GUILayout.Button("삭제", GUILayout.ExpandWidth(false)))
                        list.RemoveAt(i);

                    GUI.enabled = true;
                }

                EditorGUILayout.EndHorizontal();
            }

            if (scrollView)
                EditorGUILayout.EndScrollView();

            return scrollViewPos;
        }

        public static void DrawList(List<string> list, string label, int tab = 0, int tab2 = 0, bool deleteSafety = true) => drawList(list, label, false, Vector2.zero, tab, tab2, deleteSafety);
        public static Vector2 DrawList(List<string> list, string label, Vector2 scrollViewPos, int tab = 0, int tab2 = 0, bool deleteSafety = true) => drawList(list, label, true, scrollViewPos, tab, tab2, deleteSafety);
        static Vector2 drawList(List<string> list, string label, bool scrollView, Vector2 scrollViewPos, int tab, int tab2, bool deleteSafety)
        {
            //GUI
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(30 * tab);

                {
                    if (GUILayout.Button("추가", GUILayout.ExpandWidth(false)))
                        list.Add("");
                }

                {
                    if (list.Count <= 0 || (list[list.Count - 1] != null && list[list.Count - 1] != "" && deleteSafety))
                        GUI.enabled = false;

                    if (GUILayout.Button("삭제", GUILayout.ExpandWidth(false)) && list.Count > 0)
                        list.RemoveAt(list.Count - 1);

                    GUI.enabled = true;
                }

                {
                    int count = EditorGUILayout.IntField("리스트 길이", list.Count, GUILayout.Height(21));
                    //변수 설정
                    if (count < 0)
                        count = 0;

                    if (count > list.Count)
                    {
                        for (int i = list.Count; i < count; i++)
                            list.Add("");
                    }
                    else if (count < list.Count)
                    {
                        for (int i = list.Count - 1; i >= count; i--)
                        {
                            if (list.Count > 0 && (list[list.Count - 1] == "" || !deleteSafety))
                                list.RemoveAt(list.Count - 1);
                            else
                                count++;
                        }
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();

            if (scrollView)
                scrollViewPos = EditorGUILayout.BeginScrollView(scrollViewPos);

            for (int i = 0; i < list.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(30 * (tab + tab2));

                GUILayout.Label(label, GUILayout.ExpandWidth(false));
                list[i] = EditorGUILayout.TextField(list[i]);

                {
                    if (i - 1 < 0)
                        GUI.enabled = false;

                    if (GUILayout.Button("위로", GUILayout.ExpandWidth(false)))
                        list.Move(i, i - 1);

                    GUI.enabled = true;
                }

                {
                    if (i + 1 >= list.Count)
                        GUI.enabled = false;

                    if (GUILayout.Button("아래로", GUILayout.ExpandWidth(false)))
                        list.Move(i, i + 1);

                    GUI.enabled = true;
                }

                {
                    if (list[i] != null && list[i] != "")
                        GUI.enabled = false;

                    if (GUILayout.Button("삭제", GUILayout.ExpandWidth(false)))
                        list.RemoveAt(i);

                    GUI.enabled = true;
                }

                EditorGUILayout.EndHorizontal();
            }

            if (scrollView)
                EditorGUILayout.EndScrollView();

            return scrollViewPos;
        }

        public static void DrawList<T>(List<T> list, string label, Action<int> action, int tab = 0, int tab2 = 0, bool deleteSafety = true) => drawList(list, label, action, false, Vector2.zero, tab, tab2, deleteSafety);
        public static Vector2 DrawList<T>(List<T> list, string label, Action<int> action, Vector2 scrollViewPos, int tab = 0, int tab2 = 0, bool deleteSafety = true) => drawList(list, label, action, true, scrollViewPos, tab, tab2, deleteSafety);
        static Vector2 drawList<T>(List<T> list, string label, Action<int> action, bool scrollView, Vector2 scrollViewPos, int tab, int tab2, bool deleteSafety)
        {
            //GUI
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(30 * tab);

                {
                    if (GUILayout.Button("추가", GUILayout.ExpandWidth(false)))
                        list.Add(default(T));
                }

                {
                    if (list.Count <= 0 || (list[list.Count - 1] != null && !list[list.Count - 1].Equals(default(T)) && deleteSafety))
                        GUI.enabled = false;

                    if (GUILayout.Button("삭제", GUILayout.ExpandWidth(false)) && list.Count > 0)
                        list.RemoveAt(list.Count - 1);

                    GUI.enabled = true;
                }

                {
                    int count = EditorGUILayout.IntField("리스트 길이", list.Count, GUILayout.Height(21));
                    //변수 설정
                    if (count < 0)
                        count = 0;

                    if (count > list.Count)
                    {
                        for (int i = list.Count; i < count; i++)
                            list.Add(default(T));
                    }
                    else if (count < list.Count)
                    {
                        for (int i = list.Count - 1; i >= count; i--)
                        {
                            if (list.Count > 0 && (list[list.Count - 1] == null || list[list.Count - 1].Equals(default(T)) || !deleteSafety))
                                list.RemoveAt(list.Count - 1);
                            else
                                count++;
                        }
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();

            if (scrollView)
                scrollViewPos = EditorGUILayout.BeginScrollView(scrollViewPos);

            for (int i = 0; i < list.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(30 * (tab + tab2));

                GUILayout.Label(label, GUILayout.ExpandWidth(false));
                action.Invoke(i);

                {
                    if (i - 1 < 0)
                        GUI.enabled = false;

                    if (GUILayout.Button("위로", GUILayout.ExpandWidth(false)))
                        list.Move(i, i - 1);

                    GUI.enabled = true;
                }

                {
                    if (i + 1 >= list.Count)
                        GUI.enabled = false;

                    if (GUILayout.Button("아래로", GUILayout.ExpandWidth(false)))
                        list.Move(i, i + 1);

                    GUI.enabled = true;
                }

                {
                    if (list[i] != null && !list[i].Equals(default(T)))
                        GUI.enabled = false;

                    if (GUILayout.Button("삭제", GUILayout.ExpandWidth(false)))
                        list.RemoveAt(i);

                    GUI.enabled = true;
                }

                EditorGUILayout.EndHorizontal();
            }

            if (scrollView)
                EditorGUILayout.EndScrollView();

            return scrollViewPos;
        }

        public static string DrawNameSpace(string nameSpace) => DrawStringArray(nameSpace, ResourceManager.nameSpaces.ToArray());
        public static string DrawNameSpace(string label, string nameSpace) => DrawStringArray(label, nameSpace, ResourceManager.nameSpaces.ToArray());

        public static string DrawStringArray(string value, string[] array)
        {
            if (array == null)
                return "";

            int index = EditorGUILayout.Popup(Array.IndexOf(array, value), array, GUILayout.MinWidth(0));
            if (index >= 0)
                return array[index];
            else
                return value;
        }
        public static string DrawStringArray(string label, string value, string[] array)
        {
            if (array == null)
                return "";

            int index = EditorGUILayout.Popup(label, Array.IndexOf(array, value), array, GUILayout.MinWidth(0));
            if (index >= 0)
                return array[index];
            else
                return value;
        }
    }
}