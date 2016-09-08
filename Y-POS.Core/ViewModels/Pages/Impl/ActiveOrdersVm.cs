using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Common;
using YumaPos.Client.Navigation;
using YumaPos.Client.UI.ViewModels.Impl;
using Y_POS.Core.ViewModels.Items.Contracts;
using Y_POS.Core.ViewModels.Items.Impl;

namespace Y_POS.Core.ViewModels.Pages
{
    public class ActiveOrdersVm : PageVm, IActiveOrdersVm
    {
        #region Fields

        private ReactiveCommand<object> _commandCreateOrder;
        private ReactiveCommand<object> _commandCheckout;
        private ReactiveCommand<object> _commandPrintOrder;
        private ReactiveCommand<object> _commandVoid;

        #endregion

        #region Properties
        
        [Reactive]
        public IActiveOrderItemVm[] Items { get; private set; }
        [Reactive]
        public IActiveOrderItemVm SelectedItem { get; set; }

        #endregion

        #region Commands

        public ICommand CommandCreateOrder { get { return _commandCreateOrder; } }
        public ICommand CommandCheckout { get { return _commandCheckout; } }
        public ICommand CommandPrintOrder { get { return _commandPrintOrder; } }
        public ICommand CommandVoid { get { return _commandVoid; } }

        #endregion

        #region Constructor

        public ActiveOrdersVm()
        {
        }

        #endregion

        #region Lifecycle

        protected override void InitCommands()
        {
            _commandCreateOrder = ReactiveCommand.Create();
            _commandCheckout = ReactiveCommand.Create();
            _commandPrintOrder = ReactiveCommand.Create();
            _commandVoid = ReactiveCommand.Create();
        }

        protected override void InitLifetimeSubscriptions()
        {
            AddLifetimeSubscription(_commandCreateOrder.Subscribe(_ => NavigationService.StartIntent(new Intent(AppNavigation.OrderMaker))));
            AddLifetimeSubscription(_commandCheckout.Where(_ => SelectedItem != null).Subscribe(_ => NavigateTo(new Intent(AppNavigation.Checkout)
                .SetArgs(new ArgsBundle().Put("id", SelectedItem.OrderNumber)))));
        }

        protected override async void OnStart()
        {
            Items = await Task.Run(() => Enumerable.Range(120, 15).Select(i => new ActiveOrderItemVm(i)).OrderByDescending(vm => vm.OrderNumber).ToArray());
        }

        #endregion

        #region Private methods

        private void NavigateTo(Intent intent)
        {
            try
            {
                NavigationService.StartIntent(intent);
            }
            catch (Exception ex)
            {
                DialogService.CreateMessageDialog(ex.Message, "Error").Show();
            }
        }

        #endregion
    }
}
