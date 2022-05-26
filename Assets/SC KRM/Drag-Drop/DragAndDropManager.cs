using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using B83.Win32;
using SCKRM.Input;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SCKRM
{
    public class DragAndDropManager : MonoBehaviour
    {
        public event DragAndDropAction dragAndDropEvent;

        public delegate void DragAndDropAction(string[] paths, Vector2 mousePos);

        void OnEnable()
        {
            UnityDragAndDropHook.InstallHook();
            UnityDragAndDropHook.OnDroppedFiles += OnFiles;

            void OnFiles(List<string> aFiles, POINT aPos) => dragAndDropEvent?.Invoke(aFiles.ToArray(), new Vector2(aPos.x, Screen.height - aPos.y));
        }



#if UNITY_EDITOR
        bool dragAndDropLock = false;
        void Update()
        {
            string[] paths = DragAndDrop.paths;
            if (paths != null && paths.Length > 0 && CursorManager.isFocused)
            {
                if (!dragAndDropLock)
                {
                    Debug.Log("asdf");
                    dragAndDropEvent?.Invoke(paths, InputManager.mousePosition);
                }

                dragAndDropLock = true;
            }
            else
                dragAndDropLock = false;
        }
#endif

        void OnDisable() => UnityDragAndDropHook.UninstallHook();
    }
}
