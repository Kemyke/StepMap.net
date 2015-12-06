using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.Common
{
    [DataContract]
    public enum ResultCode
    {
        [EnumMember]
        OK = 0,
        [EnumMember]
        INVALID_EMAILADDRESS = 1,
        [EnumMember]
        UNKOWN_USER = 2,
        [EnumMember]
        CONFIRMATION_GUID_NOT_VALID = 3,
        [EnumMember]
        ACCOUNT_IS_NOT_CONFIRMED = 4,
        [EnumMember]
        USER_ALREADY_EXISTS = 5,

        [EnumMember]
        UNKOWN_ERROR = 900,
    }
}
