using SCKRM.Language;
using SCKRM.Object;
using SCKRM.Threads;
using SCKRM.Tool;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI.SideBar
{
    [AddComponentMenu("")]
    [RequireComponent(typeof(RectTransform))]
    public sealed class RunningTaskInfo : ObjectPooling
    {
        [SerializeField, HideInInspector] RectTransform _rectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                    _rectTransform = GetComponent<RectTransform>();

                return _rectTransform;
            }
        }



        [SerializeField] TMP_Text _nameText;
        public TMP_Text nameText => _nameText;

        [SerializeField] TMP_Text _infoText;
        public TMP_Text infoText => _infoText;

        [SerializeField] Slider _slider;
        public Slider slider => _slider;

        [SerializeField] RectTransform _fillShow;
        public RectTransform fillShow => _fillShow;

        public int index { get; set; }

        public override void OnCreate()
        {
            LanguageManager.currentLanguageChange += InfoLoad;
            ThreadManager.threadChange += ThreadChange;
        }

        bool infoLoad = false;
        void ThreadChange() => infoLoad = true;

        public void InfoLoad()
        {
            if (index >= 0 && index < ThreadManager.runningThreads.Count)
            {
                ThreadMetaData threadMetaData = ThreadManager.runningThreads[index];
                nameText.text = LanguageManager.TextLoad(threadMetaData.name);
                infoText.text = LanguageManager.TextLoad(threadMetaData.info);
            }
        }

        [System.NonSerialized] bool noResponse = false;

        [System.NonSerialized] float loopValue = 0;
        [System.NonSerialized] float tempProgress = 0;
        [System.NonSerialized] float tempTimer = 0;
        [System.NonSerialized] float tempMinX = 0;
        [System.NonSerialized] float tempMaxX = 0;
        void Update()
        {
            if (index >= 0 && index < ThreadManager.runningThreads.Count)
            {
                if (infoLoad)
                {
                    InfoLoad();
                    infoLoad = false;
                }

                ThreadMetaData threadMetaData = ThreadManager.runningThreads[index];
                if (!threadMetaData.loop)
                {
                    if (!slider.gameObject.activeSelf)
                        slider.gameObject.SetActive(true);

                    if (tempTimer >= 1)
                    {
                        if (slider.enabled)
                            slider.enabled = false;

                        if (!noResponse)
                        {
                            tempMinX = fillShow.anchorMin.x - (loopValue - 0.25f).Clamp01();
                            tempMaxX = fillShow.anchorMax.x - loopValue.Clamp01();

                            noResponse = true;
                        }

                        loopValue += 0.0125f * Kernel.fpsDeltaTime;

                        tempMinX = tempMinX.Lerp(0, 0.2f * Kernel.fpsDeltaTime);
                        tempMaxX = tempMaxX.Lerp(0, 0.2f * Kernel.fpsDeltaTime);

                        fillShow.anchorMin = new Vector2((loopValue - 0.25f).Clamp01() + tempMinX, fillShow.anchorMin.y);
                        fillShow.anchorMax = new Vector2(loopValue.Clamp01() + tempMaxX, fillShow.anchorMax.y);

                        if (fillShow.anchorMin.x >= 1)
                            loopValue = 0;
                    }
                    else
                    {
                        if (!slider.enabled)
                            slider.enabled = true;

                        noResponse = false;

                        slider.value = threadMetaData.progress;
                        fillShow.anchorMin = fillShow.anchorMin.Lerp(slider.fillRect.anchorMin, 0.2f * Kernel.fpsDeltaTime);
                        fillShow.anchorMax = fillShow.anchorMax.Lerp(slider.fillRect.anchorMax, 0.2f * Kernel.fpsDeltaTime);

                        tempTimer += Kernel.deltaTime;
                    }

                    if (tempProgress != threadMetaData.progress)
                    {
                        tempTimer = 0;
                        tempProgress = threadMetaData.progress;
                    }
                }
                else
                {
                    if (slider.gameObject.activeSelf)
                        slider.gameObject.SetActive(false);

                    if (!slider.enabled)
                        slider.enabled = true;

                    loopValue = 0;
                    noResponse = false;
                }
            }
        }

        public override void Remove()
        {
            base.Remove();

            rectTransform.sizeDelta = new Vector2(430, 19);
            index = 0;
            loopValue = 0;
            noResponse = false;
            tempProgress = 0;
            tempTimer = 0;
            tempMinX = 0;
            tempMaxX = 0;
            nameText.text = "";
            infoText.text = "";
            slider.value = 0;
            fillShow.anchorMin = new Vector2(0, slider.fillRect.anchorMin.y);
            fillShow.anchorMax = new Vector2(0, slider.fillRect.anchorMax.y);

            slider.gameObject.SetActive(true);
            slider.enabled = true;

            LanguageManager.currentLanguageChange -= InfoLoad;
            ThreadManager.threadChange -= InfoLoad;
        }
    }
}