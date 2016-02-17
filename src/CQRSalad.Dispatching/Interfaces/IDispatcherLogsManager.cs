using System;
using System.Collections.Generic;

namespace CQRSalad.Dispatching
{
    public sealed class CLRObjectLog
    {
        public CLRObjectLog(object instance, string typeName)
        {
            Instance = instance;
            CLR_Type = typeName;
        }

        public CLRObjectLog(object instance)
        {
            Argument.IsNotNull(instance, "instance");

            Instance = instance;
            CLR_Type = instance.GetType().AssemblyQualifiedName;
        }

        public string CLR_Type { get; private set; }
        public object Instance { get; private set; }
    }
    
    public interface IDispatcherContextLog
    {
        string Id { get; }
        CLRObjectLog Message { get; }
        CLRObjectLog Handler { get; }
        string HandlingMethod { get; }
        CLRObjectLog Result { get; }
        string Error { get; }
        DateTime Timestamp { get; }
    }

    public interface IDispatcherLogsManager
    {
        void Log(IDispatcherContext context);
        void Log(IDispatcherContext context, Exception exception);
        List<IDispatcherContextLog> GetLogs();
    }

    public sealed class LoggingInterceptor : IContextInterceptor
    {
        private readonly IDispatcherLogsManager _logsManager;

        public LoggingInterceptor(IDispatcherLogsManager logsManager)
        {
            _logsManager = logsManager;
        }

        public void OnInvocationStarted(IDispatcherContext context)
        {
        }

        public void OnInvocationFinished(IDispatcherContext context)
        {
            _logsManager.Log(context);
        }

        public void OnException(IDispatcherContext context, Exception invocationException)
        {
            _logsManager.Log(context);
        }
    }
}