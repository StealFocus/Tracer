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
                SendTraceEventForLoggingEvent sendTraceEventForLoggingEvent = SendTraceEventForLogginEvent;
                sendTraceEventForLoggingEvent.BeginInvoke(loggingEvent, correlationId, batchId, this.GetTracerHubUri(), this.GetSource(), Callback, null);
                SendTraceEventForLogginEvent(loggingEvent, correlationId, batchId, this.GetTracerHubUri(), this.GetSource());
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

        private static void Callback(IAsyncResult ar)
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

            return null;
        }

        private static Guid? GetBatchId()
        {
            if (ThreadContext.Properties[ThreadContextKey.BatchId] != null)
            {
                return (Guid)ThreadContext.Properties[ThreadContextKey.BatchId];
            }

            return null;
        }

        private static TraceEvent BuildTraceEvent(LoggingEvent loggingEvent, Guid? correlationId, Guid? batchId, string source)
        {
            TraceEvent traceEvent = new TraceEvent();
            traceEvent.BatchId = batchId;
            traceEvent.CorrelationId = correlationId;
            traceEvent.Exception = loggingEvent.ExceptionObject;
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

        private static void SendTraceEventForLogginEvent(LoggingEvent loggingEvent, Guid? correlationId, Guid? batchId, Uri tracerHubUri, string source)
        {
            TraceEvent traceEvent = BuildTraceEvent(loggingEvent, correlationId, batchId, source);
            HubConnection hubConnection = new HubConnection(tracerHubUri.AbsoluteUri);
            IHubProxy hubProxy = hubConnection.CreateProxy(TacerHubName);
            hubConnection.Start().Wait();
            hubProxy.Invoke(TracerHubMethodName, traceEvent);
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
