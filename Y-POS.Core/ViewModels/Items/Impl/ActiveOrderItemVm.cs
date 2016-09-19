using System;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Common;
using YumaPos.Client.Navigation;
using YumaPos.Client.UI.ViewModels.Impl;
using YumaPos.Shared.API.Enums;
using YumaPos.Shared.API.Models;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Core.ViewModels.Items.Impl
{
    public sealed class ActiveOrderItemVm : BaseEntityVm, IActiveOrderItemVm
    {
        #region Properties

        public int OrderNumber { get; }
        public DateTime CreationTime { get; }
        [Reactive]
        public OrderStatus Status { get; private set; }
        public string CustomerName { get; }
        public decimal Amount { get; }

        #endregion

        #region Commands

        public ICommand CommandOpen { get; }

        #endregion

        #region Constructor

        public ActiveOrderItemVm(RestaurantOrderDto dto) : base(dto.OrderId.ToString())
        {
            OrderNumber = dto.Number;
            CreationTime = dto.Created;
            Status = dto.Status;
            CustomerName = dto.CustomerName;
            Amount = dto.Amount;

            var cmd = ReactiveCommand.Create();
            cmd.Select(o => Guid.Parse((string) o))
                .ObserveOn(SchedulerService.UiScheduler)
                .Subscribe(id => NavigationService.StartIntent(new Intent(AppNavigation.OrderMaker)
                    .SetArgs(new ArgsBundle().Put("id", id))));
            CommandOpen = cmd;
        }

        #endregion

        internal void UpdateStatus(OrderStatus status)
        {
            Status = status;
        }
    }
}
