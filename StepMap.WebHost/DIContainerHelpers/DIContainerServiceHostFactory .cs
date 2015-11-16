using StepMap.Common.DIContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.WebHost.DIContainerHelpers
{
    /// <summary>
    /// Always retrieves an instance from DI container. 
    /// DO NOT use this factory if your service object defined as InstanceContextMode.Single! In this case use DIContainerSingleServiceHostFactory!
    /// 
    /// If it is declared Singleton in DIContainer config it behaves like you define InstanceContextMode.Single.
    /// If it is not Singleton a new service instance will be created each time.
    /// </summary>
    public class DIContainerServiceHostFactory : ServiceHostFactory 
    {
        private DIContainerFactory factory = new DIContainerFactory();

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            IDIContainer container = factory.CreateAndLoadDIContainer();
            Initialize(container);
            return new DIContainerServiceHost(container, serviceType, baseAddresses);
        }

        protected virtual void Initialize(IDIContainer container) { }
    }  
}
