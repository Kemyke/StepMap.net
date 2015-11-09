using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web;

namespace StepMap.WebHost
{
    public class CustomHostContractBehavior: IContractBehavior
    {
        private readonly IInstanceProvider instanceProvider;

        public CustomHostContractBehavior(IInstanceProvider instanceProvider)
        {
            if (instanceProvider == null)
            {
                throw new ArgumentNullException("instanceProvider");
            }

            this.instanceProvider = instanceProvider;
        }

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            dispatchRuntime.InstanceProvider = instanceProvider;
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }
    }
}