using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dto = StepMap.ServiceContracts.DTO;
using dal = StepMap.DAL;

namespace StepMap.ServiceImpl.Converters
{
    public static class UserConverter
    {
        public static dto.User ConvertUser(dal.User user)
        {
            dto.User ret = new dto.User();
            ret.Id = user.Id;
            ret.Name = user.Name;
            ret.Email = user.Email;
            return ret;
        }

        public static dal.User ConvertUser(dto.User user)
        {
            dal.User ret = new dal.User();
            ret.Id = user.Id;
            ret.Name = user.Name;
            ret.Email = user.Email;
            return ret;
        }

    }
}
