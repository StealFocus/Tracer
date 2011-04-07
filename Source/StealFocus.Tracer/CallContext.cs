// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CallContext.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the CallContext type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.Tracer
{
    using System;
    using System.Collections.Generic;

    public class CallContext : IDisposable
    {
        private static readonly Stack<CallContext> callContextStack = new Stack<CallContext>();

        protected CallContext()
        {
        }

        private CallContext(string info)
        {
            this.Info = info;
            callContextStack.Push(this);
        }

        ~CallContext() 
        {
            this.Dispose(false);
        }

        public static CallContext Current
        {
            get
            {
                if (callContextStack.Count == 0)
                {
                    return null;
                }

                return callContextStack.Peek();
            }
        }

        public virtual int Id { get; set; }

        public virtual string Info { get; private set; }

        public static CallContext Create(string info)
        {
            return new CallContext(info);
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion // Implementation of IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                callContextStack.Pop();
            }
        }
    }
}
