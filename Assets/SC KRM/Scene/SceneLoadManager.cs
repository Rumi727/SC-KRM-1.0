using Cysharp.Threading.Tasks;
using SCKRM.Loading;
using UnityEngine;

namespace SCKRM.Scene
{
    public static class SceneManager
    {
        public static bool isDone { get; private set; } = true;
        public static bool isLoading { get; private set; } = false;

        public static async UniTask LoadScene(int sceneBuildIndex) => await loadScene(sceneBuildIndex);

        static async UniTask loadScene(int sceneBuildIndex)
        {
            if (isLoading)
            {
                Debug.LogWarning("Could not load another scene while loading scene");
                return;
            }

            isLoading = true;
            isDone = false;

            LoadingAni loadingAni = LoadingAniManager.LoadingStart();
            loadingAni.maxProgress = 1;

            await UniTask.WaitUntil(() => loadingAni.isStartAniEnd || loadingAni.isRemoved);

            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            await UniTask.NextFrame();

            AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneBuildIndex);
            asyncOperation.allowSceneActivation = false;

            while (!asyncOperation.isDone || !asyncOperation.allowSceneActivation)
            {
                loadingAni.progress = asyncOperation.progress + 0.1f;
                asyncOperation.allowSceneActivation = loadingAni.isLongLoadingAniEnd && loadingAni.progress >= 1;

                await UniTask.NextFrame(PlayerLoopTiming.Initialization);
            }

            isLoading = false;
        }
    }
}
