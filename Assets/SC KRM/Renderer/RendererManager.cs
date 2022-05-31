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

        static ThreadMetaData rerenderThread;
        public static void Rerender(CustomAllRenderer[] customRenderers, bool thread = true)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(Rerender));
            if (!Kernel.isPlaying)
                throw new NotPlayModeMethodException(nameof(Rerender));

            if (thread)
            {
                if (rerenderThread != null)
                    rerenderThread.Remove();

                ThreadMetaData threadMetaData = ThreadManager.Create(Rerender, customRenderers, "notice.running_task.rerender.name");
                rerenderThread = threadMetaData;
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
            int stopLoop = 0;

            threadMetaData.maxProgress = customRenderers.Length - 1;

            threadMetaData.cancelEvent += CancelEvent;
            threadMetaData.cantCancel = false;

            for (int i = 0; i < customRenderers.Length; i++)
            {
                Interlocked.Decrement(ref stopLoop);
                if (Interlocked.Increment(ref stopLoop) > 0)
                    return;

                customRenderers[i].Refresh();

                threadMetaData.progress = i;
            }

            void CancelEvent()
            {
                Interlocked.Increment(ref stopLoop);

                threadMetaData.maxProgress = 1;
                threadMetaData.progress = 1;

                threadMetaData.cancelEvent -= CancelEvent;
            }
        }
    }
}