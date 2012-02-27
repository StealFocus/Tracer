namespace StealFocus.Tracer.Model
{
    using System;

    [Serializable]
    public class TraceEvent
    {
        public Guid? CorrelationId { get; set; }

        public Guid? BatchId { get; set; }

        public string Source { get; set; }

        public string UserName { get; set; }

        public DateTime Timestamp { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }

        public SourceCode SourceCode { get; set; }
    }
}
