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
    /// Creates singleton instaces of the service. 
    /// Equals to InstanceContextMode.Single
    /// </summary>
    public class DIContainerSingleServiceHostFactory : ServiceHostFactory
    {
        static DIContainerSingleServiceHostFactory()
        {
            DIContainerFactory factory = new DIContainerFactory();
            container = factory.CreateAndLoadDIContainer();
        }

        private static Dictionary<Type, object> cache = new Dictionary<Type,object>();
        private static IDIContainer container = null;
        private static bool isContainerInitialized = false;

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            object service;
            if(!cache.TryGetValue(serviceType, out service))
            {
                lock(cache)
                {
                    if (!isContainerInitialized)
                    {
                        Initialize(container);
                        isContainerInitialized = true;
                    }
                    if(!cache.TryGetValue(serviceType, out service))
                    {
                        service = container.GetInstance(serviceType);
                        cache.Add(serviceType, service);
                    }
                }
            }

            return new DIContainerServiceHost(container, service, baseAddresses);
        }

        protected virtual void Initialize(IDIContainer container) { }
    }  
}
