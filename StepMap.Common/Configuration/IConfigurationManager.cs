using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.Common.Configuration
{
    /// <summary>
    /// Configuration manager abstraction.
    /// </summary>
    /// <typeparam name="T">Configuration section type</typeparam>
    public interface IConfigurationManager<out T>
    {
        /// <summary>
        /// The loaded configuration section
        /// </summary>
        T Config { get; }
        
        /// <summary>
        /// Loads the configuration section
        /// </summary>
        void LoadConfiguation();
    }
}
