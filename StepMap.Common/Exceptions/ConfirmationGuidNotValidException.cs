using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.Common.Exceptions
{
    [Serializable]
    public class ConfirmationGuidNotValidException : Exception
    {
        public ConfirmationGuidNotValidException() { }
        public ConfirmationGuidNotValidException(string message) : base(message) { }
        public ConfirmationGuidNotValidException(string message, Exception inner) : base(message, inner) { }
        protected ConfirmationGuidNotValidException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
