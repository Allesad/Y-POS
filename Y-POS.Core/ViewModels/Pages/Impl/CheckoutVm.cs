using System;
using System.Collections.Generic;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Common;
using YumaPos.Client.UI.ViewModels.Contracts;
using YumaPos.Client.UI.ViewModels.Impl;
using Y_POS.Core.ViewModels.PageParts;

namespace Y_POS.Core.ViewModels.Pages
{
    public sealed class CheckoutVm : PageVm, ICheckoutVm
    {
        #region Fields

        private readonly ISelectCustomerVm _selectCustomerVm;

        #endregion

        #region Properties

        [Reactive]
        public IBaseVm OptionVm { get; private set; }

        #endregion

        #region Constructor

        public CheckoutVm(ISelectCustomerVm selectCustomerVm)
        {
            if (selectCustomerVm == null) throw new ArgumentNullException(nameof(selectCustomerVm));

            _selectCustomerVm = selectCustomerVm;
        }

        #endregion

        #region Lifecycle

        protected override IEnumerable<ILifecycleVm> GetChildren()
        {
            return new ILifecycleVm[] { _selectCustomerVm };
        }

        protected override void InitCommands()
        {
            
        }

        protected override void InitLifetimeSubscriptions()
        {
            
        }

        protected override void OnCreate(IArgsBundle args)
        {
            OptionVm = _selectCustomerVm;
        }

        #endregion
    }
}
