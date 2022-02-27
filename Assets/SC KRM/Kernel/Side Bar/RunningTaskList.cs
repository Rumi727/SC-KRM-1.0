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
        [System.NonSerialized] List<AsyncTask> tempList = new List<AsyncTask>();
        void Update()
        {
            if (!Kernel.isInitialLoadEnd)
                return;

            if (tempList.Count != AsyncTaskManager.asyncTasks.Count)
            {
                RunningTaskInfo[] runningTaskInfos = GetComponentsInChildren<RunningTaskInfo>();
                for (int i = 0; i < AsyncTaskManager.asyncTasks.Count; i++)
                {
                    if (i >= runningTaskInfos.Length)
                    {
                        RunningTaskInfo runningTaskInfo = (RunningTaskInfo)ObjectPoolingSystem.ObjectCreate("running_task_list.running_task", transform);
                        runningTaskInfo.transform.SetSiblingIndex(0);
                        runningTaskInfo.asyncTask = AsyncTaskManager.asyncTasks[i];
                        runningTaskInfo.asyncTaskIndex = i;
                        runningTaskInfo.InfoLoad();
                    }
                }

                tempList = new List<AsyncTask>(AsyncTaskManager.asyncTasks);
            }
        }
    }
}