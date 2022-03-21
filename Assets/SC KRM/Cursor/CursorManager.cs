using SCKRM.Input;
using SCKRM.Tool;
using SCKRM.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM
{
    public class CursorManager : ManagerUI<CursorManager>
    {
        public static bool visible { get; set; } = true;

        protected override void OnEnable() => SingletonCheck(this);

        Vector2 dragStartMousePosition;
        void LateUpdate()
        {
            if (Kernel.isInitialLoadEnd)
            {
                if (graphic.enabled != visible)
                {
                    graphic.enabled = visible;
                    transform.position = Vector3.zero;
                }

                if (InputManager.GetMouseButton(0, InputType.Down, "all"))
                    dragStartMousePosition = InputManager.mousePosition;
                else if (InputManager.GetMouseButton(0, InputType.Alway, "all"))
                {
                    graphic.color = graphic.color.Lerp(Kernel.SaveData.systemColor * new Color(1, 1, 1, 0.5f), 0.2f * Kernel.fpsUnscaledDeltaTime);
                    transform.localScale = transform.localScale.Lerp(Vector3.one * 0.2f, 0.075f * Kernel.fpsUnscaledDeltaTime);

                    if (dragStartMousePosition != (Vector2)transform.position)
                    {
                        Vector3 dir = (Vector2)transform.position - dragStartMousePosition;
                        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle + 67.5f)), 0.2f * Kernel.fpsUnscaledDeltaTime);
                    }
                    else
                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector3.zero), 0.2f * Kernel.fpsUnscaledDeltaTime);
                }
                else
                {
                    graphic.color = graphic.color.Lerp(new Color(0, 0, 0, 0.5f), 0.2f * Kernel.fpsUnscaledDeltaTime);
                    transform.localScale = transform.localScale.Lerp(Vector3.one * 0.25f, 0.3f * Kernel.fpsUnscaledDeltaTime);

                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector3.zero), 0.2f * Kernel.fpsUnscaledDeltaTime);
                }

                transform.position = InputManager.mousePosition;
            }
        }
    }
}
