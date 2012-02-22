namespace StealFocus.Tracer.Web.UI
{
    using System;

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

            System.Diagnostics.Debug.WriteLine("TEST!!!!!!" + traceEvent.Message);

            // Call the addMessage method on all clients
            Clients.addMessage(traceEvent.Message);
        }
    }
}