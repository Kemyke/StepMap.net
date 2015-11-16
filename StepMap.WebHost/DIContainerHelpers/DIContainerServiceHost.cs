using StepMap.Common.DIContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.WebHost.DIContainerHelpers
{
    public class DIContainerServiceHost : ServiceHost
    {
        protected IDIContainer container = null;

        public DIContainerServiceHost(IDIContainer container, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            Initialize(container);
        }

        public DIContainerServiceHost(IDIContainer container, object instance, params Uri[] baseAddresses)
            : base(instance, baseAddresses)
        {
            Initialize(container);
        }

        private void Initialize(IDIContainer container)
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
                var contractBehavior = new DIContainerContractBehavior(new DIContainerInstanceProvider(container, contractDescription.ContractType));

                contractDescription.Behaviors.Add(contractBehavior);
            }
        }

        private void ApplyContractBehaviors(IDIContainer container)
        {
            var registeredContractBehaviors = container.GetAllInstance<IContractBehavior>();

            foreach (var contractBehavior in registeredContractBehaviors)
            {
                foreach (var contractDescription in ImplementedContracts.Values)
                {
                    contractDescription.Behaviors.Add(contractBehavior);
                }
            }
        }

        private void ApplyServiceBehaviors(IDIContainer container)
        {
            var registeredServiceBehaviors = container.GetAllInstance<IServiceBehavior>();

            foreach (var serviceBehavior in registeredServiceBehaviors)
            {
                Description.Behaviors.Add(serviceBehavior);
            }
        }
    }
}
