namespace StealFocus.Tracer.Web.UI.Test
{
    using System;

    using SignalR.Hubs;

    [CLSCompliant(false)]
    [HubName("myChat")]
    public class MyChat : Hub
    {
        public void Send(string message)
        {
            // Call the addMessage method on all clients
            Clients.addMessage(message);
        }
    }
}