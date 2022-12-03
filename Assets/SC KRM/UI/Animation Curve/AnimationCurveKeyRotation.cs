using SCKRM.Input;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

namespace SCKRM.UI
{
    public sealed class AnimationCurveKeyRotation : UI, IDragHandler
    {
        [SerializeField] float handleOffset = 50;
        [SerializeField] AnimationCurveKey animationCurveKey;
        [SerializeField] AnimationCurveKeyInOut inOut;
        [SerializeField] UILineRenderer lineRenderer;

        AnimationCurveWindow animationCurveWindow => animationCurveKey.animationCurveWindow;
        AnimationCurve animationCurve => animationCurveWindow.curve;
        int index => animationCurveKey.index;

        void Update()
        {
            Keyframe keyframe = animationCurve[index];

            if (inOut == AnimationCurveKeyInOut.inTangent)
            {
                float aTan = keyframe.inTangent.Atan();
                rectTransform.anchoredPosition = -new Vector2(aTan.Cos(), aTan.Sin()) * handleOffset;
            }
            else
            {
                float aTan = keyframe.outTangent.Atan();
                rectTransform.anchoredPosition = new Vector2(aTan.Cos(), aTan.Sin()) * handleOffset;
            }

            lineRenderer.Points[1] = -rectTransform.anchoredPosition;
            lineRenderer.SetAllDirty();
        }

        public void OnDrag(PointerEventData eventData)
        {
            Keyframe keyframe = animationCurve[index];

            UnityEngine.Camera camera = null;
            if (canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                camera = canvas.worldCamera;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(animationCurveKey.rectTransform, InputManager.mousePosition, camera, out Vector2 pointerPos);
            Vector2 pos = animationCurveKey.rectTransform.anchoredPosition - pointerPos;

            //180도 제한
            if (pos.x < 0 && inOut == AnimationCurveKeyInOut.inTangent)
            {
                if (pos.y < 0)
                    SetTangent(ref keyframe, float.NegativeInfinity);
                else
                    SetTangent(ref keyframe, float.PositiveInfinity);
            }
            else if (pos.x > 0 && inOut == AnimationCurveKeyInOut.outTangent)
            {
                if (pos.y < 0)
                    SetTangent(ref keyframe, float.PositiveInfinity);
                else
                    SetTangent(ref keyframe, float.NegativeInfinity);
            }
            else
            {
                float tangent = pos.y / pos.x;
                if (!float.IsNormal(tangent))
                    tangent = 0;

                SetTangent(ref keyframe, tangent);
            }
        }

        void SetTangent(ref Keyframe keyframe, float tangent)
        {
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
            if (keyframe.tangentMode != 0) //수평 자유 모드가 아닐경우
            {
                AnimationUtility.TangentMode tangentMode = GetTangentMode();

                if (tangentMode == AnimationUtility.TangentMode.Auto || tangentMode == AnimationUtility.TangentMode.ClampedAuto)
                {
                    //오토 모드 일 경우 수평 자유 모드로 바꿔줘야합니다
                    keyframe.tangentMode = 0;

                    keyframe.inTangent = tangent;
                    keyframe.outTangent = tangent;

                    animationCurve.MoveKey(index, keyframe);
                }
                else
                {
                    if (inOut == AnimationCurveKeyInOut.inTangent)
                        keyframe.inTangent = tangent;
                    else
                        keyframe.outTangent = tangent;

                    animationCurve.MoveKey(index, keyframe);
                    SetTangentMode(AnimationUtility.TangentMode.Free); //오토 모드가 아니지만, 또 변경할 탄젠트가 자유 모드가 아닌 경우는 자유 모드로 바꿔줘야합니다
                }
            }
            else //수평 자유 모드일 경우 양쪽 다 바꿔줍니다
            {
                keyframe.inTangent = tangent;
                keyframe.outTangent = tangent;

                animationCurve.MoveKey(index, keyframe);
            }

            animationCurveWindow.KeyRefresh();
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
        }

        AnimationUtility.TangentMode GetTangentMode()
        {
            if (inOut == AnimationCurveKeyInOut.inTangent)
                return AnimationUtility.GetKeyLeftTangentMode(animationCurve, index);
            else
                return AnimationUtility.GetKeyRightTangentMode(animationCurve, index);
        }

        void SetTangentMode(AnimationUtility.TangentMode tangentMode)
        {
            if (inOut == AnimationCurveKeyInOut.inTangent)
                AnimationUtility.SetKeyLeftTangentMode(animationCurve, index, tangentMode);
            else
                AnimationUtility.SetKeyRightTangentMode(animationCurve, index, tangentMode);
        }
    }

    public enum AnimationCurveKeyInOut
    {
        inTangent,
        outTangent
    }
}
