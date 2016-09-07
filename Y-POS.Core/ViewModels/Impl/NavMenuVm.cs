using System;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using YumaPos.Client.Navigation;
using YumaPos.Client.UI.ViewModels.Impl;

namespace Y_POS.Core.ViewModels
{
    public sealed class NavMenuVm : BaseVm, INavMenuVm
    {
        #region Fields

        private readonly ReactiveCommand<object> _commandNavigate = ReactiveCommand.Create();

        #endregion

        #region Properties

        #endregion

        #region Commands

        public ICommand CommandNavigate { get { return _commandNavigate; } }

        #endregion

        #region Constructor

        public NavMenuVm()
        {
            _commandNavigate
                .Select(o => (NavUri) o)
                .Where(uri => !uri.Equals(NavigationService.CurrenUri))
                .Subscribe(NavigateTo);

            this.WhenAnyValue(vm => vm.NavigationService.CurrenUri)
                .Subscribe();
        }

        #endregion

        #region Private methods

        private void NavigateTo(NavUri target)
        {
            try
            {
                NavigationService.StartIntent(new Intent(target).AddFlag(IntentFlags.ClearTop));
            }
            catch (Exception ex)
            {
                DialogService.CreateMessageDialog(ex.Message).Show();
            }
        }

        #endregion
    }
}
