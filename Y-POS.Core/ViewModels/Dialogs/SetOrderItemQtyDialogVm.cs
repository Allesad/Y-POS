using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Windows.Input;
using DialogManagement.Contracts;
using DialogManagement.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.UI.ViewModels.Impl;

namespace Y_POS.Core.ViewModels.Dialogs
{
    public sealed class SetOrderItemQtyDialogVm : BaseVm, IDialogButtonsConfigProvider
    {
        #region Properties

        public string ItemName { get; }
        [Reactive]
        public int Qty { get; set; }

        #endregion

        #region Commands

        public ICommand CommandIncreaseQty { get; }
        public ICommand CommandDecreaseQty { get; }

        #endregion

        #region Constructor

        public SetOrderItemQtyDialogVm(string itemName, int initialQty)
        {
            if (initialQty <= 0) throw new ArgumentOutOfRangeException(nameof(initialQty), "Quantity cannot be < 0");

            ItemName = itemName;
            Qty = initialQty;

            var cmdIncrease = ReactiveCommand.Create();
            var cmdDecrease = ReactiveCommand.Create(this.WhenAnyValue(dialog => dialog.Qty).Select(qty => qty > 1));

            cmdIncrease.Subscribe(_ => Qty++);
            cmdDecrease.Subscribe(_ => Qty--);

            CommandIncreaseQty = cmdIncrease;
            CommandDecreaseQty = cmdDecrease;
        }

        #endregion

        #region IDialogButtonsConfigProvider

        public IEnumerable<DialogButtonConfig> GetButtons()
        {
            return DefaultButtonSets.OkCancel;
        }

        #endregion
    }
}
