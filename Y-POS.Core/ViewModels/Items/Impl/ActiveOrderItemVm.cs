using System;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using YumaPos.Client.Common;
using YumaPos.Client.Navigation;
using YumaPos.Client.UI.ViewModels.Impl;
using YumaPos.Shared.API.Enums;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Core.ViewModels.Items.Impl
{
    public sealed class ActiveOrderItemVm : BaseEntityVm, IActiveOrderItemVm
    {
        #region Properties

        public int OrderNumber { get; }
        public DateTime CreationTime { get; }
        public OrderStatus Status { get; }
        public string CustomerName { get; }
        public decimal Amount { get; }

        #endregion

        #region Commands

        public ICommand CommandOpen { get; }

        #endregion

        #region Constructor

        public ActiveOrderItemVm(int number) : base(number.ToString())
        {
            OrderNumber = number;
            CreationTime = MockDataGenerator.GetRandomDate(DateTime.Today, DateTime.UtcNow);
            Status = MockDataGenerator.GetOrderStatus();
            CustomerName = MockDataGenerator.GetRandomCustomerName();
            Amount = MockDataGenerator.GetRandomAmount(10, 50);

            var cmd = ReactiveCommand.Create();
            cmd.Select(o => (int) o).Subscribe(id => NavigationService.StartIntent(new Intent(AppNavigation.OrderMaker)
                .SetArgs(new ArgsBundle().Put("id", id))));
            CommandOpen = cmd;
        }

        #endregion
    }
}
