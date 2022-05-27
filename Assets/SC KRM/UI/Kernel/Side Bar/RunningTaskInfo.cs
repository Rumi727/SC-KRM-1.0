using SCKRM.Language;
using SCKRM.Object;
using SCKRM.Resource;
using SCKRM.Threads;
using SCKRM.UI.StatusBar;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SCKRM.UI.SideBar
{
    [AddComponentMenu("")]
    [RequireComponent(typeof(RectTransform))]
    public sealed class RunningTaskInfo : ObjectPoolingUI, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] TMP_Text _nameText;
        public TMP_Text nameText => _nameText;

        [SerializeField] TMP_Text _infoText;
        public TMP_Text infoText => _infoText;

        [SerializeField] ProgressBar _progressBar;
        public ProgressBar progressBar => _progressBar;



        [SerializeField] CanvasGroup _removeButtonCanvasGroup;
        public CanvasGroup removeButtonCanvasGroup => _removeButtonCanvasGroup;



        public AsyncTask asyncTask { get; set; }
        public int asyncTaskIndex { get; set; }

        public override void OnCreate()
        {
            base.OnCreate();

            LanguageManager.currentLanguageChange += InfoLoad;
            ThreadManager.threadChange += InfoLoad;
        }

        public void InfoLoad()
        {
            if (asyncTask != null)
            {
                nameText.text = ResourceManager.SearchLanguage(asyncTask.name);
                infoText.text = ResourceManager.SearchLanguage(asyncTask.info);

                if (string.IsNullOrEmpty(nameText.text))
                    nameText.text = asyncTask.name;
                if (string.IsNullOrEmpty(infoText.text))
                    infoText.text = asyncTask.info;
            }
        }

        [System.NonSerialized] string tempName = "";
        [System.NonSerialized] string tempInfo = "";
        [System.NonSerialized] bool pointer = false;
        void Update()
        {
            if (asyncTask == null || asyncTaskIndex >= AsyncTaskManager.asyncTasks.Count)
            {
                progressBar.allowNoResponse = false;
                progressBar.progress = 1;
                progressBar.maxProgress = 1;

                if (progressBar.fillShow.anchorMax.x >= 0.99f)
                    Remove();

                return;
            }

            if (tempName != asyncTask.name || tempInfo != asyncTask.info)
                InfoLoad();

            if (!asyncTask.cantCancel)
            {
                if (pointer || removeButtonCanvasGroup.gameObject == EventSystem.current.currentSelectedGameObject)
                    removeButtonCanvasGroup.alpha = removeButtonCanvasGroup.alpha.MoveTowards(1, 0.2f * Kernel.fpsDeltaTime);
                else
                    removeButtonCanvasGroup.alpha = removeButtonCanvasGroup.alpha.MoveTowards(0, 0.2f * Kernel.fpsDeltaTime);
            }
            else
            {
                removeButtonCanvasGroup.alpha = 0;
                removeButtonCanvasGroup.interactable = false;
            }

            if (!asyncTask.loop)
            {
                if (!progressBar.gameObject.activeSelf)
                    progressBar.gameObject.SetActive(true);

                progressBar.progress = asyncTask.progress;
                progressBar.maxProgress = asyncTask.maxProgress;
            }
            else if (progressBar.gameObject.activeSelf)
                progressBar.gameObject.SetActive(false);
        }

        public void Cancel() => asyncTask.Remove();

        public override void Remove()
        {
            base.Remove();

            rectTransform.sizeDelta = new Vector2(430, 19);
            asyncTask = null;
            nameText.text = "";
            infoText.text = "";

            progressBar.Initialize();

            progressBar.gameObject.SetActive(true);
            progressBar.enabled = true;

            LanguageManager.currentLanguageChange -= InfoLoad;
            ThreadManager.threadChange -= InfoLoad;
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) => pointer = true;

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData) => pointer = false;
    }
}