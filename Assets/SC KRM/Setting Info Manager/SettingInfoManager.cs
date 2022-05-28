using SCKRM.Input;
using SCKRM.Renderer;
using SCKRM.Resource;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SCKRM.UI
{
    public class SettingInfoManager : Manager<SettingInfoManager>
    {
        public static bool isShow { get; private set; } = false;



        [SerializeField, NotNull] CanvasGroup settingInfo;
        [SerializeField, NotNull] SetSizeAsTargetRectTransform settingInfoSetSizeAsTargetRectTransformX;
        [SerializeField, NotNull] SetSizeAsTargetRectTransform settingInfoSetSizeAsTargetRectTransformY;
        [SerializeField, NotNull] TMP_Text labelText;
        [SerializeField, NotNull] TMP_Text valueText;
        [SerializeField, NotNull] TMP_Text shortcutKeysText;
        [SerializeField, NotNull] BetterContentSizeFitter labelTextBetterContentSizeFitter;
        [SerializeField, NotNull] BetterContentSizeFitter valueTextBetterContentSizeFitter;
        [SerializeField, NotNull] BetterContentSizeFitter shortcutKeysTextBetterContentSizeFitter;

        void Awake() => SingletonCheck(this);

        static float showTimer = 0;
        void LateUpdate()
        {
            if (isShow)
            {
                settingInfo.alpha += 0.1f * Kernel.fpsDeltaTime;

                showTimer += Time.deltaTime;
                if (showTimer >= 1.5f)
                    Hide();
            }
            else
                settingInfo.alpha -= 0.1f * Kernel.fpsDeltaTime;

            settingInfo.alpha = settingInfo.alpha.Clamp01();

            if (settingInfo.alpha > 0)
            {
                {
                    float labelTextX = labelText.rectTransform.rect.size.x;
                    float valueTextX = valueText.rectTransform.rect.size.x;
                    float shortcutKeysTextX = shortcutKeysText.rectTransform.rect.size.x;
                    float max = Max(labelTextX, valueTextX, shortcutKeysTextX);
                    if (max == labelTextX)
                        settingInfoSetSizeAsTargetRectTransformX.targetRectTransform = labelText.rectTransform;
                    else if (max == valueTextX)
                        settingInfoSetSizeAsTargetRectTransformX.targetRectTransform = valueText.rectTransform;
                    else if (max == shortcutKeysTextX)
                        settingInfoSetSizeAsTargetRectTransformX.targetRectTransform = shortcutKeysText.rectTransform;

                    static float Max(params float[] values) => values.Max();
                }

                {
                    Vector2 max = new Vector2(Screen.width - settingInfoSetSizeAsTargetRectTransformX.offset.x, Screen.height - settingInfoSetSizeAsTargetRectTransformY.offset.y);
                    labelTextBetterContentSizeFitter.max = max;
                    valueTextBetterContentSizeFitter.max = max;
                    shortcutKeysTextBetterContentSizeFitter.max = max;
                }
            }
        }

        public static void Show(NameSpacePathPair label, NameSpacePathPair value, params string[] hotkeys)
        {
            int space = 0;
            SetText(ResourceManager.SearchLanguage(label.path, label.nameSpace), instance.labelText, label);
            SetText(ResourceManager.SearchLanguage(value.path, value.nameSpace), instance.valueText, value);
            SetText(KeyCodeToString(), instance.shortcutKeysText, "");

            void SetText(string text, TMP_Text tmp, string defaultText)
            {
                tmp.text = text;
                if (tmp.text == "")
                    tmp.text = defaultText;
                if (tmp.text == "")
                    space++;
            }

            string KeyCodeToString()
            {
                if (hotkeys != null)
                {
                    string text = "";
                    bool withHotKeys = false;
                    for (int i = 0; i < hotkeys.Length; i++)
                    {
                        KeyCode[] keyCodes = InputManager.controlSettingList[hotkeys[i]].ToArray();
                        if (keyCodes.Length <= 0)
                            continue;

                        for (int j = 0; j < keyCodes.Length; j++)
                        {
                            KeyCode keyCode = keyCodes[j];
                            if (keyCode == KeyCode.None)
                                continue;

                            string hotkey = keyCode.KeyCodeToString();
                            if (j < keyCodes.Length - 1)
                                text += hotkey + " + ";
                            else
                                text += hotkey;

                            withHotKeys = true;
                        }

                        if (i < hotkeys.Length - 1)
                            text += " | ";
                    }

                    if (withHotKeys)
                        return text;
                }

                return ResourceManager.SearchLanguage("setting_info.no_hotkey", "sc-krm");
            }

            if (space >= 3)
                return;

            isShow = true;
            showTimer = 0;

            if (instance.settingInfo.alpha <= 0)
            {
                SetContentSize(instance.labelTextBetterContentSizeFitter);
                SetContentSize(instance.valueTextBetterContentSizeFitter);
                SetContentSize(instance.shortcutKeysTextBetterContentSizeFitter);

                void SetContentSize(BetterContentSizeFitter betterContentSizeFitter)
                {
                    betterContentSizeFitter.SetLayoutHorizontal();
                    betterContentSizeFitter.SetLayoutVertical();
                }

                instance.LateUpdate();

                SetSize(instance.settingInfoSetSizeAsTargetRectTransformX);
                SetSize(instance.settingInfoSetSizeAsTargetRectTransformY);

                void SetSize(SetSizeAsTargetRectTransform setSizeAsTargetRectTransform)
                {
                    setSizeAsTargetRectTransform.LayoutRefresh();
                    setSizeAsTargetRectTransform.SizeUpdate(false);
                }
            }
        }

        public static void Hide() => isShow = false;
    }
}
