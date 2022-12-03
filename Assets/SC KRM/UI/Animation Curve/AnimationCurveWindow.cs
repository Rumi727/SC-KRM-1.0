using DanielLochner.Assets.SimpleZoom;
using SCKRM.Object;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace SCKRM.UI
{
    public sealed class AnimationCurveWindow : UI
    {
        public AnimationCurve curve
        {
            get => _curve;
            set => _curve = value;
        }
        [SerializeField] AnimationCurve _curve;



        public float lineSplit
        {
            get => _lineSplit;
            set
            {
                _lineSplit = value.Clamp(1);
                Render();
            }
        }
        [SerializeField, Min(1)] float _lineSplit;



        public UILineRendererList lineRenderer => _lineRenderer; [SerializeField] UILineRendererList _lineRenderer;
        public RectTransform linePivot => _linePivot; [SerializeField] RectTransform _linePivot;
        public SimpleZoom lineSimpleZoom => _lineSimpleZoom; [SerializeField] SimpleZoom _lineSimpleZoom;

        void Update() => Render();

        public void KeyRefresh()
        {
            for (int i = 0; i < curve.length; i++)
            {
                AnimationUtility.SetKeyLeftTangentMode(curve, i, AnimationUtility.GetKeyLeftTangentMode(curve, i));
                AnimationUtility.SetKeyRightTangentMode(curve, i, AnimationUtility.GetKeyRightTangentMode(curve, i));
            }
        }

        public void Render()
        {
            lineRenderer.ClearPoints();

            Keyframe[] curveKeyframes = curve.keys;
            if (curveKeyframes.Length <= 0)
            {
                lineRenderer.AddPoint(Vector2.zero);
                KeyObjectCreate(0);

                return;
            }



            KeyObjectCreate(curveKeyframes.Length);
            for (int i = 0; i < curveKeyframes.Length - 1; i++)
            {
                Keyframe keyframe = curveKeyframes[i];
                float time = keyframe.time;
                float nextTime = curveKeyframes[i + 1].time;
                float delta = (nextTime - time) * (1f / lineSplit);

                for (int j = 0; j < lineSplit; j++)
                {
                    float splitTime = time + (delta * j);
                    float value = curve.Evaluate(splitTime);

                    lineRenderer.AddPoint(GetLinePos(splitTime, value));
                }

                Vector2 pos = GetLinePos(time, keyframe.value);
                RectTransform keyRectTransform = animationCurveKeys[i].rectTransform;

                keyRectTransform.anchoredPosition = Vector2.zero;

                keyRectTransform.anchorMin = pos;
                keyRectTransform.anchorMax = pos;
            }

            {
                Keyframe keyframe = curveKeyframes[curveKeyframes.Length - 1];
                float endTime = keyframe.time;
                lineRenderer.AddPoint(GetLinePos(endTime, curve.Evaluate(endTime)));

                Vector2 pos = GetLinePos(endTime, keyframe.value);
                RectTransform keyRectTransform = animationCurveKeys[curveKeyframes.Length - 1].rectTransform;

                keyRectTransform.anchoredPosition = Vector2.zero;

                keyRectTransform.anchorMin = pos;
                keyRectTransform.anchorMax = pos;
            }

            lineRenderer.SetAllDirty();
        }

        int tempKeyCount = 0;
        List<AnimationCurveKey> animationCurveKeys = new List<AnimationCurveKey>();
        void KeyObjectCreate(int keyCount)
        {
            if (tempKeyCount != keyCount)
            {
                {
                    AnimationCurveKey[] animationCurveKeys = GetComponentsInChildren<AnimationCurveKey>();
                    for (int i = 0; i < animationCurveKeys.Length; i++)
                        animationCurveKeys[i].Remove();
                }

                animationCurveKeys.Clear();

                for (int i = 0; i < keyCount; i++)
                {
                    AnimationCurveKey animationCurveKey = (AnimationCurveKey)ObjectPoolingSystem.ObjectCreate("animation_curve_window.key", lineRenderer.transform).monoBehaviour;
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
                    animationCurveKey.animationCurveWindow = this;
                    animationCurveKey.index = i;
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

                    animationCurveKeys.Add(animationCurveKey);
                }

                tempKeyCount = keyCount;
            }
        }

        Vector2 GetLinePos(float time, float value) => new Vector2(time, value);
    }
}
