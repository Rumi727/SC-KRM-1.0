using SCKRM.Cursor;
using SCKRM.Input;
using SCKRM.Resource;
using SCKRM.UI;
using TMPro;
using UnityEngine;

namespace SCKRM.Tooltip
{
    [AddComponentMenu("SC KRM/Tooltip/Tooltip Manager", 0)]
    public sealed class TooltipManager : Manager<TooltipManager>
    {
        public static bool isShow { get; private set; } = false;



        [SerializeField, NotNull] RectTransform toolTip;
        [SerializeField, NotNull] TargetSizeFitter toolTipTargetSizeFitter;
        [SerializeField, NotNull] CanvasGroup toolTipCanvasGroup;
        [SerializeField, NotNull] TMP_Text toolTipText;
        [SerializeField, NotNull] BetterContentSizeFitter toolTipTextBetterContentSizeFitter;



        void Awake() => SingletonCheck(this);

        void LateUpdate()
        {
            if (isShow)
                toolTipCanvasGroup.alpha += 0.1f * Kernel.unscaledDeltaTime;
            else
                toolTipCanvasGroup.alpha -= 0.1f * Kernel.unscaledDeltaTime;

            toolTipCanvasGroup.alpha = toolTipCanvasGroup.alpha.Clamp01();

            if (toolTipCanvasGroup.alpha > 0)
            {
                RectTransform cursorRectTransform = CursorManager.instance.rectTransform;
                Vector2 cursorScale = cursorRectTransform.localScale;
                Vector2 cursorSize = cursorRectTransform.rect.size;
                float cursorZRotationRad = cursorRectTransform.localEulerAngles.z * Mathf.Deg2Rad;
                float cursorZRotationSin = Mathf.Sin(cursorZRotationRad);
                float cursorZRotationCos = Mathf.Cos(cursorZRotationRad);

                Vector2 offset = new Vector2(cursorZRotationSin * cursorSize.x, cursorZRotationCos * -cursorSize.y) + new Vector2(cursorZRotationCos * cursorSize.x, cursorZRotationSin * cursorSize.x);
                Vector2 pos = (InputManager.mousePosition / UIManager.currentGuiSize) + (offset * cursorScale);
                pos.x = pos.x.Clamp(0, ScreenManager.width - toolTip.rect.size.x);
                pos.y = pos.y.Clamp(toolTip.rect.size.y, ScreenManager.height);

                toolTip.anchoredPosition = pos;
                toolTipTextBetterContentSizeFitter.max = new Vector2(ScreenManager.width, ScreenManager.height) - toolTipTargetSizeFitter.offset;
            }
        }

        public static void Show(string text, string nameSpace = "")
        {
            instance.toolTipText.text = ResourceManager.SearchLanguage(text, nameSpace);
            if (instance.toolTipText.text == "")
                instance.toolTipText.text = text;
            if (instance.toolTipText.text == "")
                return;

            if (instance.toolTipCanvasGroup.alpha <= 0)
            {
                instance.toolTipTextBetterContentSizeFitter.SetLayoutHorizontal();
                instance.toolTipTextBetterContentSizeFitter.SetLayoutVertical();

                instance.toolTipTargetSizeFitter.LayoutRefresh();
                instance.toolTipTargetSizeFitter.SizeUpdate(false);
            }

            isShow = true;
        }

        public static void Hide() => isShow = false;
    }
}
