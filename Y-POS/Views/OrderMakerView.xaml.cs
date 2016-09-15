using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Windows;
using ReactiveUI;
using Y_POS.Core.Extensions;
using Y_POS.Core.ViewModels.PageParts;
using Y_POS.Core.ViewModels.Pages;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for OrderMakerView.xaml
    /// </summary>
    public partial class OrderMakerView : BaseView
    {
        //private readonly Random _rnd = new Random();

        private enum DetailsType
        {
            Menu,
            ItemConstructor
        }

        public OrderMakerView()
        {
            InitializeComponent();

            /*MSC.Categories = new ReactiveList<object>
                {
                    new Category("Coffee", _rnd.Next(10,30)), 
                    new Category("Tea", _rnd.Next(10,30)), 
                    new Category("Desserts", _rnd.Next(10,30)), 
                    new Category("Meat", _rnd.Next(10,30)), 
                    new Category("Fish", _rnd.Next(10,30)), 
                    new Category("Drinks", _rnd.Next(10,30)), 
                };*/
        }

        protected override void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            base.OnLoaded(sender, routedEventArgs);

            ((IOrderMakerVm) DataContext).WhenAnyValue(vm => vm.DetailsVm)
                .SubscribeToObserveOnUi(vm => UpdateActionBar(vm is IOrderItemConstructorVm ? DetailsType.ItemConstructor : DetailsType.Menu));
        }

        private void UpdateActionBar(DetailsType type)
        {
            switch (type)
            {
                case DetailsType.ItemConstructor:
                    MenuActions.Visibility = Visibility.Collapsed;
                    ItemConstructorActions.Visibility = Visibility.Visible;
                    break;
                default:
                    MenuActions.Visibility = Visibility.Visible;
                    ItemConstructorActions.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void AddCategory(object sender, RoutedEventArgs e)
        {
            //((ReactiveList<object>) MSC.Categories)?.Add(new Category("Some cat", _rnd.Next(20)));
        }

        private class Category
        {
            private readonly string _title;
            public Category(string title, int count)
            {
                _title = title;

                var lst = new List<CategoryItem>(count);
                for (int i = 0; i < lst.Capacity; i++)
                {
                    lst.Add(new CategoryItem("Item " + i));
                }
                Items = lst;
            }

            public override string ToString()
            {
                return _title;
            }

            public IEnumerable<CategoryItem> Items { get; private set; }
        }

        private class CategoryItem
        {
            private readonly string _title;

            public CategoryItem(string title)
            {
                _title = title;
            }

            public override string ToString()
            {
                return _title;
            }
        }

        private void MSC_OnCategoryChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            /*var category = e.NewValue as Category;

            if (category == null) return;

            MSC.CategoryItems = category.Items;*/
        }

        private void MSC_OnCategoryItemSelected(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //MessageBox.Show(string.Format("Selected '{0}'", e.NewValue));
        }
    }
}
