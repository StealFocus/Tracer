// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CallContextTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the CallContextTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.Tracer.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CallContextTests
    {
        [TestMethod]
        public void Test()
        {
            using (CallContext.Create("first"))
            {
                Console.WriteLine(CallContext.Current.Info);
                using (CallContext.Create("second"))
                {
                    Console.WriteLine(CallContext.Current.Info);
                    using (CallContext.Create("third"))
                    {
                        Console.WriteLine(CallContext.Current.Info);
                        using (CallContext.Create("fourth"))
                        {
                            Console.WriteLine(CallContext.Current.Info);
                        }
                    }
                }
            }
        }
    }
}
