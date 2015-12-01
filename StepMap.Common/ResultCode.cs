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
        UNKOWN_ERROR = 900,
    }
}
