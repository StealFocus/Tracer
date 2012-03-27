namespace StealFocus.Tracer.Tests.L4N
{
    using System;

    using log4net;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TracerAppenderTests
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [TestMethod]
        public void TestSingleLog()
        {
            Context.SetCorrelationId(Guid.NewGuid());
            Context.SetBatchId(Guid.NewGuid());
            log.Error("Some error.");
        }

        [TestMethod]
        public void TestMultipleLogs()
        {
            Context.SetCorrelationId(Guid.NewGuid());
            Context.SetBatchId(Guid.NewGuid());
            for (int i = 0; i < 20; i++)
            {
                log.Error("Some error.");   
            }
        }
    }
}
