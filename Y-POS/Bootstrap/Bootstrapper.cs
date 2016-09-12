﻿using Autofac;
using Autofac.Builder;
using DialogManagement.Contracts;
using DialogManagement.Core;
using YumaPos.Client.Account;
using YumaPos.Client.App;
using YumaPos.Client.Common;
using YumaPos.Client.Configuration;
using YumaPos.Client.Hardware;
using YumaPos.Client.Navigation.Contracts;
using YumaPos.Client.Navigation.Impl;
using YumaPos.Client.Navigation.PageRegistration;
using YumaPos.Client.Services;
using YumaPos.Common.Infrastructure.IoC;
using YumaPos.Common.Infrastructure.IoC.Registration;
using YumaPos.Common.Infrastructure.Logging;
using YumaPos.Common.Tools.IoC;
using YumaPos.Common.Tools.Logging;
using YumaPos.Shared.API;
using Y_POS.Configuration;
using Y_POS.Core;
using Y_POS.Core.MockData;
using Y_POS.Core.ViewModels.Pages;
using Y_POS.Resources;

namespace Y_POS.Bootstrap
{
    internal static class Bootstrapper
    {
        private const string MAIN_SCOPE = "MainScope";

        #region Properties

        #endregion

        public static IResolver Run()
        {
            var builder = new ContainerBuilder();
            
            // Register types
            RegisterTypes(builder);

            var container = builder.Build();

            var mainScope = container.BeginLifetimeScope("MainScope");

            return mainScope.Resolve<IResolver>();
        }

        private static void RegisterTypes(ContainerBuilder builder)
        {
            // Common
            builder.Register<Resolver>(Lifecycles.PerScope).As<IResolver>();
            builder.Register<ScopeManager>().As<IScopeManager>();
            builder.Register<WpfSchedulerService>().As<ISchedulerService>();
            builder.Register<ConfigurationProvider>().As<IConfigurationProvider>();
            builder.Register<MockAuthApi>().As<IAuthorizationApi>();
            builder.Register<MockAccountServiceManager>().As<IAccountService>().As<IAccountServiceManager>();
            builder.Register<MockAppServiceManager>().As<IAppService>().As<IAppServiceManager>();
            builder.Register<ResourceService>().As<IResourcesService>();
            
            // Navigation
            builder.Register<Navigator>().As<INavigator>();
            builder.Register<DefaultNavIntentProcessor>().As<INavIntentProcessor>();
            builder.Register<DefaultNavigationHistory>().As<INavigationHistory>();

            // Logging
            builder.Register<LoggingService>().As<ILoggingService>();

            // Services
            builder.Register<MockOrderService>().As<IOrderService>();

            // Hardware
            builder.Register<MockPrinter>().As<IPrintService>();

            // Dialogs
            builder.Register<DialogManager>().As<IDialogManager>();

            // Main ViewModels
            builder.Register<AppMainVm>().As<IAppMainVm>();
            builder.Register<NavMenuVm>().As<INavMenuVm>();

            // Page ViewModels
            builder.RegisterType<AppPagesRegistrator>().As<PagesRegistrator>().InstancePerDependency();
            builder.RegisterType<LoginVm>().As<ILoginVm>().InstancePerLifetimeScope();
            builder.RegisterType<PinVm>().As<IPinVm>().InstancePerLifetimeScope();
            builder.RegisterType<ActiveOrdersVm>().As<IActiveOrdersVm>().InstancePerLifetimeScope();
            builder.RegisterType<ClosedOrdersVm>().As<IClosedOrdersVm>().InstancePerLifetimeScope();
            builder.RegisterType<CashdrawerVm>().As<ICashdrawerVm>().InstancePerLifetimeScope();
            builder.RegisterType<ReportsVm>().As<IReportsVm>().InstancePerLifetimeScope();
            builder.RegisterType<SettingsVm>().As<ISettingsVm>().InstancePerLifetimeScope();
            builder.RegisterType<OrderMakerVm>().As<IOrderMakerVm>().InstancePerLifetimeScope();
            builder.RegisterType<CheckoutVm>().As<ICheckoutVm>().InstancePerLifetimeScope();
        }

        private static IRegistrationBuilder<TImpl, ConcreteReflectionActivatorData, SingleRegistrationStyle> Register<TImpl>(this ContainerBuilder builder, Lifecycles lifecycle = Lifecycles.PerDefaultScope)
        {
            var reg = builder.RegisterType<TImpl>();
            switch (lifecycle)
            {
                case Lifecycles.PerDefaultScope:
                    reg.InstancePerMatchingLifetimeScope(MAIN_SCOPE);
                    break;
                case Lifecycles.PerScope:
                    reg.InstancePerLifetimeScope();
                    break;
                case Lifecycles.Singleton:
                    reg.SingleInstance();
                    break;
            }
            return reg;
        } 

        private static IRegistrationBuilder<TImpl, ConcreteReflectionActivatorData, SingleRegistrationStyle> Register<TInterface, TImpl>(this ContainerBuilder builder, Lifecycles lifecycle = Lifecycles.PerDefaultScope)
        {
            return builder.Register<TImpl>(lifecycle).As<TInterface>();
        }
        
        public sealed class AppPagesRegistrator : PagesRegistrator
        {
            protected override void Init()
            {
                Register<ILoginVm>(AppNavigation.Login);
                Register<IPinVm>(AppNavigation.PinLogin);
                Register<IActiveOrdersVm>(AppNavigation.ActiveOrders);
                Register<IClosedOrdersVm>(AppNavigation.ClosedOrders);
                Register<ICashdrawerVm>(AppNavigation.Cashdrawer);
                Register<IReportsVm>(AppNavigation.Reports);
                Register<ISettingsVm>(AppNavigation.Settings);
                Register<IOrderMakerVm>(AppNavigation.OrderMaker);
                Register<ICheckoutVm>(AppNavigation.Checkout);
            }
        }
    }
}
