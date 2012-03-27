namespace StealFocus.Tracer.L4N
{
    using System;
    using System.Globalization;
    using System.Runtime.Remoting.Messaging;
    using System.Text;

    using log4net;
    using log4net.Appender;
    using log4net.Core;

    using SignalR.Client.Hubs;

    using StealFocus.Tracer.Configuration;
    using StealFocus.Tracer.Model;

    public class TracerAppender : AppenderSkeleton
    {
        private const string TacerHubName = "StealFocus.Tracer.Web.UI.TraceHub";

        private const string TracerHubMethodName = "Send";

        private static readonly object SyncRoot = new object();

        private static HubConnection hubConnection;

        private static IHubProxy hubProxy;

        public string TracerHubUrl { get; set; }

        public string TracerSourceName { get; set; }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }

            try
            {
                Guid? correlationId = GetCorrelationId();
                Guid? batchId = GetBatchId();

                //// Problems with calling SignalR asynchronously, need to work out why use async to release thread for caller.
                //// SendTraceEventForLoggingEvent sendTraceEventForLoggingEvent = SendTraceEventForLoggingEvent;
                //// sendTraceEventForLoggingEvent.BeginInvoke(loggingEvent, correlationId, batchId, this.GetTracerHubUri(), this.GetSource(), SendTraceEventForLoggingEventCallback, null);

                // Calling SignalR synchronously (this is bad), until above problem is resolved.
                SendTraceEventForLoggingEvent(loggingEvent, correlationId, batchId, this.GetTracerHubUri(), this.GetSource());
            }
            catch (Exception e)
            {
                StringBuilder errorMessage = new StringBuilder();
                errorMessage.AppendLine("There was an error logging to the Tracer server. The exception was:");
                errorMessage.AppendLine(e.ToString());
                System.Diagnostics.Debug.WriteLine(errorMessage.ToString());
                throw;
            }
        }

        private static void SendTraceEventForLoggingEventCallback(IAsyncResult ar)
        {
            AsyncResult result = (AsyncResult)ar;
            SendTraceEventForLoggingEvent sendTraceEventForLoggingEvent = (SendTraceEventForLoggingEvent)result.AsyncDelegate;
            try
            {
                sendTraceEventForLoggingEvent.EndInvoke(ar);
            }
            catch (Exception e)
            {
                StringBuilder errorMessage = new StringBuilder();
                errorMessage.AppendLine("There was an error logging to the Tracer server. The exception was:");
                errorMessage.AppendLine(e.ToString());
                System.Diagnostics.Debug.WriteLine(errorMessage.ToString());
                throw;
            }
        }

        private static Guid? GetCorrelationId()
        {
            if (ThreadContext.Properties[ThreadContextKey.CorrelationId] != null)
            {
                return (Guid)ThreadContext.Properties[ThreadContextKey.CorrelationId];
            }

            return Guid.NewGuid();
        }

        private static Guid? GetBatchId()
        {
            if (ThreadContext.Properties[ThreadContextKey.BatchId] != null)
            {
                return (Guid)ThreadContext.Properties[ThreadContextKey.BatchId];
            }

            return Guid.NewGuid();
        }

        private static void EnsureHubProxyIsInitialised(Uri tracerHubUri)
        {
            lock (SyncRoot)
            {
                if (hubProxy == null)
                {
                    hubConnection = new HubConnection(tracerHubUri.AbsoluteUri);
                    hubProxy = hubConnection.CreateProxy(TacerHubName);
                    hubConnection.Start().Wait();
                }
            }
        }

        private static TraceEvent BuildTraceEvent(LoggingEvent loggingEvent, Guid? correlationId, Guid? batchId, string source)
        {
            TraceEvent traceEvent = new TraceEvent();
            traceEvent.BatchId = batchId;
            traceEvent.CorrelationId = correlationId;
            traceEvent.Exception = loggingEvent.ExceptionObject;
            traceEvent.HostName = Environment.MachineName;
            traceEvent.Id = Guid.NewGuid();
            traceEvent.Message = loggingEvent.RenderedMessage;
            traceEvent.Source = source;
            traceEvent.SourceCode = new SourceCode();
            traceEvent.SourceCode.ClassName = loggingEvent.LocationInformation.ClassName;
            traceEvent.SourceCode.FileName = loggingEvent.LocationInformation.FileName;
            traceEvent.SourceCode.LineNumber = loggingEvent.LocationInformation.LineNumber;
            traceEvent.SourceCode.MethodName = loggingEvent.LocationInformation.MethodName;
            traceEvent.Timestamp = loggingEvent.TimeStamp;
            traceEvent.UserName = loggingEvent.UserName;
            return traceEvent;
        }

        private static void SendTraceEventForLoggingEvent(LoggingEvent loggingEvent, Guid? correlationId, Guid? batchId, Uri tracerHubUri, string source)
        {
            EnsureHubProxyIsInitialised(tracerHubUri);
            TraceEvent traceEvent = BuildTraceEvent(loggingEvent, correlationId, batchId, source);
            hubProxy.Invoke(TracerHubMethodName, traceEvent).Wait();
        }

        private string GetSource()
        {
            if (this.TracerSourceName == null)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "No  'tracerSourceName' value was found the in the Tracer log4net appender configuration.");
                throw new TracerException(exceptionMessage);
            }

            return this.TracerSourceName;
        }

        private Uri GetTracerHubUri()
        {
            if (this.TracerSourceName == null)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "No  'tracerHubUrl' value was found the in the Tracer log4net appender configuration.");
                throw new TracerException(exceptionMessage);
            }

            try
            {
                return new Uri(this.TracerHubUrl);
            }
            catch (FormatException e)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The 'tracerHubUrl' value of '{0}' found the in the configuration could not be parsed as a URI.", this.TracerHubUrl);
                throw new TracerException(exceptionMessage, e);
            }
        }
    }
}
