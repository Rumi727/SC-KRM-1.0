using SCKRM.Object;
using SCKRM.Threads;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.UI.SideBar
{
    [AddComponentMenu("")]
    public sealed class RunningTaskList : UI
    {
        [System.NonSerialized] List<ThreadMetaData> tempList = new List<ThreadMetaData>();
        void Update()
        {
            if (!Kernel.isInitialLoadEnd)
                return;

            if (tempList.Count != ThreadManager.runningThreads.Count)
            {
                RunningTaskInfo[] runningTaskInfos = GetComponentsInChildren<RunningTaskInfo>();
                for (int i = 0; i < ThreadManager.runningThreads.Count; i++)
                {
                    if (i >= runningTaskInfos.Length)
                    {
                        RunningTaskInfo runningTaskInfo = (RunningTaskInfo)ObjectPoolingSystem.ObjectCreate("running_task_list.running_task", transform);
                        runningTaskInfo.transform.SetSiblingIndex(0);
                        runningTaskInfo.threadMetaData = ThreadManager.runningThreads[i];
                        runningTaskInfo.InfoLoad();
                    }
                }

                tempList = new List<ThreadMetaData>(ThreadManager.runningThreads);
            }
        }
    }
}