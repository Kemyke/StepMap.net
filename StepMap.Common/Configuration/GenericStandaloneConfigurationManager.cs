using StepMap.Logger.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.Common.Configuration
{
    /// <summary>
    /// Configuration manager for standalone environment 
    /// </summary>
    public class GenericStandaloneConfigurationManager<T> : IConfigurationManager<T>
    {
        private ILogger logger;
        private string configSectionName = null;

        public T Config { get; internal set; }

        internal string SectionName
        {
            get
            {
                return configSectionName;
            }
        }

        public GenericStandaloneConfigurationManager(ILogger logger)
        {
            if(logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            this.logger = logger;
        }

        public void LoadConfiguation()
        {
            configSectionName = typeof(T).Name;

            var config = (T)ConfigurationManager.GetSection(configSectionName);
            if (config == null)
            {
                throw new ConfigurationErrorsException(string.Format("{0} section not found!", configSectionName));
            }

            logger.Debug("{0} section is loaded successfully", configSectionName);
            Config = config;
        }
    }
}
