using Cysharp.Threading.Tasks;
using SCKRM.Threads;
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

        public virtual void Remove()
        {
            AsyncTaskManager.asyncTasks.Remove(this);

            _cancel.Cancel();
            _cancel.Dispose();
        }
    }
}
