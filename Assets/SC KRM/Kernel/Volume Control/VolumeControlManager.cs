using SCKRM.Input;
using SCKRM.Tool;
using SCKRM.UI.StatusBar;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SCKRM.UI
{
    public sealed class VolumeControlManager : ManagerUI<VolumeControlManager>, IPointerEnterHandler, IPointerExitHandler
    {
        bool isPointer;
        float timer = 0;
        [SerializeField] GameObject hide;

        void OnEnable() => SingletonCheck(this);

        void Update()
        {
            if (Kernel.isInitialLoadEnd)
            {
                if (isPointer)
                    timer = 1;
                else
                    timer -= Kernel.unscaledDeltaTime;

                RectTransform statusBar = StatusBarManager.instance.rectTransform;
                if (isPointer || timer >= 0)
                {
                    if (!hide.activeSelf)
                        hide.SetActive(true);

                    if (StatusBarManager.SaveData.bottomMode)
                        rectTransform.anchoredPosition = rectTransform.anchoredPosition.Lerp(new Vector2(rectTransform.anchoredPosition.x, -15), 0.2f * Kernel.fpsUnscaledDeltaTime);
                    else
                        rectTransform.anchoredPosition = rectTransform.anchoredPosition.Lerp(new Vector2(rectTransform.anchoredPosition.x, -(statusBar.anchoredPosition.y + statusBar.sizeDelta.y) - 15), 0.2f * Kernel.fpsUnscaledDeltaTime);
                }
                else
                {
                    rectTransform.anchoredPosition = rectTransform.anchoredPosition.Lerp(new Vector2(rectTransform.anchoredPosition.x, rectTransform.sizeDelta.y + 1), 0.2f * Kernel.fpsUnscaledDeltaTime);

                    if (hide.activeSelf && rectTransform.anchoredPosition.y >= rectTransform.sizeDelta.y - 0.01f)
                        hide.SetActive(false);
                }

                if (InputManager.GetKey("volume_control.minus", InputType.Down, "all"))
                {
                    if (isPointer || timer >= 0)
                        Kernel.SaveData.mainVolume -= 10;

                    timer = 1;
                }
                else if (InputManager.GetKey("volume_control.plus", InputType.Down, "all"))
                {
                    if (isPointer || timer >= 0)
                        Kernel.SaveData.mainVolume += 10;

                    timer = 1;
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData) => isPointer = true;

        public void OnPointerExit(PointerEventData eventData) => isPointer = false;
    }
}
