namespace StealFocus.Tracer.Web.UI
{
    using System;
    using System.Globalization;

    using SignalR.Hubs;

    using StealFocus.Tracer.Model;

    [CLSCompliant(false)]
    [HubName("traceHub")]
    public class TraceHub : Hub
    {
        public void Send(TraceEvent traceEvent)
        {
            if (traceEvent == null)
            {
                System.Diagnostics.Debug.WriteLine("The received Trace Event was null.");
                throw new ArgumentNullException("traceEvent");
            }

            string diagnosticMessage;
            if (traceEvent.CorrelationId == null)
            {
                diagnosticMessage = string.Format(CultureInfo.CurrentCulture, "Received Trace Event from Source '{0}'.", traceEvent.Source);   
            }
            else
            {
                diagnosticMessage = string.Format(CultureInfo.CurrentCulture, "Received Trace Event from Source '{0}' with Correlation ID of '{1}'.", traceEvent.Source, traceEvent.CorrelationId);
            }

            System.Diagnostics.Debug.WriteLine(diagnosticMessage);

            // Send Trace Event to all clients.
            Clients.addTraceEvent(traceEvent);
        }
    }
}