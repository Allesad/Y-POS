using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Builders;
using YumaPos.Client.Common;
using YumaPos.Client.Extensions;
using YumaPos.Client.UI.ViewModels.Impl;
using Y_POS.Core.Extensions;
using Y_POS.Core.ViewModels.Items.Contracts;
using Y_POS.Core.ViewModels.Items.Impl;

namespace Y_POS.Core.ViewModels.PageParts
{
    public sealed class OrderItemConstructorVm : LifecycleVm, IOrderItemConstructorVm
    {
        #region Fields

        private readonly IOrderItemConstructor _itemConstructor;

        private readonly ReactiveList<ModifiersGroupItemVm> _selectedModifiersGroups = new ReactiveList<ModifiersGroupItemVm>();

        private Guid? _orderItemId;

        #endregion

        #region Properties

        public Guid? MenuItemId { get; private set; }

        [Reactive]
        public IModifierItemVm[] RelatedModifiers { get; private set; }
        public IReadOnlyReactiveList<IModifiersGroupItemVm> SelectedGroups => _selectedModifiersGroups;
        [Reactive]
        public IModifiersGroupItemVm SelectedGroup { get; set; }

        [Reactive]
        public string MenuItemTitle { get; private set; }
        [Reactive]
        public decimal MenuItemPrice { get; private set; }
        [Reactive]
        public string GroupTitle { get; private set; }
        [Reactive]
        public int MaxGroupQty { get; private set; }
        [Reactive]
        public string RequiredStatus { get; private set; }
        [Reactive]
        public decimal Total { get; private set; }
        
        public extern bool CanCompleteItem { [ObservableAsProperty] get; }

        #endregion

        #region Constructor

        public OrderItemConstructorVm(IOrderItemConstructor itemConstructor)
        {
            if (itemConstructor == null) throw new ArgumentNullException(nameof(itemConstructor));

            _itemConstructor = itemConstructor;
        }

        #endregion

        #region Lifecycle

        protected override void InitLifetimeSubscriptions()
        {
            // Track added/removed items from selected modifiers list
            AddLifetimeSubscription(this.WhenAnyValue(vm => vm._itemConstructor.SelectedItems.Changed).Switch()
                .SubscribeToObserveOnUi(OnSelectedItemsChanged));

            // Track modifiers to select
            var modifiersStream = this.WhenAnyValue(vm => vm._itemConstructor.Modifiers).Publish().RefCount();

            AddLifetimeSubscription(modifiersStream.Select(items => items != null
                    ? items.Select(item => new ModifierItemVm(item, _itemConstructor) as IModifierItemVm).ToArray()
                    : new IModifierItemVm[0])
                .SubscribeToObserveOnUi(items => RelatedModifiers = items));

            // Automatically add empty group for new set of related modifiers
            AddLifetimeSubscription(modifiersStream
                .Where(items => items.Any() && items[0].Type == ModifierType.Related)
                .Select(items => items[0])
                .Select(item => GetGroupForModifier(item) == null ? CreateGroupForRelatedModifier(item) : null)
                .Where(vm => vm != null)
                .SubscribeToObserveOnUi(vm => _selectedModifiersGroups.Add(vm)));

            // Update groups info when related modifiers set changed
            AddLifetimeSubscription(modifiersStream.SubscribeToObserveOnUi(UpdateGroupInfoOnModifiersChanged));

            // Track selected modifiers group
            AddLifetimeSubscription(this.WhenAnyValue(vm => vm.SelectedGroup).Skip(1)
                .Where(vm => vm != null)
                .Select(vm => (ModifiersGroupItemVm) vm)
                .SubscribeToObserveOnUi(OnGroupSelected));

            // Listen to event to retrieve common modifiers groups after initial related modifiers selection
            AddLifetimeSubscription(Observable.FromEventPattern(
                    h => _itemConstructor.RelatedModifiersPassed += h,
                    h => _itemConstructor.RelatedModifiersPassed -= h)
                .Select(_ => GetCommonModifiersGroups().ToArray())
                .SubscribeToObserveOnUi(groups =>
                {
                    _selectedModifiersGroups.AddRange(groups);
                    var firstGroup = groups.FirstOrDefault();
                    SelectedGroup = firstGroup != null
                        ? _selectedModifiersGroups.First(vm => vm.Id == firstGroup.Id)
                        : null;
                }));

            // Track total order item amount
            AddLifetimeSubscription(this.WhenAnyValue(vm => vm._itemConstructor.TotalAmount)
                .SubscribeToObserveOnUi(amount => Total = amount));
        }

