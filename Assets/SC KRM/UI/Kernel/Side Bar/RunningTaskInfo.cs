using SCKRM.Language;
using SCKRM.Object;
using SCKRM.Resource;
using SCKRM.Threads;
using SCKRM.Tool;
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
            LanguageManager.currentLanguageChange += InfoLoad;
            ThreadManager.threadChange += InfoLoad;
        }

        public void InfoLoad()
        {
            if (asyncTask != null)
            {
                nameText.text = ResourceManager.SearchLanguage(asyncTask.name);
                infoText.text = ResourceManager.SearchLanguage(asyncTask.info);
            }
        }

        [System.NonSerialized] bool noResponse = false;

        [System.NonSerialized] float loopValue = 0;
        [System.NonSerialized] float tempProgress = 0;
        [System.NonSerialized] float tempTimer = 0;
        [System.NonSerialized] float tempMinX = 0;
        [System.NonSerialized] float tempMaxX = 0;
        [System.NonSerialized] bool pointer = false;
        void Update()
        {
            if (pointer || removeButtonCanvasGroup.gameObject == EventSystem.current.currentSelectedGameObject)
                removeButtonCanvasGroup.alpha = removeButtonCanvasGroup.alpha.MoveTowards(1, 0.2f * Kernel.fpsDeltaTime);
            else
                removeButtonCanvasGroup.alpha = removeButtonCanvasGroup.alpha.MoveTowards(0, 0.2f * Kernel.fpsDeltaTime);

            if (asyncTask == null || asyncTaskIndex >= AsyncTaskManager.asyncTasks.Count)
            {
                Remove();
                return;
            }

            if (!asyncTask.loop)
            {
                if (!progressBar.gameObject.activeSelf)
                    progressBar.gameObject.SetActive(true);
            }
            else
            {
                if (progressBar.gameObject.activeSelf)
                    progressBar.gameObject.SetActive(false);

                loopValue = 0;
                noResponse = false;
            }
        }

        public void Cancel() => asyncTask.Remove();

        public override void Remove()
        {
            base.Remove();

            rectTransform.sizeDelta = new Vector2(430, 19);
            asyncTask = null;
            loopValue = 0;
            noResponse = false;
            tempProgress = 0;
            tempTimer = 0;
            tempMinX = 0;
            tempMaxX = 0;
            nameText.text = "";
            infoText.text = "";
            progressBar.slider.value = 0;
            progressBar.fillShow.anchorMin = new Vector2(0, progressBar.slider.fillRect.anchorMin.y);
            progressBar.fillShow.anchorMax = new Vector2(0, progressBar.slider.fillRect.anchorMax.y);

            progressBar.gameObject.SetActive(true);
            progressBar.enabled = true;

            LanguageManager.currentLanguageChange -= InfoLoad;
            ThreadManager.threadChange -= InfoLoad;
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) => pointer = true;

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData) => pointer = false;
    }
}