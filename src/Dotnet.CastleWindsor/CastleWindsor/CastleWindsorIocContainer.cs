﻿using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Dotnet.Dependency;
using System;
using System.Reflection;

namespace Dotnet.CastleWindsor
{
    public class CastleWindsorIocContainer : IIocContainer
    {
        private IWindsorContainer _container { get; set; }
        public CastleWindsorIocContainer(IWindsorContainer container)
        {
            _container = container;
        }

        public void UseEngine(object engine)
        {
            _container = (IWindsorContainer)engine;
        }

        public T GetEngine<T>()
        {
            return (T)_container;
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public T Resolve<T>(Type type)
        {
            return (T)_container.Resolve(type);
        }

        public T Resolve<T>(object argumentsAsAnonymousType)
        {
            return _container.Resolve<T>(argumentsAsAnonymousType);
        }

        public object Resolve(Type type, params object[] args)
        {
            return _container.Resolve(type, new Arguments(args));
        }

        public T ResolveNamed<T>(string serviceName)
        {
            return _container.Resolve<T>(serviceName);
        }

        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        public bool IsRegistered(Type type)
        {
            return _container.Kernel.HasComponent(type);
        }

        public bool IsRegistered<T>()
        {
            return _container.Kernel.HasComponent(typeof(T));
        }

        public void Register<T>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton, string serviceName = null) where T : class
        {
            var registration = Component.For<T>();
            if (serviceName != null)
            {
                registration.Named(serviceName);
            }
            registration.IsDefault();
            _container.Register(ApplyLifestyle(registration, lifeStyle));
        }

        public void Register(Type type, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton, string serviceName = null)
        {
            var registration = Component.For(type);
            if (serviceName != null)
            {
                registration.Named(serviceName);
            }
            registration.IsDefault();
            _container.Register(ApplyLifestyle(registration, lifeStyle));
        }

        public void Register<T>(T impl) where T : class
        {
            var registration = Component.For<T>().Instance(impl);
            registration.IsDefault();
            _container.Register(ApplyLifestyle(registration, DependencyLifeStyle.Singleton));
        }

        public void Register<TType, TImpl>(DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton, string serviceName = null)
            where TType : class
            where TImpl : class, TType
        {
            var registration = Component.For<TType, TImpl>().ImplementedBy<TImpl>();
            if (serviceName != null)
            {
                registration.Named(serviceName);
            }
            registration.IsDefault();
            _container.Register(ApplyLifestyle(registration, lifeStyle));
        }

        public void Register<TType, TImpl>(TImpl impl)
         where TType : class
         where TImpl : class, TType
        {
            var registration = Component.For<TType>().Instance(impl);
            registration.IsDefault();
            _container.Register(ApplyLifestyle(registration, DependencyLifeStyle.Singleton));
        }


        public void Register(Type type, Type impl, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton, string serviceName = null, bool propertiesAutowired = true, bool isDefault = false)
        {
            var registration = Component.For(type, impl).ImplementedBy(impl);
            if (serviceName != null)
            {
                registration.Named(serviceName);
            }
            if (isDefault)
            {
                registration.IsDefault();
            }
            _container.Register(ApplyLifestyle(registration, lifeStyle));
        }

        public void RegisterAssemblyByConvention(Assembly assembly) {

        }


        private static ComponentRegistration<T> ApplyLifestyle<T>(ComponentRegistration<T> registration, DependencyLifeStyle lifeStyle)
           where T : class
        {
            switch (lifeStyle)
            {
                case DependencyLifeStyle.Transient:
                    return registration.LifestyleTransient();
                case DependencyLifeStyle.Singleton:
                    return registration.LifestyleSingleton();
                default:
                    return registration;
            }
        }
    }
}
