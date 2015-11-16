using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.Common.DIContainer
{
    /// <summary>
    /// Lifetime types for an instance
    /// </summary>
    public enum LifetimeTypes
    {
        /// <summary>
        /// DI container always returns a new instance
        /// </summary>
        PerCall,
        /// <summary>
        /// DI container always returns the same instance
        /// </summary>
        Singleton,
    }
}
