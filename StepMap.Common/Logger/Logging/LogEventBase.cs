using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.Logger.Logging
{
    /// <summary>
    /// Base class for logging custom data in a key-value pair format.
    /// </summary>
    public abstract class LogEventBase
    {
        /// <summary>
        /// Date and time of creation.
        /// </summary>
        public DateTime Created { get; private set; }

        /// <summary>
        /// Date and time of logging.
        /// </summary>
        public DateTime Logged { get; set; }

        public LogEventBase()
        {
            Created = DateTime.UtcNow;
        }
    }
}
