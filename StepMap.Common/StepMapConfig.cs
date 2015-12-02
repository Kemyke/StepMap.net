using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.Common
{
    public sealed class StepMapConfig : ConfigurationSection, IStepMapConfig
    {
        [ConfigurationProperty("GmailApiClientSecret", IsRequired = true)]
        public string GmailApiClientSecret
        {
            get { return (string)this["GmailApiClientSecret"]; }
            set { this["GmailApiClientSecret"] = value; }
        }

        [ConfigurationProperty("GmailClientId", IsRequired = true)]
        public string GmailClientId
        {
            get { return (string)this["GmailClientId"]; }
            set { this["GmailClientId"] = value; }
        }

        [ConfigurationProperty("NotificationAccount", IsRequired = true)]
        public string NotificationAccount
        {
            get { return (string)this["NotificationAccount"]; }
            set { this["NotificationAccount"] = value; }
        }
    }
}
