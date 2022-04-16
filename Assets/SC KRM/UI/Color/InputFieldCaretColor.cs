using SCKRM.Tool;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.UI
{
    [ExecuteAlways]
    public sealed class InputFieldCaretColor : UI
    {
        [SerializeField, NotNull] TMP_InputField _inputField; public TMP_InputField inputField => _inputField;

        protected override void OnEnable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif

            inputField.caretColor = defaultCaretColor;
            RhythmManager.oneBeat += ColorChange;
        }

        protected override void OnDisable() => RhythmManager.oneBeat -= ColorChange;

        protected override void Awake()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif

            if (inputField == null)
                return;

            defaultCaretBlinkRate = inputField.caretBlinkRate;
            defaultCaretColor = inputField.caretColor;
        }

        float defaultCaretBlinkRate = 0;
        Color defaultCaretColor = Color.white;
        void Update()
        {
            if (inputField == null)
                return;

            inputField.customCaretColor = true;

#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif

            if (RhythmManager.isBeatPlay)
            {
                inputField.caretBlinkRate = 0;
                inputField.caretColor = new Color(inputField.caretColor.r, inputField.caretColor.g, inputField.caretColor.b, inputField.caretColor.a - 0.0125f * RhythmManager.bpmFpsDeltaTime);
            }
            else
            {
                inputField.caretBlinkRate = defaultCaretBlinkRate;
                inputField.caretColor = inputField.caretColor.MoveTowards(defaultCaretColor, 0.025f * Kernel.fpsUnscaledDeltaTime);
            }
        }

        void ColorChange() => inputField.caretColor = defaultCaretColor;
    }
}
