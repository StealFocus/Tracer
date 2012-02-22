namespace StealFocus.Tracer.L4N
{
    using System;
    using System.Text;

    using log4net;
    using log4net.Appender;
    using log4net.Core;

    using SignalR.Client.Hubs;

    using StealFocus.Tracer.Configuration;
    using StealFocus.Tracer.Model;

    public class TracerAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }

            try
            {
                TraceEvent traceEvent = new TraceEvent();
                if (ThreadContext.Properties[ThreadContextKey.BatchId] != null)
                {
                    traceEvent.BatchId = (Guid)ThreadContext.Properties[ThreadContextKey.BatchId];
                }

                if (ThreadContext.Properties[ThreadContextKey.CorrelationId] != null)
                {
                    traceEvent.CorrelationId = (Guid)ThreadContext.Properties[ThreadContextKey.CorrelationId];
                }

                traceEvent.Exception = loggingEvent.ExceptionObject;
                traceEvent.Message = loggingEvent.RenderedMessage;
                traceEvent.Source = "????????????";
                traceEvent.SourceCode = new SourceCode();
                traceEvent.SourceCode.ClassName = loggingEvent.LocationInformation.ClassName;
                traceEvent.SourceCode.FileName = loggingEvent.LocationInformation.FileName;
                traceEvent.SourceCode.LineNumber = loggingEvent.LocationInformation.LineNumber;
                traceEvent.SourceCode.MethodName = loggingEvent.LocationInformation.MethodName;
                traceEvent.Timestamp = loggingEvent.TimeStamp;
                traceEvent.UserName = loggingEvent.UserName;

                HubConnection hubConnection = new HubConnection("http://localhost:8000/");
                IHubProxy hubProxy = hubConnection.CreateProxy("StealFocus.Tracer.Web.UI.TraceHub");
                hubConnection.Start().Wait();
                int i = 0;
                while (i < 10)
                {
                    hubProxy.Invoke("Send", traceEvent);
                    System.Threading.Thread.Sleep(1000);
                    i++;
                }
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
    }
}
