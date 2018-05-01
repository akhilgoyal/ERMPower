using System;
using ERMPower.Domain;
using ERMPower.Domain.Parser;
using ERMPower.Services;
using Microsoft.Practices.Unity;
using Unity;

namespace ERMPower.Services
{
    public static class UnityConfig
    {
        private static readonly Lazy<IUnityContainer> _container = new Lazy<IUnityContainer>(() => {

            IUnityContainer container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            return _container.Value;
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IFileProcessingService, MeterFileProcessingService>();
            container.RegisterType<IParser, LPParser>("LP");
            container.RegisterType<IParser, TOUParser>("TOU");
        }
    }
}
