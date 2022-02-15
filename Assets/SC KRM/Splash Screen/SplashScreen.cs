using Cysharp.Threading.Tasks;
using SCKRM.Tool;
using SCKRM.Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.Splash
{
    [AddComponentMenu("커널/스플래시/스플래시 스크린")]
    public sealed class SplashScreen : MonoBehaviour
    {
        public static bool isAniPlayed { get; set; } = true;
        [SerializeField] Image LogoImage;
        [SerializeField] Transform CS;
        [SerializeField] Image CSImage;
        [SerializeField] Text text;
        [SerializeField] string showText = "";

        float xV;
        float yV;
        float rV;

        bool xFlip = false;
        bool aniEnd = false;

        float timer = 0;

        bool aniPlay = false;

        AudioClip bow;
        AudioClip drawmap;

        async UniTaskVoid Awake()
        {
            isAniPlayed = true;
            aniPlay = false;

            bow = await ResourceManager.GetAudio(PathTool.Combine(Kernel.streamingAssetsPath, ResourceManager.soundPath.Replace("%NameSpace%", "minecraft"), "random/bow"));
            drawmap = await ResourceManager.GetAudio(PathTool.Combine(Kernel.streamingAssetsPath, ResourceManager.soundPath.Replace("%NameSpace%", "minecraft"), "ui/cartography_table/drawmap") + Random.Range(1, 4));

            if (Random.Range(0, 2) == 1)
                xFlip = true;
            else
                xFlip = false;

            if (xFlip)
            {
                CS.localPosition = new Vector2(670, Random.Range(-32f, 245f));
                xV = Random.Range(-8f, -20f);
                rV = Random.Range(10f, 20f);
            }
            else
            {
                CS.localPosition = new Vector2(-670, Random.Range(-32f, 245f));
                xV = Random.Range(8f, 20f);
                rV = Random.Range(-10f, -20f);
            }

            yV = Random.Range(8f, 15f);

            timer = 0;
            aniEnd = false;

            aniPlay = true;
            await UniTask.WaitUntil(() => alpha >= 1);

            AudioSource.PlayClipAtPoint(bow, Vector3.zero);
        }

        float alpha = 0;
        void Update()
        {
            if (!aniPlay)
                return;

            if (alpha < 1 && !aniEnd)
            {
                LogoImage.color = new Color(1, 1, 1, alpha);
                CSImage.color = new Color(1, 1, 1, alpha);
                text.color = new Color(1, 1, 1, alpha);
                alpha += 0.05f * Kernel.fpsUnscaledDeltaTime;

                return;
            }

            if (aniEnd)
            {
                text.rectTransform.anchoredPosition = text.rectTransform.anchoredPosition.Lerp(Vector3.zero, 0.1f * Kernel.fpsUnscaledDeltaTime);
                CSImage.transform.rotation = Quaternion.Lerp(CSImage.transform.rotation, Quaternion.Euler(Vector3.zero), 0.1f * Kernel.fpsUnscaledDeltaTime);
                CS.localPosition = CS.localPosition.Lerp(new Vector3(24, -24), 0.1f * Kernel.fpsUnscaledDeltaTime);

                if (timer >= 2)
                {
                    LogoImage.color = new Color(1, 1, 1, alpha);
                    CSImage.color = new Color(1, 1, 1, alpha);
                    text.color = new Color(1, 1, 1, alpha);
                    alpha -= 0.05f * Kernel.fpsUnscaledDeltaTime;

                    if (alpha < 0)
                        isAniPlayed = false;
                }
                else
                    timer += Kernel.unscaledDeltaTime;
            }
            else
            {
                LogoImage.color = Color.white;
                CSImage.color = Color.white;
                text.color = Color.white;
                alpha = 1;

                CS.localPosition = new Vector2(CS.localPosition.x + xV * Kernel.fpsUnscaledDeltaTime, CS.localPosition.y + yV * Kernel.fpsUnscaledDeltaTime);
                CSImage.transform.localEulerAngles = new Vector3(CSImage.transform.localEulerAngles.x, CSImage.transform.localEulerAngles.y, CSImage.transform.localEulerAngles.z + rV * Kernel.fpsUnscaledDeltaTime);
                yV -= 0.5f * Kernel.fpsUnscaledDeltaTime;

                if (CS.localPosition.x >= -25 && CS.localPosition.x <= 25 && CS.localPosition.y >= -25 && CS.localPosition.y <= 25)
                {
                    text.rectTransform.anchoredPosition = new Vector3(0, -13);
                    text.text = showText;

                    AudioSource.PlayClipAtPoint(drawmap, Vector3.zero);

                    aniEnd = true;
                }
                else if (xFlip && (CS.localPosition.x <= -500 || CS.localPosition.y <= -300))
                {
                    text.rectTransform.anchoredPosition = new Vector3(0, -13);
                    text.text = showText;

                    AudioSource.PlayClipAtPoint(drawmap, Vector3.zero);

                    aniEnd = true;
                }
                else if (!xFlip && (CS.localPosition.x >= 500 || CS.localPosition.y <= -300))
                {
                    text.rectTransform.anchoredPosition = new Vector3(0, -13);
                    text.text = showText;

                    AudioSource.PlayClipAtPoint(drawmap, Vector3.zero);

                    aniEnd = true;
                }
            }
        }
    }
}