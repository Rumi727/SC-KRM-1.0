using Cysharp.Threading.Tasks;
using SCKRM.Threads;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace SCKRM.Renderer
{
    public static class RendererManager
    {
        public static void AllRerender(bool thread = true) => Rerender(UnityEngine.Object.FindObjectsOfType<CustomAllRenderer>(true), thread).Forget();

        public static void AllTextRerender(bool thread = true) => Rerender(UnityEngine.Object.FindObjectsOfType<CustomAllTextRenderer>(true), thread).Forget();

        static ThreadMetaData rerenderThread;
        public static async UniTaskVoid Rerender(CustomAllRenderer[] customRenderers, bool thread = true)
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

                ThreadMetaData threadMetaData = ThreadManager.Create(Rerender, customRenderers, "notice.running_task.rerender.name");
                rerenderThread = threadMetaData;

                if (await UniTask.WaitUntil(() => threadMetaData.thread == null, cancellationToken: AsyncTaskManager.cancel).SuppressCancellationThrow())
                    return;
            }
            else
            {
                for (int i = 0; i < customRenderers.Length; i++)
                {
                    CustomAllRenderer customRenderer = customRenderers[i];
                    customRenderer.Refresh();
                    //customRenderer.Rerender();
                }
            }
        }

        static void Rerender(CustomAllRenderer[] customRenderers, ThreadMetaData threadMetaData)
        {
            if (customRenderers.Length == 0)
                threadMetaData.progress = 1;

            for (int i = 0; i < customRenderers.Length; i++)
            {
                CustomAllRenderer customRenderer = customRenderers[i];
                //customRenderer.Clear();
                customRenderer.Refresh();

                threadMetaData.progress = (float)i / (customRenderers.Length - 1);
            }
        }
    }
}