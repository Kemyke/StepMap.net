using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

namespace StepMap.Common.Configuration
{
    public class ConfigurationCollection<T> : ConfigurationElementCollection
        where T : ConfigurationElement, IConfigurationElement, new()
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IConfigurationElement)element).GetElementKey;
        }

        public T this[int index]
        {
            get
            {
                return (T)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }
    }
}
