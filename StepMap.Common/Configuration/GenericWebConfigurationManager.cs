using StepMap.Logger.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace StepMap.Common.Configuration
{
    /// <summary>
    /// Configuration manager for web hosted environment 
    /// </summary>
    public class GenericWebConfigurationManager<T> : IConfigurationManager<T>
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

        public GenericWebConfigurationManager(ILogger logger)
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

            var config = (T)WebConfigurationManager.GetSection(configSectionName);
            if (config == null)
            {
                throw new ConfigurationErrorsException(string.Format("{0} section not found!", configSectionName));
            }

            logger.Debug("{0} section is loaded successfully", configSectionName);
            Config = config;
        }
    }
}
