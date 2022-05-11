using Cysharp.Threading.Tasks;
using SCKRM.Threads;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace SCKRM.Renderer
{
    public static class RendererManager
    {
        public static void AllRerender(bool thread = true) => Rerender(UnityEngine.Object.FindObjectsOfType<MonoBehaviour>(true).OfType<IResourceRefresh>().ToArray(), thread).Forget();

        public static void AllTextRerender(bool thread = true) => Rerender(UnityEngine.Object.FindObjectsOfType<MonoBehaviour>(true).OfType<IResourceTextRefresh>().ToArray(), thread).Forget();

        static ThreadMetaData rerenderThread;
        public static async UniTaskVoid Rerender(IResourceRefresh[] resourceRefreshes, bool thread = true)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(Rerender));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(Rerender));
#endif
            if (thread)
            {
                if (rerenderThread != null)
                    rerenderThread.Remove();

                ThreadMetaData threadMetaData = ThreadManager.Create(Rerender, resourceRefreshes, "notice.running_task.rerender.name");
                rerenderThread = threadMetaData;

                if (await UniTask.WaitUntil(() => threadMetaData.thread == null, cancellationToken: AsyncTaskManager.cancelToken).SuppressCancellationThrow())
                    return;
            }
            else
            {
                for (int i = 0; i < resourceRefreshes.Length; i++)
                    resourceRefreshes[i].Refresh();
            }
        }

        static void Rerender(IResourceRefresh[] resourceRefreshes, ThreadMetaData threadMetaData)
        {
            threadMetaData.maxProgress = resourceRefreshes.Length - 1;

            for (int i = 0; i < resourceRefreshes.Length; i++)
            {
                resourceRefreshes[i].Refresh();
                threadMetaData.progress = i;
            }
        }
    }
}