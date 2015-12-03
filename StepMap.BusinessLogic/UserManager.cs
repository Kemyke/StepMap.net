using StepMap.Common.Exceptions;
using StepMap.Common.RegexHelpers;
using StepMap.DAL;
using StepMap.Logger.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.BusinessLogic
{
    public class UserManager : IUserManager
    {
        private readonly ILogger logger;
        private readonly IRegexHelper regexHelper;
        private readonly INotificationManager notificationManager;

        public UserManager(ILogger logger, IRegexHelper regexHelper, INotificationManager notificationManager)
        {
            this.logger = logger;
            this.regexHelper = regexHelper;
            this.notificationManager = notificationManager;
        }

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

        private void SendConfirmationEmail(User user)
        {
            using (var ctx = new StepMapDbContext())
            {
                UserConfirmation uc = new UserConfirmation();
                uc.User = user;
                uc.ConfirmationGuid = Guid.NewGuid().ToString();

                ctx.UserConfirmations.Add(uc);
                ctx.SaveChanges();
                
                string link = "http://www.stepmap.xyz/Account/ConfirmEmail/?guid=" + uc.ConfirmationGuid;
                //TODO: do not hardcode text
                notificationManager.SendEmail(user, "registration on stepmap.xyz", string.Format("you are registered on stepmap.xyz. please confirm your accout visiting this link: {0}!", link)); //LOCSTR
            }
        }

        public void Register(string userName, string email, string pwdHash)
        {
            bool isValid = regexHelper.IsValidEmail(email);
            if (isValid)
            {
                using (var ctx = new StepMapDbContext())
                {
                    User user = new User() { Name = userName, Email = email, PasswordHash = pwdHash, UserRole = UserRole.Member, UserState = UserState.NotActivatedYet };
                    ctx.Users.Add(user);
                    ctx.SaveChanges();

                    SendConfirmationEmail(user);
                }
            }
            else
            {
                logger.Warning("Attempt to register invalid email: {0}! Username: {1}.", email, userName);
            }
        }

        //TODO: add user state and role
        public void Login(User user)
        {
            using (var ctx = new StepMapDbContext())
            {
                user = ctx.Users.Attach(user);
                user.LastLogin = DateTime.UtcNow;
                ctx.SaveChanges();
            }
        }

        public User ConfirmEmail(string guid)
        {
            User ret;
            using (var ctx = new StepMapDbContext())
            {
                UserConfirmation userConfirmation;
                try
                {
                    userConfirmation = ctx.UserConfirmations.Single(uc => uc.ConfirmationGuid == guid);
                }
                catch (InvalidOperationException ex)
                {
                    throw new ConfirmationGuidNotValidException(string.Format("Unkown guid: {0}!", guid), ex);
                }
                ret = userConfirmation.User;
                userConfirmation.User.UserState = UserState.Active;
                ctx.UserConfirmations.Remove(userConfirmation);
                ctx.SaveChanges();
            }
            return ret;
        }
    }
}
