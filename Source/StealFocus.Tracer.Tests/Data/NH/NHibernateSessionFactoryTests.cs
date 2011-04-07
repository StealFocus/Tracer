// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NHibernateSessionFactoryTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the NHibernateSessionFactoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.Tracer.Tests.Data.NH
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using NHibernate;

    using StealFocus.Tracer.Data.NH;

    [TestClass]
    public class NHibernateSessionFactoryTests
    {
        [TestMethod]
        public void IntegrationTestCreate()
        {
            using (NHibernateSessionFactory.Create())
            {
            }
        }

        [TestMethod]
        public void IntegrationTestSaveEntity()
        {
            using (CallContext callContext = CallContext.Create("some info"))
            {
                using (ISession session = NHibernateSessionFactory.Create())
                {
                    session.Save(callContext);
                    session.Flush();
                    session.Close();
                }
            }
        }
    }
}
