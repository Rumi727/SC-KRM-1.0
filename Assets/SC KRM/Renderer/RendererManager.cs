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
                ThreadMetaData threadMetaData = ThreadManager.Create(Rerender, customRenderers, "notice.running_task.rerender.name");
                await UniTask.WaitUntil(() => threadMetaData.thread == null);
            }
            else
            {
                for (int i = 0; i < customRenderers.Length; i++)
                {
                    CustomAllRenderer customRenderer = customRenderers[i];
                    customRenderer.ResourceReload();
                    customRenderer.Rerender();
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
                customRenderer.Clear();
                customRenderer.ResourceReload();

                threadMetaData.progress = (float)i / (customRenderers.Length - 1);
                Thread.Sleep(1);
            }
        }
    }
}