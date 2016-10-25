using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Common;
using YumaPos.Client.Navigation;
using YumaPos.Client.UI.ViewModels.Impl;
using YumaPos.Shared.API.Enums;
using YumaPos.Shared.API.Models;
using YumaPos.Shared.Core.Utils.Formating;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Core.ViewModels.Items.Impl
{
    public sealed class ActiveOrderItemVm : BaseEntityVm, IActiveOrderItemVm
    {
        #region Fields

        private bool _isReadOnly;

        #endregion

        #region Properties

        public int OrderNumber { get; }
        public DateTime CreationTime { get; }
        [Reactive]
        public OrderStatus Status { get; private set; }

        public IImageModel CustomerPhoto { get; }
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
            CustomerPhoto = ImageService.GetImage(dto.CustomerDto?.ImageId);
            CustomerName = dto.CustomerDto != null ? FormattingUtils.FullName(dto.CustomerDto.FirstName, dto.CustomerDto.LastName) : string.Empty;
            Amount = dto.Amount;

            _isReadOnly = dto.Transactions != null && dto.Transactions.Any();

            var cmd = ReactiveCommand.Create();
            cmd.Select(o => Guid.Parse((string) o))
                .ObserveOn(SchedulerService.UiScheduler)
                .Subscribe(id =>
                {
                    var intent = new Intent(_isReadOnly ? AppNavigation.Checkout : AppNavigation.OrderMaker)
                        .SetArgs(new ArgsBundle().Put("id", id))
                        .AddToBackstack(!_isReadOnly);
                    NavigationService.StartIntent(intent);
                });
            CommandOpen = cmd;
        }

        #endregion

        internal void UpdateStatus(OrderStatus status)
        {
            Status = status;
        }
    }
}
