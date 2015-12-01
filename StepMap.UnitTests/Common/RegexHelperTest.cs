using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StepMap.Common.RegexHelpers;
using StepMap.Logger.Logging;
using Moq;

namespace StepMap.UnitTests.Common
{
    [TestClass]
    public class RegexHelperTest
    {
        [TestMethod]
        public void EmailValidatorTest1()
        {
            Mock<ILogger> logger = new Mock<ILogger>();
            RegexHelper helper = new RegexHelper(logger.Object);
            
            bool ret = helper.IsValidEmail("testmail@gmail.com");

            Assert.IsTrue(ret);
        }

        [TestMethod]
        public void EmailValidatorTest2()
        {
            Mock<ILogger> logger = new Mock<ILogger>();
            RegexHelper helper = new RegexHelper(logger.Object);

            bool ret = helper.IsValidEmail("testmail@gmail.c");

            Assert.IsFalse(ret);
        }

        [TestMethod]
        public void EmailValidatorTest3()
        {
            Mock<ILogger> logger = new Mock<ILogger>();
            RegexHelper helper = new RegexHelper(logger.Object);

            bool ret = helper.IsValidEmail("testmail@gmail");

            Assert.IsFalse(ret);
        }

        [TestMethod]
        public void EmailValidatorTest4()
        {
            Mock<ILogger> logger = new Mock<ILogger>();
            RegexHelper helper = new RegexHelper(logger.Object);

            bool ret = helper.IsValidEmail("testmailgmail.com");

            Assert.IsFalse(ret);
        }
    }
}
