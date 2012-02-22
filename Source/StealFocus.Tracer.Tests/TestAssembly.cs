namespace StealFocus.Tracer.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public static class TestAssembly
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            Assert.IsNotNull(typeof(Newtonsoft.Json.IJsonLineInfo));
        }
    }
}
