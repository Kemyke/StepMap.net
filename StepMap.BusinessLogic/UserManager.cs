using StepMap.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.BusinessLogic
{
    public class UserManager : IUserManager
    {
        public bool IsPasswordValid(string userName, string pwdHash)
        {
            using (var ctx = new StepMapDbContext())
            {
                var user = ctx.Users.Where(u => u.Name == userName && u.PasswordHash == pwdHash).SingleOrDefault();
                return user != null;
            }
        }

        public DAL.User GetUser(string userName)
        {
            using (var ctx = new StepMapDbContext())
            {
                var user = ctx.Users.Where(u => u.Name == userName).SingleOrDefault();
                if (user == null)
                {
                    throw new InvalidOperationException(string.Format("User not found: {0}!", userName));
                }
                else
                {
                    return user;
                }
            }
        }

        public void Register(string userName, string email, string pwdHash)
        {
            using (var ctx = new StepMapDbContext())
            {
                User user = new User() { Name = userName, Email = email, PasswordHash = pwdHash };
                ctx.Users.Add(user);
                ctx.SaveChanges();
            }
        }


        public void Login(User user)
        {
            using (var ctx = new StepMapDbContext())
            {
                user = ctx.Users.Attach(user);
                user.LastLogin = DateTime.UtcNow;
                ctx.SaveChanges();
            }
        }
    }
}
