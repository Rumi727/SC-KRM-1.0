using SCKRM.UI.SideBar;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM
{
    public sealed class LogManager : MonoBehaviour
    {
        public static List<Log> loggers { get; } = new List<Log>();
        static ConcurrentQueue<Log> loggerQueue = new ConcurrentQueue<Log>();

        void OnEnable() => Application.logMessageReceivedThreaded += logCallBack;
        void OnDisable() => Application.logMessageReceivedThreaded -= logCallBack;

        void Update()
        {
            while (loggerQueue.TryDequeue(out Log log))
            {
                loggers.Add(log);

                if (log.logType == LogType.Error || log.logType == LogType.Exception)
                    NoticeManager.Notice(log.condition, log.stackTrace, NoticeManager.Type.error).Forget();
                else if (log.logType == LogType.Warning)
                    NoticeManager.Notice(log.condition, log.stackTrace).Forget();
            }
        }

        static void logCallBack(string condition, string stackTrace, LogType logType) => loggerQueue.Enqueue(new Log(condition, stackTrace, logType));

        public class Log
        {
            public string condition { get; }
            public string stackTrace { get; }
            public LogType logType { get; }

            public Log(string condition, string stackTrace, LogType logType)
            {
                this.condition = condition;
                this.stackTrace = stackTrace;
                this.logType = logType;
            }

            public override string ToString() => $"(condition:{condition}, stackTrace:{stackTrace}, logType:{logType})";
        }
    }
}