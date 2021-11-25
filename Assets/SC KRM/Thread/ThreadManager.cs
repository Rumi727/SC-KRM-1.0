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
        public static bool isMainThread { get => mainThreadId == Thread.CurrentThread.ManagedThreadId; }



        public static void AllThreadRemove()
        {
            for (int i = 0; i < runningThreads.Count; i++)
                runningThreads[i]?.Remove();
        }

        public static void ThreadAutoRemove()
        {
            if (!isMainThread)
            {
                while (true)
                {
                    for (int i = 0; i < runningThreads.Count; i++)
                    {
                        ThreadMetaData runningThread = runningThreads[i];
                        if (runningThread != null && !runningThread.thread.IsAlive)
                            runningThread.Remove();
                    }

                    Thread.Sleep(100);
                }
            }
            else
            {
                for (int i = 0; i < runningThreads.Count; i++)
                {
                    ThreadMetaData runningThread = runningThreads[i];
                    if (runningThread != null && !runningThread.thread.IsAlive)
                        runningThread.Remove();
                }
            }
        }

        #region Thread Create Method
        public static ThreadMetaData Create(Action method, string name = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = name;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method());
            threadMetaData.thread.Start();

            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create(Action<ThreadMetaData> method, string name = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = name;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(threadMetaData));
            threadMetaData.thread.Start();

            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T>(Action<T, ThreadMetaData> method, T obj, string name = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = name;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(obj, threadMetaData));
            threadMetaData.thread.Start();

            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2>(Action<T1, T2, ThreadMetaData> method, T1 arg1, T2 arg2, string name = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = name;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, threadMetaData));
            threadMetaData.thread.Start();

            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3>(Action<T1, T2, T3, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, string name = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = name;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, threadMetaData));
            threadMetaData.thread.Start();

            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4>(Action<T1, T2, T3, T4, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, string name = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = name;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, threadMetaData));
            threadMetaData.thread.Start();

            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, string name = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = name;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, arg5, threadMetaData));
            threadMetaData.thread.Start();

            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, string name = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = name;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, arg5, arg6, threadMetaData));
            threadMetaData.thread.Start();

            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, string name = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = name;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, threadMetaData));
            threadMetaData.thread.Start();

            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, string name = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = name;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, threadMetaData));
            threadMetaData.thread.Start();

            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, string name = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = name;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, threadMetaData));
            threadMetaData.thread.Start();

            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, string name = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = name;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, threadMetaData));
            threadMetaData.thread.Start();

            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, string name = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = name;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, threadMetaData));
            threadMetaData.thread.Start();

            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, string name = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = name;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, threadMetaData));
            threadMetaData.thread.Start();

            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, string name = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = name;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, threadMetaData));
            threadMetaData.thread.Start();

            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, string name = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = name;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, threadMetaData));
            threadMetaData.thread.Start();

            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        public static ThreadMetaData Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, ThreadMetaData> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, string name = "", bool loop = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                throw new NotPlayModeThreadCreateException();
#endif

            ThreadMetaData threadMetaData = new ThreadMetaData();
            threadMetaData.name = name;
            threadMetaData.loop = loop;
            threadMetaData.thread = new Thread(() => method(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, threadMetaData));
            threadMetaData.thread.Start();

            runningThreads.Add(threadMetaData);

            return threadMetaData;
        }
        #endregion
    }

    public sealed class ThreadMetaData
    {
        public Thread thread { get; set; } = null;
        public string name { get; set; } = "";
        /// <summary>
        /// 0.0 - 1.0
        /// </summary>
        public float progress { get; set; } = 0;
        public bool loop { get; set; } = false;

        public void Remove()
        {
            ThreadManager.runningThreads.Remove(this);

            if (thread != null)
            {
                Thread _thread = thread;
                thread = null;
                _thread.Abort();
            }
        }
    }

    public class NotPlayModeThreadCreateException : Exception
    {
        /// <summary>
        /// 플레이 모드가 아닐때 스레드를 생성하는건 금지되어있습니다. 직접 스레드를 생성해주세요
        /// </summary>
        public NotPlayModeThreadCreateException() : base("플레이 모드가 아닐때 스레드를 생성하는건 금지되어있습니다. 직접 스레드를 생성해주세요") { }
    }
}