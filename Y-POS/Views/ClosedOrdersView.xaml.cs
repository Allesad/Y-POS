using System;
using System.Windows;
using System.Windows.Controls;
using Y_POS.Core.ViewModels.Pages;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for ClosedOrdersView.xaml
    /// </summary>
    public partial class ClosedOrdersView : BaseView
    {
        #region Properties

        public ClosedOrdersVm ViewModel => DataContext as ClosedOrdersVm;

        #endregion

        public ClosedOrdersView()
        {
            InitializeComponent();

            Paginator.PageSizes = new[] {10, 25, 50, 100};
        }

        protected override void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            base.OnLoaded(sender, routedEventArgs);

            //SetItemsPerPage(int.Parse(((ComboBoxItem) ItemsPerPageCb.SelectedItem).Tag.ToString()));
        }

        private void OnItemsPerPageChanged(object sender, SelectionChangedEventArgs e)
        {
            //SetItemsPerPage(int.Parse(((ComboBoxItem)e.AddedItems[0]).Tag.ToString()));
        }

        private void SetItemsPerPage(int itemsPerPage)
        {
            if (itemsPerPage <= 1) throw new ArgumentOutOfRangeException(nameof(itemsPerPage));

            ViewModel?.CommandItemsPerPage.Execute(itemsPerPage);
        }
    }
}
