using System;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using YumaPos.Client.App;
using YumaPos.Client.Navigation;
using YumaPos.Client.UI.ViewModels.Impl;

namespace Y_POS.Core.ViewModels.Pages
{
    public sealed class NavMenuVm : BaseVm, INavMenuVm
    {
        #region Fields

        private readonly IAppService _appService;

        private readonly ReactiveCommand<object> _commandNavigate = ReactiveCommand.Create();

        #endregion

        #region Properties
        
        public string StoreName { get; private set; }
        public string TerminalName { get; private set; }

        #endregion

        #region Commands

        public ICommand CommandNavigate { get { return _commandNavigate; } }

        #endregion

        #region Constructor

        public NavMenuVm(IAppService appService)
        {
            if (appService == null) throw new ArgumentNullException(nameof(appService));

            _appService = appService;

            _commandNavigate
                .Select(o => (NavUri) o)
                .Where(uri => !uri.Equals(NavigationService.CurrenUri))
                .Subscribe(NavigateTo);

            this.WhenAnyValue(vm => vm.NavigationService.CurrenUri)
                .Subscribe(_ =>
                {

                });

            StoreName = appService.Store.Title;
            TerminalName = appService.Terminal.Name;
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
