using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;

namespace StepMap.WebHost
{
    public class CustomHostFactory: ServiceHostFactory
    {
        static CustomHostFactory()
        {
            container = new UnityContainer();
        }

        private static Dictionary<Type, object> cache = new Dictionary<Type,object>();
        private static UnityContainer container = null;
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
                        service = container.Resolve(serviceType);
                        cache.Add(serviceType, service);
                    }
                }
            }

            return new CustomHost(container, service, baseAddresses);
        }

        protected virtual void Initialize(UnityContainer container) { container.LoadConfiguration(); }
    }  

}