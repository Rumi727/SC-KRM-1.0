using Cysharp.Threading.Tasks;
using SCKRM.Threads;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace SCKRM
{
    public static class AsyncTaskManager
    {
        static readonly CancellationTokenSource _cancel = new CancellationTokenSource();
        public static CancellationToken cancel => _cancel.Token;



        public static event Action asyncTaskAdd = () => { };
        public static event Action asyncTaskChange = () => { };
        public static event Action asyncTaskRemove = () => { };

        public static void AsyncTaskAddEventInvoke() => asyncTaskAdd();
        public static void AsyncTaskRemoveEventInvoke() => asyncTaskRemove();
        public static void AsyncTaskChangeEventInvoke() => asyncTaskChange();



        public static List<AsyncTask> asyncTasks { get; } = new List<AsyncTask>();

        public static void AllAsyncTaskCancel(bool onlyAsyncTaskClass = true)
        {
            for (int i = 0; i < asyncTasks.Count; i++)
                asyncTasks[i].Remove();

            if (!onlyAsyncTaskClass)
            {
                _cancel.Cancel();
                _cancel.Dispose();
            }
        }

        public static int GetAsyncTaskClassCount() => asyncTasks.Count;
    }

    public class AsyncTask
    {
        public AsyncTask(string name = "", string info = "", bool loop = false)
        {
            this.name = name;
            this.info = info;
            this.loop = loop;

            AsyncTaskManager.asyncTasks.Add(this);

            AsyncTaskManager.AsyncTaskAddEventInvoke();
            AsyncTaskManager.AsyncTaskChangeEventInvoke();
        }

        public string name { get; }
        public string info { get; }
        public bool loop { get; }

        /// <summary>
        /// 0.0 - 1.0
        /// </summary>
        public float progress { get; set; }

        readonly CancellationTokenSource _cancel = new CancellationTokenSource();
        public CancellationToken cancel => _cancel.Token;

        /// <summary>
        /// 이 함수는 메인 스레드에서만 실행할수 있습니다
        /// This function can only be executed on the main thread
        /// </summary>
        /// <exception cref="NotMainThreadMethodException"></exception>
        public virtual void Remove()
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(Remove));

            AsyncTaskManager.asyncTasks.Remove(this);

            AsyncTaskManager.AsyncTaskChangeEventInvoke();
            AsyncTaskManager.AsyncTaskRemoveEventInvoke();

            _cancel.Cancel();
        }
    }
}
