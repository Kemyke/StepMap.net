using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StepMap.BusinessLogic;
using StepMap.Common;
using StepMap.Common.RegexHelpers;
using StepMap.DAL;
using StepMap.Logger.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.UnitTests.BusinessLogic
{
    [TestClass]
    public class UserManagerTest
    {
        [TestMethod]
        public void UserManagerTest1()
        {
            Mock<ILogger> logger = new Mock<ILogger>();
            Mock<IStepMapConfig> config = new Mock<IStepMapConfig>();
            Mock<IRegexHelper> regexHelper = new Mock<IRegexHelper>();
            Mock<INotificationManager> notificationManager = new Mock<INotificationManager>();
            regexHelper.Setup(r => r.IsValidEmail(It.IsAny<string>())).Returns(true);
            UserManager um = new UserManager(logger.Object, config.Object, regexHelper.Object, notificationManager.Object);
            um.Register("test user", "test@test.com", "hash");

            using (var ctx = new StepMapDbContext())
            {
                User u = ctx.Users.Single(tu => tu.Email == "test@test.com");
                try
                {
                    Assert.AreEqual("test user", u.Name);
                    Assert.AreEqual("test@test.com", u.Email);
                    Assert.AreEqual("hash", u.PasswordHash);
                }
                finally
                {
                    ctx.Users.Remove(u);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
