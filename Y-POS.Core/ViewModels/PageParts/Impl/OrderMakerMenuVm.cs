using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Common;
using YumaPos.Client.Extensions;
using YumaPos.Client.Services;
using YumaPos.Client.UI.ViewModels.Impl;
using YumaPos.Shared.Core.MenuModels;
using Y_POS.Core.Extensions;
using Y_POS.Core.ViewModels.Items.Contracts;
using Y_POS.Core.ViewModels.Items.Impl;

namespace Y_POS.Core.ViewModels.PageParts
{
    public sealed class OrderMakerMenuVm : PosLifecycleVm, IOrderMakerMenuVm
    {
        #region Fields

        private readonly IMenuService _menuService;

        private readonly ReactiveCommand<MenuItemSelectedEventArgs> _commandSelectItem;

        #endregion

        #region Properties

        [Reactive]
        public string SearchText { get; set; }
        [Reactive]
        public IMenuCategoryItemVm[] Categories { get; private set; }
        [Reactive]
        public IMenuItemItemVm[] CategoryItems { get; private set; }
        [Reactive]
        public IMenuCategoryItemVm SelectedCategory { get; set; }

        #endregion

        #region Commands

        public ICommand CommandSelectMenuItem => _commandSelectItem;

        #endregion

        #region Events

        public event EventHandler<MenuItemSelectedEventArgs> MenuItemSelected;

        #endregion

        #region Constructor

        public OrderMakerMenuVm(IMenuService menuService)
        {
            if (menuService == null) throw new ArgumentNullException(nameof(menuService));

            _menuService = menuService;

            _commandSelectItem =
                ReactiveCommand.CreateAsyncObservable(param => Observable.Return(param as IMenuItemItemVm)
                    .Where(item => item != null)
                    .SelectMany(GetMenuItemSelectedArgs));
        }

        #endregion

        #region Lifecycle

        protected override void OnCreate(IArgsBundle args)
        {
            _menuService.GetMenuCategoriesCollection()
                .Take(1)
                .Select(categories => categories.Select(category => new MenuCategoryItemVm(category)).ToArray())
                .Subscribe(categories => Categories = categories);
        }

        protected override void InitLifetimeSubscriptions()
        {
            // Menu item selection
            AddLifetimeSubscription(_commandSelectItem.Subscribe(RaiseMenuItemSelected));

            // Category selection
            AddLifetimeSubscription(this.WhenAnyValue(vm => vm.SelectedCategory)
                .Where(vm => vm != null)
                .SelectMany(vm => _menuService.GetMenuItemsCollectionForCategory(vm.ToGuid()))
                .Select(items => items.Select(item => new MenuItemItemVm(item)).ToArray())
                .SubscribeToObserveOnUi(vms => CategoryItems = vms));

            // Search for item
            AddLifetimeSubscription(this.WhenAnyValue(vm => vm.SearchText)
                .Throttle(TimeSpan.FromMilliseconds(250), SchedulerService.UiScheduler)
                .Select(GetSearchedMenuItems)
                .Switch()
                .SubscribeToObserveOnUi(items => CategoryItems = items));
        }

        #endregion

        #region Private methods

        private IObservable<MenuItemSelectedEventArgs> GetMenuItemSelectedArgs(IMenuItemItemVm item)
        {
            return
                _menuService.MenuItemHasRelatedModifiers(item.ToGuid())
                    .Select(hasModifiers => new MenuItemSelectedEventArgs(item, hasModifiers));
        }

        private IObservable<IMenuItemItemVm[]> GetSearchedMenuItems(string searchText)
        {
            IObservable<IMenuItem[]> stream;
            if (!string.IsNullOrEmpty(searchText))
            {
                stream = _menuService.GetSearchedMenuItems(searchText);
            }
            else
            {
                stream = SelectedCategory != null
                    ? _menuService.GetMenuItemsCollectionForCategory(SelectedCategory.ToGuid())
                    : Observable.Return(new IMenuItem[0]);
            }
            return stream.Select(items => items.Select(item => new MenuItemItemVm(item)).ToArray());
        }

        private void RaiseMenuItemSelected(MenuItemSelectedEventArgs args)
        {
            var handler = Volatile.Read(ref MenuItemSelected);
            handler?.Invoke(this, args);
        }

        #endregion
    }
}