        protected override void OnCreate(IArgsBundle args)
        {
            this.WhenAnyValue(vm => vm._itemConstructor.CanComplete).ToPropertyEx(this, vm => vm.CanCompleteItem);
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        #endregion

        #region Public Methods

        public void ProcessMenuItem(IMenuItemItemVm menuItem)
        {
            MenuItemId = menuItem.ToGuid();
            MenuItemTitle = menuItem.Title;
            MenuItemPrice = menuItem.Price;

            _itemConstructor.ProcessMenuItem(MenuItemId.Value, MenuItemPrice);
        }

        public void EditOrderItem(Guid orderId, IOrderedItemVm orderedItem)
        {
            _orderItemId = orderedItem.ToGuid();
            MenuItemTitle = orderedItem.Title;
            MenuItemPrice = orderedItem.Price;

            _itemConstructor.EditOrderItem(orderId, _orderItemId.Value, orderedItem.Price);
        }

        public void Cancel()
        {
            MenuItemId = null;
            _orderItemId = null;
            _itemConstructor.Clean();
            SelectedGroup = null;
        }

        public IEnumerable<ModifierToAdd> GetRelatedModifiers()
        {
            return
                _selectedModifiersGroups.Where(vm => vm.Type == ModifierType.Related)
                    .SelectMany(vm => vm.Modifiers.Select(itemVm => new ModifierToAdd(itemVm.ToGuid(), itemVm.Qty)));
        }

        public IEnumerable<ModifierToAdd> GetCommonModifiers()
        {
            return _selectedModifiersGroups.Where(vm => vm.Type == ModifierType.Common)
                .SelectMany(vm => vm.Modifiers.Select(itemVm => new ModifierToAdd(itemVm.ToGuid(), itemVm.Qty)));
        }

        #endregion

        #region Private methods

        private void OnSelectedItemsChanged(NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (IModifierItem newItem in args.NewItems)
                    {
                        OnItemAdded(newItem);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (IModifierItem oldItem in args.OldItems)
                    {
                        OnItemRemoved(oldItem);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    OnSelectedItemsReset();
                    break;
            }
        }

        private void OnItemAdded(IModifierItem item)
        {
            ModifiersGroupItemVm group = GetGroupForModifier(item);
            switch (item.Type)
            {
                case ModifierType.Related:
                    if (group != null)
                    {
                        group.AddModifier(new ModifierItemVm(item, _itemConstructor));
                    }
                    else
                    {
                        group = CreateGroupForRelatedModifier(item);
                        group.AddModifier(new ModifierItemVm(item, _itemConstructor));
                        _selectedModifiersGroups.Add(group);
                    }
                    break;
                case ModifierType.Common:
                    group.AddModifier(new ModifierItemVm(item, _itemConstructor));
                    break;
            }
        }

        private void OnItemRemoved(IModifierItem item)
        {
            switch (item.Type)
            {
                case ModifierType.Related:
                    foreach (var group in _selectedModifiersGroups.Where(vm => vm.Type == ModifierType.Related))
                    {
                        if (group.TryRemoveModifier(item)) break;
                    }
                    break;
                case ModifierType.Common:
                    foreach (var group in _selectedModifiersGroups.Where(vm => vm.Type == ModifierType.Common))
                    {
                        if (group.TryRemoveModifier(item)) break;
                    }
                    break;
            }
        }

        private void OnSelectedItemsReset()
        {
            _selectedModifiersGroups.Clear();
            foreach (var selectedItem in _itemConstructor.SelectedItems)
            {
                OnItemAdded(selectedItem);
            }
        }

        private void UpdateGroupInfoOnModifiersChanged(IReadOnlyList<IModifierItem> modifiers)
        {
            if (modifiers.Count != 0)
            {
                UpdateModifiersGroupInfo(modifiers[0].GroupMaxQty, modifiers[0].IsRequired);
            }
            else
            {
                UpdateModifiersGroupInfo(-1, false);
            }
        }

        private void OnGroupSelected(ModifiersGroupItemVm group)
        {
            UpdateModifiersGroupInfo(group.MaxQty, group.IsRequired, group.Title);
            switch (group.Type)
            {
                case ModifierType.Related:
                    var modifier = group.Modifiers.FirstOrDefault();
                    if (modifier != null)
                    {
                        _itemConstructor.ShowRelatedModifiers(modifier.ToGuid());
                    }
                    else
                    {
                        _itemConstructor.ShowRelatedModifiers(null, group.GroupNumber);
                    }
                    break;
                case ModifierType.Common:
                    _itemConstructor.ShowCommonModifiers(group.Id.Value);
                    break;
            }
        }

        private ModifiersGroupItemVm GetGroupForModifier(IModifierItem modifier)
        {
            switch (modifier.Type)
            {
                case ModifierType.Related:
                    int groupNumber = modifier.GroupNumber;
                    return _selectedModifiersGroups.Where(vm => vm.Type == ModifierType.Related)
                        .FirstOrDefault(vm => vm.GroupNumber == groupNumber);
                case ModifierType.Common:
                    Guid? parentId = modifier.ParentId;
                    return _selectedModifiersGroups.Where(vm => vm.Type == ModifierType.Common)
                        .FirstOrDefault(vm => vm.Id == parentId);
            }
            return null;
        }

        private static ModifiersGroupItemVm CreateGroupForRelatedModifier(IModifierItem modifier)
        {
            var g = new ModifiersGroupItemVm(ModifierType.Related, null, modifier.GroupNumber)
            {
                IsRequired = modifier.IsRequired,
                MaxQty = modifier.GroupMaxQty
            };
            return g;
        }

        private IEnumerable<ModifiersGroupItemVm> GetCommonModifiersGroups()
        {
            return _itemConstructor.GetCommonModifierGroups()
                .Select(group => new ModifiersGroupItemVm(ModifierType.Common, group.Id, -1)
                {
                    Title = group.Title
                });
        } 

        private void UpdateModifiersGroupInfo(int maxQty, bool isRequired, string title = default (string))
        {
            GroupTitle = title;
            MaxGroupQty = maxQty;
            RequiredStatus = isRequired
                ? Properties.Resources.Modifier_RequiredStatus_Required
                : Properties.Resources.Modifier_RequiredStatus_Optional;
        }

        #endregion
    }
}
