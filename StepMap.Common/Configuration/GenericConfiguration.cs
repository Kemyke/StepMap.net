using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Web.Configuration;
using StepMap.Logger.Logging;
using StepMap.Common.DIContainer;

namespace StepMap.Common.Configuration
{
    [Obsolete]
    public static class GenericConfiguration<T> where T : ConfigurationSection
    {
        private static GenericStandaloneConfigurationManager<T> standaloneConfig = null;
        private static ILogger logger = null;

        static GenericConfiguration()
        {
            DIContainerFactory diFactory = new DIContainerFactory();
            IDIContainer diContainer = diFactory.CreateDIContainer();
            logger = diContainer.GetInstance<ILogger>();
            standaloneConfig = new GenericStandaloneConfigurationManager<T>(logger);
            standaloneConfig.LoadConfiguation();
        }

        public static T Settings
        {
            get
            {
                return standaloneConfig.Config;
            }
        }

        public static Boolean ReloadSectionOnChangeEnabled
        {
            get { return reloadSectionOnChangeEnabled; }
            set { SetReloadSectionOnChangeEnabled(value); }
        }

        private static bool reloadSectionOnChangeEnabled;
        private static Action<Exception> ExceptionHandler;
        private static Action<string> MessageHandler;
        private static FileSystemWatcher changeWatcher;

        public static void SetReloadSectionOnChangeEnabled(Boolean value, Action<Exception> exceptionHandler = null, Action<String> messageHandler = null)
        {
            logger.Debug("GenericConfiguration<{0}> SetReloadSectionOnChangeEnabled", standaloneConfig.SectionName);

            GenericConfiguration<T>.ExceptionHandler = exceptionHandler;
            GenericConfiguration<T>.MessageHandler = messageHandler;

            if (value && !reloadSectionOnChangeEnabled)
            {
                if (Settings == null)
                {
                    throw new ConfigurationErrorsException(String.Format("Config section '{0}' is not loaded!", standaloneConfig.SectionName));
                }

                if (changeWatcher == null)
                {
                    String path = String.Format("{0}\\{1}", Path.GetDirectoryName(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath), Settings.SectionInformation.ConfigSource);
                    changeWatcher = new FileSystemWatcher(Path.GetDirectoryName(path), Path.GetFileName(path));
                    changeWatcher.Changed += new FileSystemEventHandler(SectionChangedHandler);
                }

                changeWatcher.EnableRaisingEvents = true;
                logger.Debug("Config section '{0}' is set to reload on change.", standaloneConfig.SectionName);
            }
            else if (!value && reloadSectionOnChangeEnabled)
            {
                if (changeWatcher != null)
                {
                    changeWatcher.EnableRaisingEvents = false;
                }
                logger.Debug("Config section '{0}' is set to not reload on change.", standaloneConfig.SectionName);
            }

            reloadSectionOnChangeEnabled = value;
        }

        private static void SectionChangedHandler(Object source, FileSystemEventArgs e)
        {
            logger.Debug("GenericConfiguration<{0}> SectionChangedHandler", standaloneConfig.SectionName);

            try
            {
                ConfigurationManager.RefreshSection(standaloneConfig.SectionName);
                T newSettings = ConfigurationManager.GetSection(standaloneConfig.SectionName) as T;
                if (newSettings == null)
                {
                    HandleSectionChangedError();
                }
                else
                {
                    standaloneConfig.Config = newSettings;
                    string message = string.Format("Config section '{0}' is reloaded successfully!", standaloneConfig.SectionName);
                    logger.Debug(message);
                    Action<String> MessageHandler = GenericConfiguration<T>.MessageHandler;
                    if (MessageHandler != null)
                    {
                        MessageHandler(message);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Debug(ex.ToString());
                try
                {
                    HandleSectionChangedError(ex);
                }
                catch (Exception innerEx)
                {
                    logger.Debug(innerEx.ToString());
                }
            }
        }

        private static void HandleSectionChangedError(Exception ex = null)
        {
            string message = string.Format("Config section '{0}' is not reloaded!", standaloneConfig.SectionName);
            logger.Debug(message);
            Action<Exception> ExceptionHandler = GenericConfiguration<T>.ExceptionHandler;
            if (ExceptionHandler != null)
            {
                ExceptionHandler(new ConfigurationErrorsException(message, ex));
            }
        }
    }
}
