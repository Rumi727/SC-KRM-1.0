using UnityEngine;
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
using B83.Win32;
using System.Collections.Generic;
#else
using System;
using System.Reflection;
using UnityEditor;
#endif

namespace SCKRM
{
    public class DragAndDropManager : Manager<DragAndDropManager>
    {
        public static event DragAndDropAction dragAndDropEvent;
        public delegate void DragAndDropAction(string[] paths, Vector2 mousePos);



#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
        void OnEnable()
        {
            UnityDragAndDropHook.InstallHook();
            UnityDragAndDropHook.OnDroppedFiles += OnFiles;

            void OnFiles(List<string> aFiles, POINT aPos) => dragAndDropEvent?.Invoke(aFiles.ToArray(), new Vector2(aPos.x, Screen.height - aPos.y));
        }

        void OnDisable() => UnityDragAndDropHook.UninstallHook();
#endif



#if UNITY_EDITOR
        string[] tempDragAndDropPath = null;
        bool drag = false;
        Assembly assembly = typeof(EditorWindow).Assembly;
        Type type;
        void Update()
        {
            if (type == null)
            {
                type = assembly.GetType("UnityEditor.GameView");
                return;
            }

            string[] paths = DragAndDrop.paths;
            if (EditorWindow.mouseOverWindow != null && EditorWindow.mouseOverWindow.GetType() == type)
            {
                if ((paths == null || paths.Length <= 0) && tempDragAndDropPath != null && tempDragAndDropPath.Length > 0)
                {
                    if (drag)
                    {
                        drag = false;
                        dragAndDropEvent?.Invoke(tempDragAndDropPath, UnityEngine.Input.mousePosition);
                    }
                    else
                        drag = true;
                }
            }
            else
                drag = false;

            tempDragAndDropPath = paths;
        }
#endif
    }
}