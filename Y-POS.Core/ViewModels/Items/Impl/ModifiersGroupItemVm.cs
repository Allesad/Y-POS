using System;
using System.Linq;
using ReactiveUI;
using YumaPos.Client.Builders;
using YumaPos.Client.Extensions;
using YumaPos.Client.UI.ViewModels.Impl;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Core.ViewModels.Items.Impl
{
    public sealed class ModifiersGroupItemVm : BaseVm, IModifiersGroupItemVm
    {
        #region Fields

        private readonly ReactiveList<IModifierItemVm> _modifiers;

        #endregion

        #region Properties

        internal ModifierType Type { get; }
        internal Guid? Id { get; private set; }
        internal int GroupNumber { get; private set; }

        public string Title { get; internal set; }
        public bool IsTitleVisible => !string.IsNullOrEmpty(Title);
        public bool IsRequired { get; internal set; }
        public int MaxQty { get; internal set; }
        public IReadOnlyReactiveList<IModifierItemVm> Modifiers => _modifiers;

        #endregion

        #region Constructor

        public ModifiersGroupItemVm(ModifierType type, Guid? id, int groupNumber)
        {
            if (type == ModifierType.Related && groupNumber < 0)
                throw new ArgumentOutOfRangeException(nameof(groupNumber), "Group number for related modifiers group should be > 0");
            if (type == ModifierType.Common && id == null)
                throw new ArgumentNullException(nameof(id), "Group Id for common modifier group shouldn't be null");

            Type = type;
            Id = id;
            GroupNumber = groupNumber;

            _modifiers = new ReactiveList<IModifierItemVm>
            {
                ChangeTrackingEnabled = true
            };
        }

        #endregion

        #region Internal methods

        internal void AddModifier(IModifierItemVm modifier)
        {
            if (modifier == null) throw new ArgumentNullException(nameof(modifier));

            _modifiers.Add(modifier);
        }

        internal bool TryRemoveModifier(IModifierItem modifier)
        {
            var vm = _modifiers.FirstOrDefault(itemVm => itemVm.ToGuid() == modifier.Id);
            if (vm == null) return false;

            _modifiers.Remove(vm);
            return true;
        }

        #endregion
    }
}
