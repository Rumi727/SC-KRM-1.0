using SCKRM.Input;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SCKRM.UI
{
    [ExecuteAlways]
    public sealed class VolumeControl : UIAni, IPointerDownHandler, IEndDragHandler
    {
        enum Type
        {
            main,
            bgm,
            sound
        }

        [SerializeField] Type type;
        [SerializeField] RectTransform sliderRectTransform;
        [SerializeField] Slider slider;
        [SerializeField] TMP_Text nameText;
        [SerializeField] TMP_Text valueText;
        bool isDrag = false;

        void Update()
        {
            sliderRectTransform.offsetMin = new Vector2(nameText.rectTransform.sizeDelta.x + 6, sliderRectTransform.offsetMin.y);
            sliderRectTransform.offsetMax = new Vector2(-valueText.rectTransform.sizeDelta.x - 6, sliderRectTransform.offsetMax.y);

            if (!Application.isPlaying)
                return;

            if (!isDrag)
            {
                if (lerp)
                {
                    switch (type)
                    {
                        case Type.main:
                            slider.value = slider.value.Lerp(Kernel.SaveData.mainVolume, lerpValue * Kernel.fpsUnscaledDeltaTime);
                            break;
                        case Type.bgm:
                            slider.value = slider.value.Lerp(Kernel.SaveData.bgmVolume, lerpValue * Kernel.fpsUnscaledDeltaTime);
                            break;
                        case Type.sound:
                            slider.value = slider.value.Lerp(Kernel.SaveData.soundVolume, lerpValue * Kernel.fpsUnscaledDeltaTime);
                            break;
                    }
                }
                else
                {
                    switch (type)
                    {
                        case Type.main:
                            slider.value = Kernel.SaveData.mainVolume;
                            break;
                        case Type.bgm:
                            slider.value = Kernel.SaveData.bgmVolume;
                            break;
                        case Type.sound:
                            slider.value = Kernel.SaveData.soundVolume;
                            break;
                    }
                }
            }

            switch (type)
            {
                case Type.main:
                    valueText.text = Kernel.SaveData.mainVolume.ToString();
                    break;
                case Type.bgm:
                    valueText.text = Kernel.SaveData.bgmVolume.ToString();
                    break;
                case Type.sound:
                    valueText.text = Kernel.SaveData.soundVolume.ToString();
                    break;
            }
        }

        public void OnValueChanged()
        {
            if (isDrag)
            {
                switch (type)
                {
                    case Type.main:
                        Kernel.SaveData.mainVolume = (int)slider.value;
                        break;
                    case Type.bgm:
                        Kernel.SaveData.bgmVolume = (int)slider.value;
                        break;
                    case Type.sound:
                        Kernel.SaveData.soundVolume = (int)slider.value;
                        break;
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isDrag = true;
            VolumeControlManager.OnBeginDrag();
            InputManager.SetInputLock("volumecontrol", true);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDrag = false;
            VolumeControlManager.OnEndDrag();
            InputManager.SetInputLock("volumecontrol", false);
        }
    }
}
