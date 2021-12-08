using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace SCKRM.Threads
{
    public static class ThreadManager
    {
        public static List<ThreadMetaData> runningThreads { get; } = new List<ThreadMetaData>();

        public static int mainThreadId { get; } = Thread.CurrentThread.ManagedThreadId;

        /// <summary>
        /// mainThreadId == Thread.CurrentThread.ManagedThreadId
        /// </summary>
        public static bool isMainThread => mainThreadId == Thread.CurrentThread.ManagedThreadId;



        /// <summary>
        /// 이 이벤트는 다른 스레드에서 호출되므로 Unity API를 사용하지 마십시오.
        /// This event is called from another thread, so don't use the Unity API
        /// </summary>
        public static event Action threadChange;
        public static void ThreadChangeEventInvoke() => threadChange?.Invoke();



        public static void AllThreadRemove()
        {
            for (int i = 0; i < runningThreads.Count; i++)
                runningThreads[i]?.Remove();
        }

        public static void ThreadAutoRemove()
        {
            for (int i = 0; i < runningThreads.Count; i++)
            {
                ThreadMetaData runningThread = runningThreads[i];
                if (runningThread != null && !runningThread.autoRemoveDisable && (runningThread.thread == null || !runningThread.thread.IsAlive))
                    runningThread.Remove();
            }
        }

        public static void ThreadAutoRemove(bool loop)
        {
            if (!isMainThread)
            {
                if (loop)
                {
                    while (true)
                    {
                        ThreadAutoRemove();
                        Thread.Sleep(100);
                    }
                }
                else
                    ThreadAutoRemove();
            }
            else
                throw new MainThreadMethodException();
        }

        #region Thread Create Method
        public static ThreadMetaData Create(Action method, string name = "", string info = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = name;
            threadMetaData.info = info;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method());
            threadMetaData.thread.Start();

            ThreadChangeEventInvoke();
            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create(Action<ThreadMetaData> method, string name = "", string info = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = name;
            threadMetaData.info = info;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(threadMetaData));
            threadMetaData.thread.Start();

            ThreadChangeEventInvoke();
            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T>(Action<T, ThreadMetaData> method, T obj, string name = "", string info = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = name;
            threadMetaData.info = info;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(obj, threadMetaData));
            threadMetaData.thread.Start();

            ThreadChangeEventInvoke();
            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2>(Action<T1, T2, ThreadMetaData> method, T1 arg1, T2 arg2, string name = "", string info = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = name;
            threadMetaData.info = info;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, threadMetaData));
            threadMetaData.thread.Start();

            ThreadChangeEventInvoke();
            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3>(Action<T1, T2, T3, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, string name = "", string info = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = name;
            threadMetaData.info = info;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, threadMetaData));
            threadMetaData.thread.Start();

            ThreadChangeEventInvoke();
            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4>(Action<T1, T2, T3, T4, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, string name = "", string info = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();;
            threadMetaData.name = name;
            threadMetaData.info = info;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, threadMetaData));
            threadMetaData.thread.Start();

            ThreadChangeEventInvoke();
            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, string name = "", string info = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();;
            threadMetaData.name = name;
            threadMetaData.info = info;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, arg5, threadMetaData));
            threadMetaData.thread.Start();

            ThreadChangeEventInvoke();
            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, string name = "", string info = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();;
            threadMetaData.name = name;
            threadMetaData.info = info;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, arg5, arg6, threadMetaData));
            threadMetaData.thread.Start();

            ThreadChangeEventInvoke();
            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, string name = "", string info = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();;
            threadMetaData.name = name;
            threadMetaData.info = info;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, threadMetaData));
            threadMetaData.thread.Start();

            ThreadChangeEventInvoke();
            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, string name = "", string info = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();;
            threadMetaData.name = name;
            threadMetaData.info = info;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, threadMetaData));
            threadMetaData.thread.Start();

            ThreadChangeEventInvoke();
            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, string name = "", string info = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();;
            threadMetaData.name = name;
            threadMetaData.info = info;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, threadMetaData));
            threadMetaData.thread.Start();

            ThreadChangeEventInvoke();
            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, string name = "", string info = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();;
            threadMetaData.name = name;
            threadMetaData.info = info;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, threadMetaData));
            threadMetaData.thread.Start();

            ThreadChangeEventInvoke();
            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, string name = "", string info = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();;
            threadMetaData.name = name;
            threadMetaData.info = info;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, threadMetaData));
            threadMetaData.thread.Start();

            ThreadChangeEventInvoke();
            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, string name = "", string info = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();;
            threadMetaData.name = name;
            threadMetaData.info = info;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, threadMetaData));
            threadMetaData.thread.Start();

            ThreadChangeEventInvoke();
            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, string name = "", string info = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();;
            threadMetaData.name = name;
            threadMetaData.info = info;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, threadMetaData));
            threadMetaData.thread.Start();

            ThreadChangeEventInvoke();
            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, string name = "", string info = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();;
            threadMetaData.name = name;
            threadMetaData.info = info;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, threadMetaData));
            threadMetaData.thread.Start();

            ThreadChangeEventInvoke();
            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, string name = "", string info = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();;
            threadMetaData.name = name;
            threadMetaData.info = info;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, threadMetaData));
            threadMetaData.thread.Start();

            ThreadChangeEventInvoke();
            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        #endregion
    }

    public sealed class ThreadMetaData
    {
        public Thread thread { get; set; } = null;
        public string name { get; set; } = "";
        public string info { get; set; } = "";
        /// <summary>
        /// 0.0 - 1.0
        /// </summary>
        public float progress { get; set; } = 0;
        public bool loop { get; set; } = false;
        public bool autoRemoveDisable { get; set; } = false;

        public void Remove()
        {
            ThreadManager.ThreadChangeEventInvoke();
            ThreadManager.runningThreads.Remove(this);

            if (thread != null)
            {
                Thread _thread = thread;
                thread = null;
                _thread.Abort();
            }
        }
    }

    public class NotMainThreadMethodException : Exception
    {
        /// <summary>
        /// This function must run on the main thread
        /// 이 함수는 메인 스레드에서 실행 해야 합니다
        /// </summary>
        public NotMainThreadMethodException() : base("This function must run on the main thread\n이 함수는 메인 스레드에서 실행 해야 합니다") { }

        /// <summary>
        /// {method} function must be executed on the main thread
        /// {method} 함수는 메인 스레드에서 실행되어야 합니다
        /// </summary>
        public NotMainThreadMethodException(string method) : base($"{method} function must be executed on the main thread\n{method} 함수는 메인 스레드에서 실행되어야 합니다") { }
    }

    public class MainThreadMethodException : Exception
    {
        /// <summary>
        /// This function cannot be executed on the main thread
        /// 이 함수는 메인 스레드에서 실행 할 수 없습니다
        /// </summary>
        public MainThreadMethodException() : base("This function cannot be executed on the main thread\n이 함수는 메인스레드에서 실행 할 수 없습니다") { }

        /// <summary>
        /// {method} function cannot be executed on the main thread
        /// {method} 함수는 메인 스레드에서 실행 할 수 없습니다
        /// </summary>
        public MainThreadMethodException(string method) : base($"{method} function cannot be executed on the main thread\n{method} 함수는 메인스레드에서 실행 할 수 없습니다") { }
    }

    public class NotPlayModeThreadCreateException : Exception
    {
        /// <summary>
        /// It is forbidden to spawn threads when not in play mode. Please create your own thread
        /// 플레이 모드가 아닐때 스레드를 생성하는건 금지되어있습니다. 직접 스레드를 생성해주세요
        /// </summary>
        public NotPlayModeThreadCreateException() : base("It is forbidden to spawn threads when not in play mode. Please create your own thread\n플레이 모드가 아닐때 스레드를 생성하는건 금지되어있습니다. 직접 스레드를 생성해주세요") { }
    }
}