namespace StealFocus.Tracer
{
    using System;

    using log4net;

    using StealFocus.Tracer.Configuration;

    public static class Context
    {
        public static void SetCorrelationId(Guid correlationId)
        {
            ThreadContext.Properties[ThreadContextKey.CorrelationId] = correlationId;
        }

        public static void SetBatchId(Guid batchId)
        {
            ThreadContext.Properties[ThreadContextKey.BatchId] = batchId;
        }
    }
}
