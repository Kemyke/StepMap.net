using StepMap.Common.DIContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.WebHost.DIContainerHelpers
{
    public class DIContainerInstanceProvider : IInstanceProvider
    {
        private readonly IDIContainer container;
        private readonly Type contractType;

        public DIContainerInstanceProvider(IDIContainer container, Type contractType)
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
            return container.GetInstance(contractType);
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
