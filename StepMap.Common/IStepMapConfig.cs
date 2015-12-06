using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.Common
{
    public interface IStepMapConfig
    {
        string NotificationAccount { get; }
        string GmailClientId { get; }
        string GmailApiClientSecret { get; }
        string ClientBaseAddress { get; }
    }
}
