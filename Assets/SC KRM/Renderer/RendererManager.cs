using Cysharp.Threading.Tasks;
using SCKRM.Threads;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace SCKRM.Renderer
{
    public static class RendererManager
    {
        public static void AllRerender(bool thread = true) => Rerender(UnityEngine.Object.FindObjectsOfType<CustomAllRenderer>(true), thread);

        public static void AllTextRerender(bool thread = true) => Rerender(UnityEngine.Object.FindObjectsOfType<CustomAllTextRenderer>(true), thread);

        public static async void Rerender(CustomAllRenderer[] customRenderers, bool thread = true)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(Rerender));
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(Rerender));
#endif
            if (thread)
            {
                Debug.Log("RendererManager: Rerender start! ");

                ThreadMetaData threadMetaData = ThreadManager.Create(Rerender, customRenderers, "notice.running_task.rerender.name");
                await UniTask.WaitUntil(() => threadMetaData.thread == null);

                Debug.Log("RendererManager: Rerender finished!");
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