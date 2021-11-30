using SCKRM.Threads;
using System.Threading;
using UnityEngine;

namespace SCKRM.Renderer
{
    public static class RendererManager
    {
        public static void AllRerender(bool thread = true) => AllRerender(UnityEngine.Object.FindObjectsOfType<CustomAllRenderer>(true), thread);

        public static void AllTextRerender(bool thread = true) => AllRerender(UnityEngine.Object.FindObjectsOfType<CustomAllTextRenderer>(true), thread);

        public static void AllRerender(CustomAllRenderer[] customRenderers, bool thread = true)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeMethodException(nameof(AllRerender));
#endif

            if (thread)
                ThreadManager.Create(Rerender, customRenderers, "notice.running_task.resource_pack_refresh.name");
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

            threadMetaData.Remove();
        }
    }
}