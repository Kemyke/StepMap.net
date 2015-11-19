using StepMap.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.BusinessLogic
{
    public interface IUserManager
    {
        bool IsPasswordValid(string userName, string pwdHash);
        User GetUser(string userName);
        void Register(string userName, string email, string pwdHash);
    }
}
