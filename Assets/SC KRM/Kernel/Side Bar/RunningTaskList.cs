using SCKRM.Object;
using SCKRM.Threads;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.UI.SideBar
{
    [AddComponentMenu("")]
    public sealed class RunningTaskList : MonoBehaviour
    {
        [System.NonSerialized] int tempCount = 0;
        void Update()
        {
            if (!Kernel.isInitialLoadEnd)
                return;

            if (tempCount != ThreadManager.runningThreads.Count)
            {
                RunningTaskInfo[] runningTaskInfos = GetComponentsInChildren<RunningTaskInfo>();
                for (int i = 0; i < runningTaskInfos.Length - ThreadManager.runningThreads.Count; i++)
                    runningTaskInfos[i].Remove();

                runningTaskInfos = GetComponentsInChildren<RunningTaskInfo>();
                for (int i = 0; i < ThreadManager.runningThreads.Count; i++)
                {
                    if (i >= runningTaskInfos.Length)
                    {
                        RunningTaskInfo runningTaskInfo = ObjectPoolingSystem.ObjectCreate("running_task_list.running_task", transform).GetComponent<RunningTaskInfo>();
                        runningTaskInfo.transform.SetSiblingIndex(0);
                        runningTaskInfo.index = i;
                        runningTaskInfo.InfoLoad();
                    }
                    else
                    {
                        RunningTaskInfo runningTaskInfo = runningTaskInfos[i];
                        runningTaskInfo.transform.SetSiblingIndex(0);
                        runningTaskInfo.index = i;
                        runningTaskInfo.InfoLoad();
                    }
                }

                tempCount = ThreadManager.runningThreads.Count;
            }
        }
    }
}