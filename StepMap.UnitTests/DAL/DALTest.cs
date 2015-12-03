using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StepMap.DAL;

namespace StepMap.UnitTests.DAL
{
    [TestClass]
    public class DALTest
    {
        [TestMethod]
        public void DALTest1()
        {
            using (StepMapDbContext ctx = new StepMapDbContext())
            {
                User u = new User();
                u.Email = "test@test.com";
                u.LastLogin = DateTime.UtcNow;
                u.Name = "test user";
                u.PasswordHash = "hash";
                u.UserRole = UserRole.Member;
                u.UserState = UserState.Active;

                u = ctx.Users.Add(u);
                ctx.SaveChanges();
                try
                {
                    User du = ctx.Users.Single(tu => tu.Email == "test@test.com");
                    Assert.AreEqual(u.LastLogin, du.LastLogin);
                    Assert.AreEqual(u.Name, du.Name);
                    Assert.AreEqual(u.PasswordHash, du.PasswordHash);
                    Assert.AreEqual(u.UserRole, du.UserRole);
                    Assert.AreEqual(u.UserState, du.UserState);
                }
                finally
                {
                    ctx.Users.Remove(u);
                    ctx.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void DALTest2()
        {
            using (StepMapDbContext ctx = new StepMapDbContext())
            {
                User u = new User();
                u.Email = "test@test.com";
                u.LastLogin = DateTime.UtcNow;
                u.Name = "test user";
                u.PasswordHash = "hash";
                u.UserRole = UserRole.Member;
                u.UserState = UserState.Active;

                Project p = new Project();
                p.Name = "test project";
                p.User = u;

                Step s = new Step();
                s.Name = "test step name";
                s.Project = p;

                Reminder r = new Reminder();
                r.Step = s;
                r.EmailAddress = "test@test.com";
                r.Message = "test message";
                r.SentDate = DateTime.UtcNow;
                r.Subject = "test subject";

                ctx.Projects.Add(p);
                ctx.Steps.Add(s);
                ctx.Reminders.Add(r);

                ctx.SaveChanges();

                try
                {
                    Project dp = ctx.Projects.Single(tu => tu.Name == "test project");
                    Assert.AreEqual(p.UserId, dp.UserId);
                    Step ds = dp.FinishedSteps.Single();
                    Assert.AreEqual(s.Name, ds.Name);
                    Reminder dr = ds.SentReminders.Single();
                    Assert.AreEqual(dr.EmailAddress, r.EmailAddress);
                    Assert.AreEqual(dr.Message, r.Message);
                    Assert.AreEqual(dr.SentDate, r.SentDate);
                    Assert.AreEqual(dr.Subject, r.Subject);
                }
                finally
                {
                    s.SentReminders.Clear();
                    p.FinishedSteps.Clear();
                    ctx.Reminders.Remove(r);
                    ctx.Steps.Remove(s);
                    ctx.Projects.Remove(p);
                    ctx.Users.Remove(u);
                    ctx.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void DALTest3()
        {
            using (StepMapDbContext ctx = new StepMapDbContext())
            {
                User u = new User();
                u.Email = "test@test.com";
                u.LastLogin = DateTime.UtcNow;
                u.Name = "test user";
                u.PasswordHash = "hash";
                u.UserRole = UserRole.Member;
                u.UserState = UserState.Active;

                u = ctx.Users.Add(u);
                ctx.SaveChanges();

                UserConfirmation uc = new UserConfirmation();
                uc.User = u;
                uc.ConfirmationGuid = "guid";
                uc = ctx.UserConfirmations.Add(uc);
                ctx.SaveChanges();

                try
                {
                    UserConfirmation du = ctx.UserConfirmations.Single(tu => tu.ConfirmationGuid == "guid");
                    Assert.AreEqual(u.Id, du.User.Id);
                }
                finally
                {
                    ctx.UserConfirmations.Remove(uc);
                    ctx.Users.Remove(u);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
