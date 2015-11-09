using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Web;

namespace StepMap.WebHost
{
    public class CustomHostInstanceProvider: IInstanceProvider
    {
        private readonly UnityContainer container;
        private readonly Type contractType;

        public CustomHostInstanceProvider(UnityContainer container, Type contractType)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            if (contractType == null)
            {
                throw new ArgumentNullException("contractType");
            }

            this.container = container;
            this.contractType = contractType;
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return container.Resolve(contractType);
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
        }
    }

}