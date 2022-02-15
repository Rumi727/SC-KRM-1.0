using SCKRM.Language;
using SCKRM.Object;
using SCKRM.Resource;
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
        [SerializeField] TMP_Text _nameText;
        public TMP_Text nameText => _nameText;

        [SerializeField] TMP_Text _infoText;
        public TMP_Text infoText => _infoText;

        [SerializeField] Slider _slider;
        public Slider slider => _slider;

        [SerializeField] RectTransform _fillShow;
        public RectTransform fillShow => _fillShow;

        public ThreadMetaData threadMetaData { get; set; }

        public override void OnCreate()
        {
            LanguageManager.currentLanguageChange += InfoLoad;
            ThreadManager.threadChange += InfoLoad;
        }

        public void InfoLoad()
        {
            nameText.text = ResourceManager.SearchLanguage(threadMetaData.name);
            infoText.text = ResourceManager.SearchLanguage(threadMetaData.info);
        }

        [System.NonSerialized] bool noResponse = false;

        [System.NonSerialized] float loopValue = 0;
        [System.NonSerialized] float tempProgress = 0;
        [System.NonSerialized] float tempTimer = 0;
        [System.NonSerialized] float tempMinX = 0;
        [System.NonSerialized] float tempMaxX = 0;
        void Update()
        {
            if (threadMetaData.thread == null && !threadMetaData.autoRemoveDisable)
            {
                Remove();
                return;
            }

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

                    loopValue += 0.0125f * Kernel.fpsUnscaledDeltaTime;

                    tempMinX = tempMinX.Lerp(0, 0.2f * Kernel.fpsUnscaledDeltaTime);
                    tempMaxX = tempMaxX.Lerp(0, 0.2f * Kernel.fpsUnscaledDeltaTime);

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
                    fillShow.anchorMin = fillShow.anchorMin.Lerp(slider.fillRect.anchorMin, 0.2f * Kernel.fpsUnscaledDeltaTime);
                    fillShow.anchorMax = fillShow.anchorMax.Lerp(slider.fillRect.anchorMax, 0.2f * Kernel.fpsUnscaledDeltaTime);

                    tempTimer += Kernel.unscaledDeltaTime;
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

        public override void Remove()
        {
            base.Remove();

            rectTransform.sizeDelta = new Vector2(430, 19);
            threadMetaData = null;
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