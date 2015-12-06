using StepMap.Common;
using StepMap.Common.Configuration;
using StepMap.Common.DIContainer;
using StepMap.WebHost.DIContainerHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StepMap.WebHost
{
    public class StepMapHostFactory : DIContainerSingleServiceHostFactory
    {
        protected override void Initialize(IDIContainer container)
        {
            base.Initialize(container);

            var cm = container.GetInstance<IConfigurationManager<IStepMapConfig>>();
            cm.LoadConfiguation();
            //TODO: hack
            ((StepMapConfig)cm.Config).AppPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            container.RegisterInstance(cm.Config);
        }
    }
}