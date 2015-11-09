using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Web;

namespace StepMap.WebHost
{
    public class CustomHost: ServiceHost
    {
        protected UnityContainer container = null;

        public CustomHost(UnityContainer container, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            Initialize(container);
        }

        public CustomHost(UnityContainer container, object instance, params Uri[] baseAddresses)
            : base(instance, baseAddresses)
        {
            Initialize(container);
        }

        private void Initialize(UnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;

            ApplyServiceBehaviors(container);

            ApplyContractBehaviors(container);

            foreach (var contractDescription in ImplementedContracts.Values)
            {
                var contractBehavior = new CustomHostContractBehavior(new CustomHostInstanceProvider(container, contractDescription.ContractType));

                contractDescription.Behaviors.Add(contractBehavior);
            }
        }

        private void ApplyContractBehaviors(UnityContainer container)
        {
            var registeredContractBehaviors = container.ResolveAll<IContractBehavior>();

            foreach (var contractBehavior in registeredContractBehaviors)
            {
                foreach (var contractDescription in ImplementedContracts.Values)
                {
                    contractDescription.Behaviors.Add(contractBehavior);
                }
            }
        }

        private void ApplyServiceBehaviors(UnityContainer container)
        {
            var registeredServiceBehaviors = container.ResolveAll<IServiceBehavior>();

            foreach (var serviceBehavior in registeredServiceBehaviors)
            {
                Description.Behaviors.Add(serviceBehavior);
            }
        }
    }

}