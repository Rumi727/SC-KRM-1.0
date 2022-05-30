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
        public static CancellationToken cancelToken => _cancel.Token;



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
            {
                AsyncTask asyncTask = asyncTasks[i];
                if (!asyncTask.cantCancel)
                {
                    asyncTask.Remove();
                    i--;
                }
            }

            if (!onlyAsyncTaskClass)
            {
                _cancel.Cancel();
                _cancel.Dispose();
            }
        }
    }

    public class AsyncTask
    {
        public AsyncTask(string name = "", string info = "", bool loop = false, bool cantCancel = false)
        {
            this.name = name;
            this.info = info;
            this.loop = loop;
            this.cantCancel = cantCancel;

            AsyncTaskManager.asyncTasks.Add(this);

            AsyncTaskManager.AsyncTaskAddEventInvoke();
            AsyncTaskManager.AsyncTaskChangeEventInvoke();
        }

        public virtual string name { get; set; }
        public virtual string info { get; set; }
        public virtual bool loop { get; set; }
        public virtual bool cantCancel { get; set; }

        public virtual float progress { get; set; }
        public virtual float maxProgress { get; set; }



        public virtual bool isCanceled { get; set; }



        object cancelEventLockObject = new object();
        event Action _cancelEvent;
        /// <summary>
        /// Thread-Safe, cancelEvent += () => { cancelEvent += () => { }; }; Do not add more methods to this event from inside this event method like this. This causes deadlock
        /// </summary>
        public event Action cancelEvent
        {
            add
            {
                Monitor.Enter(cancelEventLockObject);
                _cancelEvent += value;
                Monitor.Exit(cancelEventLockObject);
            }
            remove
            {
                Monitor.Enter(cancelEventLockObject);
                _cancelEvent -= value;
                Monitor.Exit(cancelEventLockObject);
            }
        }



        readonly CancellationTokenSource _cancel = new CancellationTokenSource();
        public CancellationToken cancel => _cancel.Token;



        /// <summary>
        /// 이 함수는 메인 스레드에서만 실행할수 있습니다
        /// This function can only be executed on the main thread
        /// </summary>
        /// <exception cref="NotMainThreadMethodException"></exception>
        public virtual bool Remove(bool force = false)
        {
            if (!ThreadManager.isMainThread)
                throw new NotMainThreadMethodException(nameof(Remove));

            if (!isCanceled && (!cantCancel || force))
            {
                isCanceled = true;

                Monitor.Enter(cancelEventLockObject);
                _cancelEvent?.Invoke();
                Monitor.Exit(cancelEventLockObject);

                AsyncTaskManager.asyncTasks.Remove(this);

                AsyncTaskManager.AsyncTaskChangeEventInvoke();
                AsyncTaskManager.AsyncTaskRemoveEventInvoke();

                _cancel.Cancel();

                return true;
            }

            return false;
        }
    }
}
