using UnityEngine;
using System.IO;
using SCKRM.Compress;
using SCKRM.Threads;
using System;
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
using B83.Win32;
using System.Collections.Generic;
#else
using System.Reflection;
using UnityEditor;
#endif

namespace SCKRM
{
    public class DragAndDropManager : Manager<DragAndDropManager>
    {
        /// <summary>
        /// </summary>
        /// <param name="paths">
        /// 드롭 된 파일 또는 폴더의 경로들
        /// Paths to dropped files or folders
        /// </param>
        /// <param name="isFolder">
        /// 경로가 폴더인지 감지합니다
        /// Detect if path is a folder
        /// </param>
        /// <param name="mousePos">
        /// 드롭 됐을 때의 마우스 위치
        /// mouse position when dropped
        /// </param>
        /// <returns>
        /// 메소드가 파일 감지에 성공했을 경우 true를 반환해야 하며, 감지에 실패했으면 false를 반환해야 합니다
        /// The method should return true if the file was detected successfully, and false if the detection was unsuccessful.
        /// </returns>
        public delegate bool DragAndDropFunc(string path, bool isFolder, Vector2 mousePos, ThreadMetaData threadMetaData);
        public static event DragAndDropFunc dragAndDropEvent;



#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
        void OnEnable()
        {
            UnityDragAndDropHook.InstallHook();
            UnityDragAndDropHook.OnDroppedFiles += OnFiles;

            void OnFiles(List<string> aFiles, POINT aPos) => DragAndDropEventInvoke(aFiles.ToArray(), new Vector2(aPos.x, Screen.height - aPos.y));
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
                        DragAndDropEventInvoke(tempDragAndDropPath, UnityEngine.Input.mousePosition);
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

        static void DragAndDropEventInvoke(string[] paths, Vector2 mousePos)
        {
            Delegate[] delegates = dragAndDropEvent?.GetInvocationList();
            if (delegates == null || delegates.Length <= 0)
                return;

            for (int i = 0; i < paths.Length; i++)
            {
                string path = paths[i];

                ThreadManager.Create(DragAndDrop, "notice.running_task.drag_and_drop.file_load");

                void DragAndDrop(ThreadMetaData threadMetaData)
                {
                    bool isFolder = Directory.Exists(path);
                    bool isCompressedFile = false;
                    if (!isFolder)
                    {
                        if (!File.Exists(path))
                            return;
                        else if (Path.GetExtension(path).ToLower().Equals(".zip"))
                        {
                            string uuid = Guid.NewGuid().ToString();
                            string tempFilePath = PathTool.Combine(Kernel.temporaryCachePath, uuid);
                            if (!CompressFileManager.DecompressZipFile(path, tempFilePath, "", threadMetaData))
                                return;

                            path = tempFilePath;
                            isFolder = true;
                            isCompressedFile = true;
                        }
                    }

                    if (delegates != null)
                    for (int j = 0; j < delegates.Length; j++)
                    {
                        try
                        {
                            if (((DragAndDropFunc)delegates[j]).Invoke(path, isFolder, mousePos, threadMetaData))
                                break;
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }

                    if (isCompressedFile && Directory.Exists(path))
                        Directory.Delete(path, true);
                }
            }
        }
    }
}
