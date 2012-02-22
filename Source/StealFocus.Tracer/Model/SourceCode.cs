namespace StealFocus.Tracer.Model
{
    using System;

    [Serializable]
    public class SourceCode
    {
        public string ClassName { get; set; }

        public string MethodName { get; set; }

        public string FileName { get; set; }

        public string LineNumber { get; set; }
    }
}
