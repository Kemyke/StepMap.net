using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.Common.DIContainer
{
    /// <summary>
    /// Abstraction of a dependency injection container.
    /// </summary>
    public interface IDIContainer : IDisposable
    {
        /// <summary>
        /// Returns an instance of a class which implements the given interface.
        /// </summary>
        TType GetInstance<TType>();

        /// <summary>
        /// Returns an instance of a class which implements the given interface.
        /// </summary>
        /// <param name="name">The name of the registration.</param>
        TType GetInstance<TType>(string name);

        /// <summary>
        /// Returns an instance of a class which implements the given interface.
        /// </summary>
        /// <param name="t">The type of the the instance.</param>
        object GetInstance(Type t);

        /// <summary>
        /// Returns an instance of a class which implements the given interface.
        /// </summary>
        /// <param name="t">The type of the the instance.</param>
        /// <param name="name">The name of the registration.</param>
        object GetInstance(Type t, string name);

        /// <summary>
        /// Returns an instance of a class which implements the given interface.
        /// </summary>
        IEnumerable<TType> GetAllInstance<TType>();


        /// <summary>
        /// Returns if an interface is registered in the DI container or not
        /// </summary>
        bool IsRegistered<TInterface>();

        /// <summary>
        /// Returns if an interface is registered in the DI container or not
        /// </summary>
        /// <param name="name">The name of the registration.</param>
        bool IsRegistered<TInterface>(string name);
        

        /// <summary>
        /// Register a type for an interface in the DI container. 
        /// The type should implement the given interface.
        /// </summary>
        void RegisterType<TInterface, TType>() where TType : TInterface;

        /// <summary>
        /// Register a type for an interface in the DI container. 
        /// The type should implement the given interface.
        /// </summary>
        /// <param name="name">The name of the registration.</param>
        void RegisterType<TInterface, TType>(string name) where TType : TInterface;

        /// <summary>
        /// Register a type for an interface in the DI container.
        /// The type should implement the given interface.
        /// </summary>
        /// <param name="constructorParams">The parameters for the type constructor.</param>
        void RegisterType<TInterface, TType>(params object[] constructorParams) where TType : TInterface;

        /// <summary>
        /// Register a type for an interface in the DI container.
        /// The type should implement the given interface.
        /// </summary>
        /// <param name="name">The name of the registration.</param>
        /// <param name="constructorParams">The parameters for the type constructor.</param>
        void RegisterType<TInterface, TType>(string name, params object[] constructorParams) where TType : TInterface;
        
        /// <summary>
        /// Register a type for an interface in the DI container.
        /// The type should implement the given interface.       
        /// </summary>
        /// <param name="lifetimeType">Lifetime type of an instance of TType.</param>
        void RegisterType<TInterface, TType>(LifetimeTypes lifetimeType) where TType : TInterface;

        /// <summary>
        /// Register a type for an interface in the DI container.
        /// The type should implement the given interface.       
        /// </summary>
        /// <param name="name">The name of the registration.</param>
        /// <param name="lifetimeType">Lifetime type of an instance of TType.</param>
        void RegisterType<TInterface, TType>(string name, LifetimeTypes lifetimeType) where TType : TInterface;

        /// <summary>
        /// Register a type for an interface in the DI container.
        /// The type should implement the given interface.       
        /// </summary>
        /// <param name="lifetimeType">Lifetime type of an instance of TType.</param>
        /// <param name="constructorParams">The parameters for the type constructor.</param>
        void RegisterType<TInterface, TType>(LifetimeTypes lifetimeType, params object[] param) where TType : TInterface;

        /// <summary>
        /// Register a type for an interface in the DI container.
        /// The type should implement the given interface.       
        /// </summary>
        /// <param name="name">The name of the registration.</param>
        /// <param name="lifetimeType">Lifetime type of an instance of TType.</param>
        /// <param name="constructorParams">The parameters for the type constructor.</param>
        void RegisterType<TInterface, TType>(string name, LifetimeTypes lifetimeType, params object[] constructorParams) where TType : TInterface;

        /// <summary>
        /// Register a concrete instance for an interface in the DI container. 
        /// </summary>
        /// <param name="instance">The registered instance</param>
        void RegisterInstance<TInterface>(TInterface instance);

        /// <summary>
        /// Register a concrete instance for an interface in the DI container. 
        /// </summary>
        /// <param name="name">The name of the registration.</param>
        /// <param name="instance">The registered instance</param>
        void RegisterInstance<TInterface>(string name, TInterface instance);

        /// <summary>
        /// Loads the configuration of the DI container if exsist.
        /// </summary>
        void LoadConfiguration();

        /// <summary>
        /// Clean up all the objects handled by the DI container.
        /// </summary>
        void CleanUp();        
    }
}
