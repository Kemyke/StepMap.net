using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StepMap.BusinessLogic;
using StepMap.Logger.Logging;
using Moq;
using StepMap.DAL;
using System.Threading.Tasks;
using StepMap.Common;

namespace StepMap.UnitTests.BusinessLogic
{
    /// <summary>
    /// Summary description for NotificationManagerTests
    /// </summary>
    [TestClass]
    public class NotificationManagerTests
    {
        public NotificationManagerTests()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        [Ignore]
        public void TestMethod1()
        {
            var logger = new Mock<ILogger>();
            var config = new Mock<IStepMapConfig>();
            config.Setup(x => x.NotificationAccount).Returns("stepmap.daemon@gmail.com");
            config.Setup(x => x.GmailClientId).Returns("");
            config.Setup(x => x.GmailApiClientSecret).Returns("");

            NotificationManager nm = new NotificationManager(logger.Object, config.Object);
            User user = new User{ Email = "kemyyy@gmail.com", Name = "kemy" };
            nm.SendEmail(user, "Unit test", "Test text");
        }
    }
}
