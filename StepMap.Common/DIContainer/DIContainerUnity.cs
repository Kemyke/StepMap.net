using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StepMap.Common.DIContainer
{
    public class DIContainerUnity : IDIContainer
    {
        private IUnityContainer unityContainer = null;

        public DIContainerUnity()
        {
            unityContainer = new UnityContainer();
        }

        private DIContainerUnity(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;
        }

        private bool isInitialized = false;
        private bool isInitializedFromConfig = false;
        private ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

        public TType GetInstance<TType>()
        {
            CheckIsInitialized();
            var ret = unityContainer.Resolve<TType>();
            return ret;
        }

        public TType GetInstance<TType>(string name)
        {
            CheckIsInitialized();
            var ret = unityContainer.Resolve<TType>(name);
            return ret;
        }

        public object GetInstance(Type t)
        {
            CheckIsInitialized();
            var ret = unityContainer.Resolve(t);
            return ret;
        }

        public object GetInstance(Type t, string name)
        {
            CheckIsInitialized();
            var ret = unityContainer.Resolve(t, name);
            return ret;
        }

        public IEnumerable<TType> GetAllInstance<TType>()
        {
            CheckIsInitialized();
            var ret = unityContainer.ResolveAll<TType>();   
            return ret;
        }

        public bool IsRegistered<TInterface>()
        {
            CheckIsInitialized();
            return unityContainer.IsRegistered<TInterface>();
        }

        public bool IsRegistered<TInterface>(string name)
        {
            CheckIsInitialized();
            return unityContainer.IsRegistered<TInterface>(name);
        }

        public void RegisterType<TInterface, TType>() where TType : TInterface
        {
            RegisterTypeInternal<TInterface, TType>(null, null, null);
        }

        public void RegisterType<TInterface, TType>(string name) where TType : TInterface
        {
            RegisterTypeInternal<TInterface, TType>(name, null, null);
        }

        public void RegisterType<TInterface, TType>(params object[] constructorParams) where TType : TInterface
        {
            RegisterTypeInternal<TInterface, TType>(null, null, constructorParams);
        }

        public void RegisterType<TInterface, TType>(string name, params object[] constructorParams) where TType : TInterface
        {
            RegisterTypeInternal<TInterface, TType>(name, null, constructorParams);
        }

        public void RegisterType<TInterface, TType>(LifetimeTypes lifetimeType) where TType : TInterface
        {
            RegisterTypeInternal<TInterface, TType>(null, lifetimeType, null);
        }

        public void RegisterType<TInterface, TType>(string name, LifetimeTypes lifetimeType) where TType : TInterface
        {
            RegisterTypeInternal<TInterface, TType>(name, lifetimeType, null);
        }

        public void RegisterType<TInterface, TType>(LifetimeTypes lifetimeType, params object[] constructorParams) where TType : TInterface
        {
            RegisterTypeInternal<TInterface, TType>(null, lifetimeType, constructorParams);
        }

        public void RegisterType<TInterface, TType>(string name, LifetimeTypes lifetimeType, params object[] constructorParams) where TType : TInterface
        {
            RegisterTypeInternal<TInterface, TType>(name, lifetimeType, constructorParams);
        }

        private void RegisterTypeInternal<TInterface, TType>(string name, LifetimeTypes? lifetimeType, object[] constructorParams) where TType : TInterface
        {
            SetInitialized();

            LifetimeManager lm = GetLifetimeManagerByLifetimeType(lifetimeType == null ? LifetimeTypes.PerCall : lifetimeType.Value);

            if (constructorParams == null)
            {
                unityContainer.RegisterType<TInterface, TType>(name, lm);
            }
            else
            {
                InjectionConstructor ic = new InjectionConstructor(constructorParams);
                unityContainer.RegisterType<TInterface, TType>(name, lm, ic);
            }

        }

        public void RegisterInstance<TInterface>(TInterface instance)
        {
            if(instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            
            RegisterInstanceInternal<TInterface>(null, instance);
        }

        public void RegisterInstance<TInterface>(string name, TInterface instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            
            RegisterInstanceInternal<TInterface>(name, instance);
        }

        private void RegisterInstanceInternal<TInterface>(string name, TInterface instance)
        {
            SetInitialized();

            if(string.IsNullOrEmpty(name))
            {
                unityContainer.RegisterInstance<TInterface>(instance);
            }
            else
            {
                unityContainer.RegisterInstance<TInterface>(name, instance);
            }
        }

        public void LoadConfiguration()
        {
            try
            {
                rwLock.EnterWriteLock();
                if (isInitializedFromConfig)
                {
                    throw new InvalidOperationException(string.Format("DIContainer is already initialized from config file. Please do not call LoadConfiguration more than once! DIContainerUnity hashcode: {0}.", this.GetHashCode()));
                }


                isInitialized = true;
                isInitializedFromConfig = true;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
            unityContainer.LoadConfiguration();
        }

        public void CleanUp()
        {
            try
            {
                rwLock.EnterWriteLock();
                isInitialized = false;
                isInitializedFromConfig = false;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
            unityContainer.Dispose();
            unityContainer = new UnityContainer();
        }

        private LifetimeManager GetLifetimeManagerByLifetimeType(LifetimeTypes lifetimeType)
        {
            LifetimeManager ret;
            switch (lifetimeType)
            {
                case LifetimeTypes.PerCall:
                    ret = new TransientLifetimeManager();
                    break;
                case LifetimeTypes.Singleton:
                    ret = new ContainerControlledLifetimeManager();
                    break;
                default:
                    throw new NotSupportedException(lifetimeType.ToString());
            }
            return ret;
        }        

        public void Dispose()
        {
            unityContainer.Dispose();
        }

        private void SetInitialized()
        {
            try
            {
                rwLock.EnterWriteLock();
                isInitialized = true;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        private void CheckIsInitialized()
        {
            try
            {
                rwLock.EnterReadLock();
                if (!isInitialized)
                {
                    throw new InvalidOperationException(string.Format("DIContainer is not yet initialized! Please call LoadConfiguration to load config from configuration files or call one of the Register methods. DIContainerUnity hashcode: {0}.", this.GetHashCode()));
                }
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public IDIContainer CreateChildContainer()
        {
            var uc = unityContainer.CreateChildContainer();
            IDIContainer ret = new DIContainerUnity(uc);
            return ret;
        }
    }
}
