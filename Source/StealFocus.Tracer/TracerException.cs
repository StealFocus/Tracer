namespace StealFocus.Tracer
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// TracerException Class.
    /// </summary>
    [Serializable]
    public class TracerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TracerException"/> class.
        /// </summary>
        public TracerException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TracerException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <remarks>
        /// None.
        /// </remarks>
        public TracerException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TracerException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">An <see cref="Exception"/>. The exception that is the cause of the current exception. If the <paramref name="innerException"/> parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        /// <remarks>
        /// None.
        /// </remarks>
        public TracerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TracerException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <remarks>
        /// None.
        /// </remarks>
        protected TracerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
