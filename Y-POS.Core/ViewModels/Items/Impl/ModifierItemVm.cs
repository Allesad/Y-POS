using System;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Builders;
using YumaPos.Client.Common;
using YumaPos.Client.Extensions;
using YumaPos.Client.UI.ViewModels.Impl;
using Y_POS.Core.Extensions;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Core.ViewModels.Items.Impl
{
    public sealed class ModifierItemVm : BaseVm, IModifierItemVm
    {
        #region Fields

        private readonly IModifierItem _item;
        private readonly IOrderItemConstructor _itemConstructor;

        #endregion

        #region Properties

        public string Uuid { get; }
        public IImageModel ImageModel { get; }
        public string Title { get; }
        public decimal Price { get; }
        [Reactive]
        public int Qty { get; private set; }

        public int GroupMaxQty { get; }
        public int MaxQty { get; }
        public bool CanModifyQty { get; }
        public bool IsSkipOption { get; }

        #endregion

        #region Commands

        public ICommand CommandSelectModifier { get; }
        public ICommand CommandIncreaseQty { get; }
        public ICommand CommandDecreaseQty { get; }

        #endregion

        #region Constructor

        public ModifierItemVm(IModifierItem item, IOrderItemConstructor itemConstructor)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (itemConstructor == null) throw new ArgumentNullException(nameof(itemConstructor));

            _item = item;
            _itemConstructor = itemConstructor;

            Uuid = item.Id.ToString();
            Title = item.IsSkipOption ? Properties.Resources.Next : item.Title;
            Qty = item.Qty;
            Price = item.Price;
            IsSkipOption = item.IsSkipOption;
            ImageModel = ImageService.GetImage(item.ImageId);

            MaxQty = item.MaxQty;
            GroupMaxQty = item.GroupMaxQty;
            CanModifyQty = item.MaxQty > 1;

            // Track quantity and calculate total item amount
            this.WhenAnyValue(vm => vm._item.Qty).Skip(1)
                .SubscribeToObserveOnUi(i => Qty = i);

            // Commands increase/decrease
            var cmdIncrease = ReactiveCommand.Create(this.WhenAnyValue(vm => vm.Qty).Select(qty => qty < MaxQty));
            var cmdDecrease = ReactiveCommand.Create();

            cmdIncrease.Select(param => (IModifierItemVm) param)
                .SubscribeToObserveOnUi(vm => _itemConstructor.IncrementModifierQty(vm.ToGuid()));
            cmdDecrease.Select(param => (IModifierItemVm) param)
                .SubscribeToObserveOnUi(vm => _itemConstructor.DecrementModifierQty(vm.ToGuid()));

            CommandIncreaseQty = cmdIncrease;
            CommandDecreaseQty = cmdDecrease;

            // Command select
            //var canExecuteSelect = this.WhenAny(vm => vm.Qty, vm => vm.MaxQty, (qty, maxQty) => qty.Value < maxQty.Value);
            var cmdSelect = ReactiveCommand.Create();
            cmdSelect.Select(param => (IModifierItemVm) param)
                .Subscribe(vm =>
                {
                    if (vm.IsSkipOption)
                    {
                        _itemConstructor.Skip();
                        return;
                    }
                    _itemConstructor.SelectModifier(vm.ToGuid());
                });
            CommandSelectModifier = cmdSelect;
        }

        #endregion

        public bool Equals(IIdentifiable other)
        {
            return other != null && Equals(Uuid, other.Uuid);
        }
    }
}
