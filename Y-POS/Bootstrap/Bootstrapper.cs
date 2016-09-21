using Autofac;
using Autofac.Builder;
using DialogManagement.Contracts;
using DialogManagement.Core;
using YumaPos.Client.Account;
using YumaPos.Client.App;
using YumaPos.Client.Builders;
using YumaPos.Client.Common;
using YumaPos.Client.Configuration;
using YumaPos.Client.Hardware;
using YumaPos.Client.LocalData.Repositories;
using YumaPos.Client.Navigation.Contracts;
using YumaPos.Client.Navigation.Impl;
using YumaPos.Client.Navigation.PageRegistration;
using YumaPos.Client.Services;
using YumaPos.Client.Services.Contracts;
using YumaPos.Client.WCF;
using YumaPos.Common.Infrastructure.IoC;
using YumaPos.Common.Infrastructure.IoC.Registration;
using YumaPos.Common.Infrastructure.Logging;
using YumaPos.Common.Tools.IoC;
using YumaPos.Common.Tools.Logging;
using YumaPos.FrontEnd.Infrastructure.Common.Serialization;
using YumaPos.Shared.API;
using YumaPos.Shared.Core.Reciept.Contracts;
using YumaPos.Shared.Infrastructure;
using Y_POS.Configuration;
using Y_POS.Core;
using Y_POS.Core.MockData;
using Y_POS.Core.ViewModels.PageParts;
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
            //builder.Register<MockAccountServiceManager>().As<IAccountService>().As<IAccountServiceManager>();
            //builder.Register<MockAppServiceManager>().As<IAppService>().As<IAppServiceManager>();
            builder.Register<ResourceService>().As<IResourcesService>();

            // API
            //builder.Register<MockAuthApi>().As<IAuthorizationApi>();
            builder.Register<ApiConfig>().As<IAPIConfig>();
            builder.Register<SerializationService>().As<ISerializationService>();
            builder.Register<AuthorizationApi>(Lifecycles.PerDependency).As<IAuthorizationApi>();
            builder.Register<TerminalApi>().As<ITerminalApi>();
            builder.Register<BackOfficeApi>().As<IBackOfficeApi>();

            // Data access
            builder.Register<MockCommonClientSettingsRepository>().As<ICommonClientSettingsRepository>();
            builder.Register<MockStoreSettingsRepository>().As<IStoreSettingsRepository>();

            // Navigation
            builder.Register<Navigator>().As<INavigator>();
            builder.Register<DefaultNavIntentProcessor>().As<INavIntentProcessor>();
            builder.Register<DefaultNavigationHistory>().As<INavigationHistory>();

            // Logging
            builder.Register<LoggingService>().As<ILoggingService>();

            // Terminal and account management
            builder.Register<AppServiceManager>().As<IAppServiceManager>().As<IAppService>();
            builder.Register<AccountServiceManager>().As<IAccountServiceManager>().As<IAccountService>();

            // Services
            //builder.Register<MockOrderService>().As<IOrderService>();
            builder.Register<OrderService>().As<IOrderService>();
            builder.Register<MenuService>().As<IMenuService>();
            builder.Register<InMemoryMenuService>().As<IInMemoryMenuService>();
            builder.Register<ImageService>().As<IImageService>();
            builder.Register<GiftCardService>().As<IGiftCardService>();
            builder.Register<CustomersService>().As<ICustomersService>();

            // Hardware
            builder.Register<MockPrinter>().As<IPrintService>();

            // Dialogs
            builder.Register<DialogManager>().As<IDialogManager>();
            
            // Business logic
            builder.Register<OrderCreator>(Lifecycles.PerScope).As<IOrderCreator>();
            builder.Register<OrderItemConstructor>(Lifecycles.PerScope).As<IOrderItemConstructor>();

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

            // Page parts ViewModels
            builder.Register<OrderMakerMenuVm>(Lifecycles.PerScope).As<IOrderMakerMenuVm>();
            builder.Register<OrderItemConstructorVm>(Lifecycles.PerScope).As<IOrderItemConstructorVm>();
            builder.Register<GiftCardsVm>(Lifecycles.PerScope).As<IGiftCardsVm>();
            builder.Register<OrderMakerSetCustomerVm>(Lifecycles.PerScope).As<IOrderMakerSetCustomerVm>();
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
