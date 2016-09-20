using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Builders;
using YumaPos.Client.Common;
using YumaPos.Client.Extensions;
using YumaPos.Client.Services;
using YumaPos.Client.UI.ViewModels.Impl;
using Y_POS.Core.Extensions;
using Y_POS.Core.Properties;
using Y_POS.Core.ViewModels.Items.Contracts;
using Y_POS.Core.ViewModels.Items.Impl;

namespace Y_POS.Core.ViewModels.PageParts
{
    public sealed class GiftCardsVm : LifecycleVm, IGiftCardsVm
    {
        private enum SectionType
        {
            IssueCards,
            Refill,
            CheckBalance
        }

        #region Fields

        private readonly IOrderCreator _orderCreator;
        private readonly IGiftCardService _giftCardService;

        private ReactiveCommand<object> _commandGoToIssueCard;
        private ReactiveCommand<object> _commandGoToRefill;
        private ReactiveCommand<object> _commandGoToBalance;

        private ReactiveCommand<object> _commandCancel;
        private ReactiveCommand<object> _commandDone;

        #endregion

        #region Properties

        [Reactive]
        public IGiftCardItemVm[] CardTypes { get; private set; }

        [Reactive]
        public IGiftCardItemVm SelectedCardType { get; set; }

        [Reactive]
        public string CardNumber { get; set; }

        [Reactive]
        public decimal RefillAmount { get; set; }

        [Reactive]
        public decimal Balance { get; private set; }

        [ObservableAsProperty]
        public extern bool IsIssueCardsVisible { get; }

        [ObservableAsProperty]
        public extern bool IsRefillVisible { get; }

        [ObservableAsProperty]
        public extern bool IsBalanceVisible { get; }

        [Reactive]
        private SectionType Type { get; set; }

        #endregion

        #region Commands

        public ICommand CommandGoToIssueCard => _commandGoToIssueCard;
        public ICommand CommandGoToRefill => _commandGoToRefill;
        public ICommand CommandGoToBalance => _commandGoToBalance;
        public ICommand CommandCancel => _commandCancel;
        public ICommand CommandDone => _commandDone;

        #endregion

        #region Events

        public event EventHandler CloseEvent;

        #endregion

        #region Constructor

        public GiftCardsVm(IOrderCreator orderCreator, IGiftCardService giftCardService)
        {
            if (orderCreator == null) throw new ArgumentNullException(nameof(orderCreator));
            if (giftCardService == null) throw new ArgumentNullException(nameof(giftCardService));

            _orderCreator = orderCreator;
            _giftCardService = giftCardService;
        }

        #endregion

        #region Lifecycle

        protected override void OnCreate(IArgsBundle args)
        {
            _giftCardService.GetGiftCardTypes()
                .Select(dtos => dtos.Select(dto => new GiftCardItemVm(dto)).ToArray())
                .SubscribeToObserveOnUi(cardTypes => CardTypes = cardTypes, HandleError);
        }

        protected override void InitCommands()
        {
            _commandGoToIssueCard = ReactiveCommand.Create();
            _commandGoToRefill = ReactiveCommand.Create();
            _commandGoToBalance = ReactiveCommand.Create();
            _commandCancel = ReactiveCommand.Create();

            var canExecuteDone = this.WhenAny(vm => vm.Type, vm => vm.CardNumber, vm => vm.SelectedCardType, vm => vm.RefillAmount,
                (type, number, cardType, refillAmount) =>
                    string.IsNullOrEmpty(number.Value) 
                    || (type.Value == SectionType.IssueCards && cardType.Value == null)
                    || (type.Value == SectionType.Refill && refillAmount.Value <= 0))
                .Select(b => !b);
            _commandDone = ReactiveCommand.Create(canExecuteDone);
        }

        protected override void InitLifetimeSubscriptions()
        {
            // Go to Issue Card
            AddLifetimeSubscription(_commandGoToIssueCard.Subscribe(_ => Type = SectionType.IssueCards));

            // Go to Refill
            AddLifetimeSubscription(_commandGoToRefill.Subscribe(_ => Type = SectionType.Refill));
            
            // Go to Balance
            AddLifetimeSubscription(_commandGoToBalance.Subscribe(_ => Type = SectionType.CheckBalance));
            
            // Track current type
            var typeStream = this.WhenAnyValue(vm => vm.Type).ObserveOn(SchedulerService.UiScheduler);
            AddLifetimeSubscription(typeStream.Select(type => type == SectionType.IssueCards).ToPropertyEx(this, vm => vm.IsIssueCardsVisible));
            AddLifetimeSubscription(typeStream.Select(type => type == SectionType.Refill).ToPropertyEx(this, vm => vm.IsRefillVisible));
            AddLifetimeSubscription(typeStream.Select(type => type == SectionType.CheckBalance).ToPropertyEx(this, vm => vm.IsBalanceVisible));

            // Cancel command
            AddLifetimeSubscription(_commandCancel.Subscribe(_ => RaiseCloseEvent()));
            
            // Ok command
            AddLifetimeSubscription(_commandDone.Subscribe(_ => PerformOperation(Type, CardNumber, RefillAmount)));
        }

        protected override void OnStop()
        {
            SelectedCardType = null;
            CardNumber = string.Empty;
            Balance = 0;
            RefillAmount = 0;
        }

        #endregion

        #region Private methods

        private void PerformOperation(SectionType type, string cardNumber, decimal refillAmount = default (decimal))
        {
            if (string.IsNullOrEmpty(cardNumber))
            {
                DialogService.ShowErrorMessage(Resources.Dialog_Error_EmptyGiftCardNumber);
                return;
            }

            if (type == SectionType.Refill && refillAmount <= 0)
            {
                DialogService.ShowErrorMessage(Resources.Dialog_Error_InvalidRefillAmount);
                return;
            }

            switch (type)
            {
                case SectionType.IssueCards:
                    _orderCreator.AddGiftCardToOrder(SelectedCardType.ToGuid(), CardNumber)
                        .SubscribeToObserveOnUi(_ => RaiseCloseEvent());
                    break;
                case SectionType.Refill:
                    _orderCreator.AddGiftCardRefill(CardNumber, refillAmount)
                        .SubscribeToObserveOnUi(_ => RaiseCloseEvent());
                    break;
                case SectionType.CheckBalance:
                    _giftCardService.CheckGiftCardBalance(CardNumber)
                        .SubscribeToObserveOnUi(balance => Balance = balance);
                    break;
            }
        }

        private void RaiseCloseEvent()
        {
            var handler = Volatile.Read(ref CloseEvent);
            handler?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}