using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.Common.Exceptions
{
    [Serializable]
    public class AccountIsNotActivatedException : Exception
    {
        public AccountIsNotActivatedException() { }
        public AccountIsNotActivatedException(string message) : base(message) { }
        public AccountIsNotActivatedException(string message, Exception inner) : base(message, inner) { }
        protected AccountIsNotActivatedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
