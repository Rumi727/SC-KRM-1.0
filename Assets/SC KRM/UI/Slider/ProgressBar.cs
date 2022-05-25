using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI
{
    public class ProgressBar : UIAni
    {
        [SerializeField, Min(0)] float _progress; public float progress { get => _progress; set => _progress = value.Clamp(0); }
        [SerializeField, Min(0)] float _maxProgress; public float maxProgress { get => _maxProgress; set => _maxProgress = value.Clamp(0); }



        [SerializeField, NotNull] Slider _slider;
        public Slider slider => _slider;

        [SerializeField, NotNull] RectTransform _fillShow;
        public RectTransform fillShow => _fillShow;


        public bool allowNoResponse { get; set; } = true;
        public bool isNoResponse { get; private set; } = false;

        [System.NonSerialized] float loopValue = 0;
        [System.NonSerialized] float tempProgress = 0;
        [System.NonSerialized] float tempTimer = 0;
        [System.NonSerialized] float tempMinX = 0;
        [System.NonSerialized] float tempMaxX = 0;

        DrivenRectTransformTracker tracker;

        protected override void OnDisable() => tracker.Clear();

        void Update()
        {
            tracker.Clear();
            tracker.Add(this, fillShow, DrivenTransformProperties.Anchors);

            if (tempTimer >= 1 && allowNoResponse && progress < maxProgress)
            {
                if (slider.enabled)
                    slider.enabled = false;

                if (!isNoResponse)
                {
                    tempMinX = fillShow.anchorMin.x - (loopValue - 0.25f).Clamp01();
                    tempMaxX = fillShow.anchorMax.x - loopValue.Clamp01();

                    isNoResponse = true;
                }

                loopValue += 0.0125f * Kernel.fpsUnscaledDeltaTime;

                tempMinX.LerpRef(0, 0.2f * Kernel.fpsUnscaledDeltaTime);
                tempMaxX.LerpRef(0, 0.2f * Kernel.fpsUnscaledDeltaTime);

                fillShow.anchorMin = new Vector2((loopValue - 0.25f + tempMinX).Clamp01(), fillShow.anchorMin.y);
                fillShow.anchorMax = new Vector2((loopValue + tempMaxX).Clamp01(), fillShow.anchorMax.y);

                if (fillShow.anchorMin.x >= 1)
                    loopValue = 0;
            }
            else
            {
                if (!slider.enabled)
                    slider.enabled = true;

                isNoResponse = false;

                slider.value = progress;
                slider.maxValue = maxProgress;

                fillShow.anchorMin = fillShow.anchorMin.Lerp(slider.fillRect.anchorMin, 0.2f * Kernel.fpsUnscaledDeltaTime);
                fillShow.anchorMax = fillShow.anchorMax.Lerp(slider.fillRect.anchorMax, 0.2f * Kernel.fpsUnscaledDeltaTime);

                tempTimer += Kernel.unscaledDeltaTime;
            }

            if (tempProgress != progress)
            {
                tempTimer = 0;
                tempProgress = progress;
            }
        }

        public void Initialize()
        {
            loopValue = 0;
            isNoResponse = false;
            tempProgress = 0;
            tempTimer = 0;
            tempMinX = 0;
            tempMaxX = 0;

            slider.value = 0;
            fillShow.anchorMin = new Vector2(0, slider.fillRect.anchorMin.y);
            fillShow.anchorMax = new Vector2(0, slider.fillRect.anchorMax.y);
        }
    }
}
