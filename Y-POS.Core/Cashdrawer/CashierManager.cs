using System;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Services;
using Y_POS.Core.Infrastructure;
using Y_POS.Core.Infrastructure.Exceptions;

namespace Y_POS.Core.Cashdrawer
{
    public sealed class CashierManager : ReactiveObject
    {
        #region Fields

        private readonly IShiftService _shiftService;
        private readonly ICashDrawerService _cashDrawerService;

        #endregion

        #region Properties

        /// <summary>
        /// Flag indicating that manager is initialized. <c>InitAsync()</c> method should be called before calling any other method.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Flag indicating that "cashier in" operation is performed and cashdrawer is available for cash operations.
        /// All operations except <c>CashieInAsync</c> will result in <c>InvalidOperationException</c> if this flag is false.
        /// </summary>
        [Reactive]
        public bool IsCashierIn { get; private set; }

        /// <summary>
        /// Summary information about operations. Updated after each operation.
        /// </summary>
        [Reactive]
        public CashDrawerSummary Summary { get; private set; }

        #endregion

        #region Constructor

        public CashierManager(IShiftService shiftService, ICashDrawerService cashDrawerService)
        {
            if (shiftService == null) throw new ArgumentNullException(nameof(shiftService));
            if (cashDrawerService == null) throw new ArgumentNullException(nameof(cashDrawerService));

            _shiftService = shiftService;
            _cashDrawerService = cashDrawerService;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Initialize CashierManager, i.e. check if cashier is available for operations and retrieve summary information
        /// </summary>
        /// <returns></returns>
        public Task InitAsync()
        {
            return InitInternal();
        }

        /// <summary>
        /// Make CashierManager available for operations.
        /// </summary>
        /// <param name="amount">Actual amount of cash in cashdrawer</param>
        /// <exception cref="InvalidOperationException">If <c>IsInitialized</c> flag is false or <c>IsCashierIn</c> flag is true.</exception>
        /// <returns></returns>
        public Task CashierInAsync(decimal amount)
        {
            CheckInitialized();
            if (IsCashierIn) throw new InvalidOperationException("Cashier is already in!");

            return CashierInInternal(amount, CancellationToken.None);
        }

        /// <summary>
        /// Make CashierManager unavailable for operations.
        /// </summary>
        /// <param name="amount"></param>
        /// <exception cref="InvalidOperationException">If <c>IsInitialized</c> or <c>IsCashierIn</c> flag is false.</exception>
        /// <returns></returns>
        public Task CashierOutAsync(decimal amount)
        {
            CheckInitialized();
            if (!IsCashierIn) throw new InvalidOperationException("Cashier is already out!");

            return CashierOutInternal(amount, CancellationToken.None);
        }

        /// <summary>
        /// Add cash to cashdrawer.
        /// </summary>
        /// <param name="amount">Amount of cash to add</param>
        /// <param name="reason">Reason for add cash operation</param>
        /// <exception cref="InvalidOperationException">If <c>IsInitialized</c> or <c>IsCashierIn</c> flag is false.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If specified amount is less than or equal to zero.</exception>
        /// <returns></returns>
        public Task AddCashAsync(decimal amount, string reason)
        {
            CheckInitialized();
            CheckCashierIn();
            Guard.IsPositive(amount, nameof(amount), "Amount cannot be <= 0!");

            return AddCashInternal(amount, reason, CancellationToken.None);
        }

        /// <summary>
        /// Remove cash from cashdrawer.
        /// </summary>
        /// <param name="amount">Amount of cash to remove</param>
        /// <param name="reason">Reason for remove cash</param>
        /// <exception cref="InvalidOperationException">If <c>IsInitialized</c> or <c>IsCashierIn</c> flag is false.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If specified amount is less than or equal to zero.</exception>
        /// <exception cref="NotEnoughAmountException">If requested amount is greater than available amount</exception>
        /// <returns></returns>
        public Task ExtractCashAsync(decimal amount, string reason)
        {
            CheckInitialized();
            CheckCashierIn();
            Guard.IsPositive(amount, nameof(amount), "Amount cannot be <= 0!");
            if (amount > Summary.Balance) throw new NotEnoughAmountException(amount, Summary.Balance);

            return ExtractCashInternal(amount, reason, CancellationToken.None);
        }

        /// <summary>
        /// Perform bank withdraw operation.
        /// </summary>
        /// <param name="amount">Amount of cash to remove in favor of bank</param>
        /// <exception cref="InvalidOperationException">If <c>IsInitialized</c> or <c>IsCashierIn</c> flag is false.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If specified amount is less than or equal to zero.</exception>
        /// <exception cref="NotEnoughAmountException">If requested amount is greater than available amount</exception>
        /// <returns></returns>
        public Task BankWithdrawAsync(decimal amount)
        {
            CheckInitialized();
            CheckCashierIn();
            Guard.IsPositive(amount, nameof(amount), "Amount cannot be <= 0!");
            if (amount > Summary.Balance) throw new NotEnoughAmountException(amount, Summary.Balance);

            return BankWithdrawInternal(amount, CancellationToken.None);
        }

        /// <summary>
        /// Add cash for tips
        /// </summary>
        /// <param name="amount">Tips amount to add</param>
        /// <param name="reason">Reason for tips addition</param>
        /// <exception cref="InvalidOperationException">If <c>IsInitialized</c> or <c>IsCashierIn</c> flag is false.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If specified amount is less than or equal to zero.</exception>
        /// <returns></returns>
        public Task AddTipsAsync(decimal amount, string reason)
        {
            CheckInitialized();
            CheckCashierIn();
            Guard.IsPositive(amount, nameof(amount), "Amount cannot be <= 0!");

            return AddTipsInternal(amount, reason, CancellationToken.None);
        }

        /// <summary>
        /// Add to log actual amount of cash in cashdrawer
        /// </summary>
        /// <param name="amount">Actual amount in cashdrawer</param>
        /// <returns></returns>
        public Task PerformCheckAsync(decimal amount)
        {
            CheckInitialized();
            CheckCashierIn();

            return PerformCheckInternal(amount, CancellationToken.None);
        }

        #endregion

        #region Private methods

        private void CheckInitialized()
        {
            if (!IsInitialized) throw new InvalidOperationException($"{nameof(CashierManager)} is not initialized!");
        }

        private void CheckCashierIn()
        {
            if (!IsCashierIn) throw new InvalidOperationException("CashierIn is not performed!");
        }

        private async Task InitInternal()
        {
            IsCashierIn = !await _shiftService.IsCashierOut().ToTask().ConfigureAwait(false);
            Summary = new CashDrawerSummary(await _cashDrawerService.GetInfo().ToTask().ConfigureAwait(false));
            IsInitialized = true;
        }

        private async Task CashierInInternal(decimal amount, CancellationToken ct)
        {
            await _cashDrawerService.CashierInCheck(amount).ToTask(ct).ConfigureAwait(false);
            IsCashierIn = true;
            if (ct.IsCancellationRequested)
            {
                return;
            }
            await RefreshSummary(ct);
        }

        private async Task CashierOutInternal(decimal amount, CancellationToken ct)
        {
            await _cashDrawerService.CashierOutCheck(amount).ToTask(ct).ConfigureAwait(false);
            IsCashierIn = false;
            if (ct.IsCancellationRequested)
            {
                return;
            }
            await RefreshSummary(ct);
        }

        private async Task AddCashInternal(decimal amount, string reason, CancellationToken ct)
        {
            await _cashDrawerService.AddCashIn(amount, reason).ToTask(ct).ConfigureAwait(false);
            if (ct.IsCancellationRequested) return;
            await RefreshSummary(ct);
        }

        private async Task ExtractCashInternal(decimal amount, string reason, CancellationToken ct)
        {
            await _cashDrawerService.CashOutPickUp(amount, reason).ToTask(ct).ConfigureAwait(false);
            await RefreshSummary(ct);
        }

        private async Task BankWithdrawInternal(decimal amount, CancellationToken ct)
        {
            await _cashDrawerService.BankWithdrawPickUp(amount).ToTask(ct).ConfigureAwait(false);
            await RefreshSummary(ct);
        }

        private async Task AddTipsInternal(decimal amount, string reason, CancellationToken ct)
        {
            await _cashDrawerService.AddTips(amount, reason).ToTask(ct).ConfigureAwait(false);
            await RefreshSummary(ct);
        }

        private async Task PerformCheckInternal(decimal amount, CancellationToken ct)
        {
            await _cashDrawerService.CashCheck(amount).ToTask(ct).ConfigureAwait(false);
            await RefreshSummary(ct);
        }

        private async Task RefreshSummary(CancellationToken ct)
        {
            Summary = new CashDrawerSummary(await _cashDrawerService.GetInfo().ToTask(ct).ConfigureAwait(false));
        }

        #endregion
    }
}
