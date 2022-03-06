using Cysharp.Threading.Tasks;
using SCKRM.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM
{
    public class SplashScreenBackground : MonoBehaviour
    {
        [SerializeField] Graphic graphic;
        void Awake()
        {
            graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, 1);
            Kernel.InitialLoadEndSceneMove += PadeOut;
        }

        async void PadeOut()
        {
            //씬이 이동하고 나서 잠깐 렉이 있기 때문에, 애니메이션이 제대로 재생될려면 딜레이를 걸어줘야합니다
            if (await UniTask.DelayFrame(3, cancellationToken: this.GetCancellationTokenOnDestroy()).SuppressCancellationThrow())
                return;

            if (graphic != null)
            {
                while (graphic.color.a > 0)
                {
                    Color color = graphic.color;
                    graphic.color = new Color(color.r, color.g, color.b, color.a.MoveTowards(0, 0.05f * Kernel.fpsUnscaledDeltaTime));

                    if (await UniTask.DelayFrame(1, PlayerLoopTiming.Initialization, this.GetCancellationTokenOnDestroy()).SuppressCancellationThrow())
                        return;
                }

                graphic.gameObject.SetActive(false);
            }
        }
    }
}
