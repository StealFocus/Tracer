namespace StealFocus.Tracer.L4N
{
    using System;

    using log4net.Core;

    public delegate void SendTraceEventForLoggingEvent(LoggingEvent loggingEvent, Guid? correlationId, Guid? batchId, Uri tracerHubUri, string source);
}
