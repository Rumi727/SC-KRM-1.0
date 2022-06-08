using Cysharp.Threading.Tasks;
using Mono.CSharp;
using SCKRM.FileDialog.Drive;
using SCKRM.FileDialog.MyPC;
using SCKRM.FileDialog.Screen;
using SCKRM.FileDialog.ShortcurBar;
using SCKRM.Input;
using SCKRM.Language;
using SCKRM.Renderer;
using SCKRM.Resource;
using SCKRM.UI;
using SCKRM.UI.MessageBox;
using SCKRM.UI.StatusBar;
using SFB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SCKRM.FileDialog
{
    public sealed class FileDialogManager : Manager<FileDialogManager>
    {
        public static bool isFileDialogShow { get; private set; }



        public static List<string> selectedFilePath { get; set; } = new List<string>();
        public static string currentPath { get; private set; } = "";
        public static string savedPath { get; private set; } = "";



        public static string currentSearch { get; private set; } = "";



        public static ExtensionFilter currentFilter { get; private set; } = new ExtensionFilter();
        public static ExtensionFilter[] allExtensionFilter { get; private set; } = new ExtensionFilter[0];



        public static string fileName { get; private set; } = "";



        public static bool isSingle { get; private set; } = false;
        public static bool isFileSaveMode { get; private set; } = false;
        public static bool isFolderOpenMode { get; private set; } = false;



        public static List<string> undoPath { get; } = new List<string>();
        public static int currentUndoIndex { get; private set; } = 0;



        static CancellationTokenSource _showMethodCancelTokenSource = new CancellationTokenSource();
        static CancellationTokenSource showMethodCancelTokenSource => _showMethodCancelTokenSource; 
        static CancellationToken showMethodCancelToken => showMethodCancelTokenSource.Token;

        static bool cancelButIsSuccess = false;



        [SerializeField] CanvasGroup fileDialogCanvasGroup;
        [SerializeField] GameObject fileDialogBG;
        [SerializeField] GameObject fileDialogScreen;
        [SerializeField] GameObject fileDialogMyPC;
        [SerializeField] FileDialogScreen fileDialogScreenContent;
        [SerializeField] FileDialogMyPC fileDialogMyPCContent;
        [SerializeField] FileDialogShortcutBar fileDialogShortcutBar;
        [SerializeField] TMP_InputField fileDialogSearch;
        [SerializeField] TMP_InputField fileDialogFileName;
        [SerializeField] TMP_InputField fileDialogPath;
        [SerializeField] TMP_InputField fileDialogTitle;
        [SerializeField] UI.Dropdown fileDialogFilter;
        [SerializeField] CustomAllTextRenderer fileDialogSaveOpenButtonText;
        [SerializeField] Button fileDialogSaveOpenButton;

        void Awake() => SingletonCheck(this);

        void Update()
        {
            if (isFileDialogShow)
            {
                fileDialogCanvasGroup.alpha = fileDialogCanvasGroup.alpha.Lerp(1, 0.2f * Kernel.fpsUnscaledDeltaTime);
                if (fileDialogCanvasGroup.alpha > 0.99f)
                    fileDialogCanvasGroup.alpha = 1;

                fileDialogCanvasGroup.interactable = true;
                fileDialogCanvasGroup.blocksRaycasts = true;

                if (!fileDialogBG.activeSelf)
                    fileDialogBG.SetActive(true);
            }
            else
            {
                fileDialogCanvasGroup.alpha = fileDialogCanvasGroup.alpha.Lerp(0, 0.2f * Kernel.fpsUnscaledDeltaTime);
                if (fileDialogCanvasGroup.alpha < 0.01f)
                    fileDialogCanvasGroup.alpha = 0;

                fileDialogCanvasGroup.interactable = false;
                fileDialogCanvasGroup.blocksRaycasts = false;

                if (fileDialogBG.activeSelf)
                    fileDialogBG.SetActive(false);
            }
        }

        void OnApplicationFocus(bool focus)
        {
            if (isFileDialogShow)
                AllRefresh();
        }

        void OnDestroy() => HideForce(true);

        public static void AllRefresh()
        {
            instance.fileDialogShortcutBar.Refresh();
            ScreenRefresh(false);
        }


        public static void ScreenRefresh() => ScreenRefresh(currentPath, true);
        public static void ScreenRefresh(bool undoAllow) => ScreenRefresh(currentPath, undoAllow);
        public static void ScreenRefresh(string path) => ScreenRefresh(path, true);
        public static void ScreenRefresh(string path, bool undoAllow)
        {
            path = PathTool.ReplaceInvalidPathChars(path).Replace("\\", "/");
            currentPath = path;

            if (undoAllow)
            {
                if (currentUndoIndex + 1 < undoPath.Count)
                    undoPath.RemoveRange(currentUndoIndex + 1, undoPath.Count - currentUndoIndex - 1);

                undoPath.Add(path);
                currentUndoIndex++;
            }

            if (string.IsNullOrEmpty(path))
            {
                selectedFilePath.Clear();

                if (instance.fileDialogScreen.activeSelf)
                    instance.fileDialogScreen.SetActive(false);

                if (!instance.fileDialogMyPC.activeSelf)
                    instance.fileDialogMyPC.SetActive(true);

                instance.fileDialogMyPCContent.Refresh();
            }
            else
            {
                if (!Directory.Exists(path))
                {
                    Up();
                    return;
                }

                selectedFilePath.Clear();

                if (instance.fileDialogMyPC.activeSelf)
                    instance.fileDialogMyPC.SetActive(false);

                if (!instance.fileDialogScreen.activeSelf)
                    instance.fileDialogScreen.SetActive(true);

                instance.fileDialogScreenContent.Refresh();
            }

            instance.fileDialogPath.text = currentPath;
        }

        public static void Undo()
        {
            if (currentUndoIndex > 0)
            {
                currentUndoIndex--;
                ScreenRefresh(undoPath[currentUndoIndex], false);
            }
        }

        public static void Redo()
        {
            if (currentUndoIndex < undoPath.Count - 1)
            {
                currentUndoIndex++;
                ScreenRefresh(undoPath[currentUndoIndex], false);
            }
        }

        public static void Up()
        {
            if (string.IsNullOrEmpty(currentPath))
                return;

            DirectoryInfo directoryInfo = Directory.GetParent(currentPath);
            if (directoryInfo != null)
                ScreenRefresh(directoryInfo.ToString());
        }

        /// <summary>
        /// 폴더 열기
        /// </summary>
        /// <param name="single">
        /// 단일 폴더
        /// </param>
        /// <returns></returns>
        public static UniTask<(bool isSuccess, string[] selectedPath)> ShowFolderOpen(string title, bool single = false)
        {
            isFolderOpenMode = true;
            isFileSaveMode = false;

            instance.fileDialogFilter.gameObject.SetActive(false);
            instance.fileDialogFileName.gameObject.SetActive(false);

            instance.fileDialogSaveOpenButtonText.nameSpacePathPair = "sc-krm:gui.folder_select";
            instance.fileDialogSaveOpenButtonText.Refresh();

            return show(title, single);
        }

        /// <summary>
        /// 파일 열기
        /// </summary>
        /// <param name="single">
        /// 단일 파일
        /// </param>
        /// <param name="extensionFilters">
        /// 확장자 필터
        /// </param>
        /// <returns></returns>
        public static async UniTask<(bool isSuccess, string[] selectedPath)> ShowFileOpen(string title, bool single = false, params ExtensionFilter[] extensionFilters)
        {
            isFolderOpenMode = false;
            isFileSaveMode = false;

            instance.fileDialogFilter.gameObject.SetActive(true);
            instance.fileDialogFileName.gameObject.SetActive(false);

            instance.fileDialogSaveOpenButtonText.nameSpacePathPair = "sc-krm:gui.open";
            instance.fileDialogSaveOpenButtonText.Refresh();

            return await show(title, single, extensionFilters);
        }

        /// <summary>
        /// 파일 저장
        /// </summary>
        /// <param name="extensionFilters">
        /// 확장자 필터
        /// </param>
        /// <returns></returns>
        public static async UniTask<(bool isSuccess, string selectedPath)> ShowFileSave(string title, params ExtensionFilter[] extensionFilters)
        {
            isFolderOpenMode = false;
            isFileSaveMode = true;

            instance.fileDialogFilter.gameObject.SetActive(true);
            instance.fileDialogFileName.gameObject.SetActive(true);
            instance.fileDialogFileName.text = "";

            instance.fileDialogSaveOpenButtonText.nameSpacePathPair = "sc-krm:gui.save";
            instance.fileDialogSaveOpenButtonText.Refresh();

            (bool isSuccess, _) = await show(title, true, extensionFilters);
            if (isSuccess)
                return (true, GetSaveFilePath(currentPath));
            else
                return (false, "");
        }




        static async UniTask<(bool isSuccess, string[] selectedPath)> show(string title, bool single, params ExtensionFilter[] extensionFilters)
        {
            instance.fileDialogTitle.text = title;
            isSingle = single;

            isFileDialogShow = true;
            selectedFilePath.Clear();
            currentPath = savedPath;

            currentUndoIndex = 0;
            undoPath.Clear();
            undoPath.Add(currentPath);

            #region Filter
            if (extensionFilters == null || extensionFilters.Length <= 0)
                extensionFilters = new ExtensionFilter[] { ExtensionFilter.allFileFilter };

            currentFilter = extensionFilters[0];

            allExtensionFilter = new ExtensionFilter[extensionFilters.Length];

            instance.fileDialogFilter.options = new string[extensionFilters.Length];
            instance.fileDialogFilter.value = 0;

            for (int i = 0; i < extensionFilters.Length; i++)
            {
                ExtensionFilter extensionFilter = extensionFilters[i];
                string[] extensions = extensionFilter.extensions;
                string label = LanguageManager.LanguageLoad(extensionFilter.label.path, extensionFilter.label.nameSpace);
                label += " (";

                for (int j = 0; j < extensions.Length; j++)
                {
                    if (j >= extensions.Length - 1)
                        label += extensions[j];
                    else
                        label += extensions[j] + ", ";
                }

                label += ")";

                allExtensionFilter[i] = extensionFilter;
                instance.fileDialogFilter.options[i] = label;
            }
            #endregion

            AllRefresh();



            GameObject previousTabSelectGameObject = StatusBarManager.tabSelectGameObject;
            StatusBarManager.tabSelectGameObject = instance.fileDialogSaveOpenButton.gameObject;

            GameObject previouslySelectedGameObject = EventSystem.current.currentSelectedGameObject;
            EventSystem.current.SetSelectedGameObject(instance.fileDialogSaveOpenButton.gameObject);

            bool previousForceInputLock = InputManager.forceInputLock;
            InputManager.forceInputLock = true;

            bool backEventInvoke = false;
            bool clickEventInvoke = false;

            //이벤트를 추가합니다
            UIManager.BackEventAdd(BackEvent, true);
            instance.fileDialogSaveOpenButton.onClick.AddListener(ClickEvent);

            //버튼을 클릭하거나 뒤로 돌아갈때까지 기다립니다
            while (true)
            {
                if (await UniTask.WaitUntil(() => backEventInvoke || clickEventInvoke, PlayerLoopTiming.Update, showMethodCancelToken).SuppressCancellationThrow())
                {
                    if (!Kernel.isPlaying)
                        return (false, null);

                    _showMethodCancelTokenSource = new CancellationTokenSource();

                    if (!cancelButIsSuccess)
                        backEventInvoke = true;
                    else
                        clickEventInvoke = true;

                    cancelButIsSuccess = false;
                }

                //뒤로 가기 이벤트로 취소 된 경우에는 바로 빠져나갑니다
                if (backEventInvoke)
                    break;
                else if (isFileSaveMode) //파일 저장 모드일때
                {
                    string filePath = GetSaveFilePath(currentPath);
                    if (File.Exists(filePath)) //만약 저장 할 곳에 파일이 이미 있다면 경고 창을 띄웁니다
                    {
                        NameSpacePathReplacePair[] buttons = new NameSpacePathReplacePair[] { "sc-krm:gui.yes", "sc-krm:gui.no" };
                        NameSpacePathReplacePair info = new NameSpacePathReplacePair("sc-krm", "file_dialog.file_save_warning", new ReplaceOldNewPair("%value%", Path.GetFileName(filePath)));

                        //리턴 값이 0이 아니면 ('예'가 아니면) 다시 루프시킵니다
                        if (await MessageBoxManager.Show(buttons, 1, info, "sc-krm:0:gui/icon/exclamation_mark") != 0)
                            clickEventInvoke = false;
                        else //리턴 값이 0이면 ('예'면) 루프를 빠져나갑니다
                            break;
                    }
                    else //만약 저장 할 곳이 비어있다면 바로 빠져나갑니다
                        break;
                }
                else if (!string.IsNullOrEmpty(currentPath) && (Directory.Exists(currentPath) || File.Exists(currentPath))) //currentPath 프로퍼티가 비어있지 않다면 바로 빠져나갑니다
                    break;
                else //3개 다 아닐경우엔 다시 루프 시킵니다
                    clickEventInvoke = false;
            }

            //이벤트를 삭제합니다
            instance.fileDialogSaveOpenButton.onClick.RemoveListener(ClickEvent);

            isFileDialogShow = false;

            StatusBarManager.tabSelectGameObject = previousTabSelectGameObject;
            EventSystem.current.SetSelectedGameObject(previouslySelectedGameObject);

            InputManager.forceInputLock = previousForceInputLock;

            UIManager.BackEventRemove(BackEvent, true);

            if (backEventInvoke)
                return (false, null);
            else
            {
                if (!isFolderOpenMode)
                    return (true, selectedFilePath.ToArray());
                else
                {
                    if (selectedFilePath == null || selectedFilePath.Count <= 0)
                        return (true, new string[] { currentPath });
                    else
                        return (true, selectedFilePath.ToArray());
                }
            }

            void BackEvent() => backEventInvoke = true;
            void ClickEvent() => clickEventInvoke = true;
        }

        public static string GetSaveFilePath(string path)
        {
            if (!isFileSaveMode)
                return "";

            string fileName = instance.fileDialogFileName.text;
            if (Path.GetExtension(fileName) == "" && currentFilter.extensions.Length > 0 && !currentFilter.extensions[0].Contains("*"))
                return PathTool.Combine(path, fileName + "." + currentFilter.extensions[0]);
            else
                return PathTool.Combine(path, fileName);
        }

        public static void Search(string value)
        {
            value = PathTool.ReplaceInvalidFileNameChars(value);

            currentSearch = value;
            instance.fileDialogSearch.text = value;

            ScreenRefresh(false);
        }

        public static void SetFilter(int index)
        {
            currentFilter = allExtensionFilter[index];
            instance.fileDialogFilter.value = index;

            ScreenRefresh(false);
        }

        public static void SetFileName(string value)
        {
            value = PathTool.ReplaceInvalidFileNameChars(value);

            fileName = value;
            instance.fileDialogFileName.text = value;
        }

        public static void HideForce(bool isSuccess)
        {
            cancelButIsSuccess = isSuccess;
            showMethodCancelTokenSource.Cancel();
        }
    }

    public struct ExtensionFilter
    {
        readonly static ExtensionFilter _allFileFilter = new ExtensionFilter((NameSpacePathPair)"sc-krm:file_dialog.file.all", "*");
        public static ExtensionFilter allFileFilter => _allFileFilter;



        readonly static ExtensionFilter _videoFileFilter = new ExtensionFilter((NameSpacePathPair)"sc-krm:file_dialog.file.video", ResourceManager.videoExtension);
        public static ExtensionFilter videoFileFilter => _videoFileFilter;

        readonly static ExtensionFilter _textFileFilter = new ExtensionFilter((NameSpacePathPair)"sc-krm:file_dialog.file.text", ResourceManager.textExtension);
        public static ExtensionFilter textFileFilter => _textFileFilter;

        readonly static ExtensionFilter _pictureFileFilter = new ExtensionFilter((NameSpacePathPair)"sc-krm:file_dialog.file.picture", ResourceManager.textureExtension);
        public static ExtensionFilter pictureFileFilter => _pictureFileFilter;

        readonly static ExtensionFilter _musicFileFilter = new ExtensionFilter((NameSpacePathPair)"sc-krm:file_dialog.file.music", ResourceManager.audioExtension);
        public static ExtensionFilter musicFileFilter => _musicFileFilter;

        readonly static ExtensionFilter _compressFileFilter = new ExtensionFilter((NameSpacePathPair)"sc-krm:file_dialog.file.compress", FileDialogIcon.compressedExtension);
        public static ExtensionFilter compressFileFilter => _compressFileFilter;

        readonly static ExtensionFilter _codeFileFilter = new ExtensionFilter((NameSpacePathPair)"sc-krm:file_dialog.file.code", FileDialogIcon.codeExtension);
        public static ExtensionFilter codeFileFilter => _codeFileFilter;



        public NameSpacePathPair label { get; }
        public string[] extensions { get; }

        public ExtensionFilter(params string[] extensions)
        {
            label = new NameSpacePathPair("");
            this.extensions = extensions;
        }

        public ExtensionFilter(NameSpacePathPair label, params string[] extensions)
        {
            this.label = label;
            this.extensions = extensions;
        }

        public string[] ToSearchPatterns()
        {
            string[] searchPatterns = new string[extensions.Length];
            for (int i = 0; i < extensions.Length; i++)
                searchPatterns[i] = "*." + extensions[i];

            return searchPatterns;
        }
    }
}
