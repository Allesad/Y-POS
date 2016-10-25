using System;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Builders;
using YumaPos.Client.Common;
using YumaPos.Client.UI.ViewModels.Impl;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Core.ViewModels.Items.Impl
{
    public class OrderedItemVm : BaseVm, IOrderedItemVm
    {
        #region Fields

        private readonly IOrderedItem _model;

        #endregion

        #region Properties

        public string Uuid => _model.Uuid;
        public string Title => _model.Title;
        [ObservableAsProperty]
        public extern string Description { get; }
        [ObservableAsProperty]
        public extern decimal Price { get; }
        [ObservableAsProperty]
        public extern int Qty { get; }

        #endregion

        #region Constructor

        public OrderedItemVm(IOrderedItem model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            _model = model;

            this.WhenAnyValue(vm => vm._model.Qty).ToPropertyEx(this, vm => vm.Qty);
            this.WhenAnyValue(vm => vm._model.TotalPrice).ToPropertyEx(this, vm => vm.Price);
            this.WhenAnyValue(vm => vm._model.MenuItems)
                .Select(
                    items =>
                        string.Join(" / ",
                            items.Select(item => $"{item.Title} - {item.Qty.ToString()} x {item.Price:C}")))
                .ToPropertyEx(this, vm => vm.Description);
        }

        #endregion

        public bool Equals(IIdentifiable other)
        {
            return other != null && Equals(Uuid, other.Uuid);
        }

    }
}
