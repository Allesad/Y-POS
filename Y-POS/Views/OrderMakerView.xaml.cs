using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for OrderMakerView.xaml
    /// </summary>
    public partial class OrderMakerView : UserControl
    {
        private readonly Random _rnd = new Random();

        public OrderMakerView()
        {
            InitializeComponent();

            MSC.Categories = new ReactiveList<object>
                {
                    new Category("Coffee", _rnd.Next(10,30)), 
                    new Category("Tea", _rnd.Next(10,30)), 
                    new Category("Desserts", _rnd.Next(10,30)), 
                    new Category("Meat", _rnd.Next(10,30)), 
                    new Category("Fish", _rnd.Next(10,30)), 
                    new Category("Drinks", _rnd.Next(10,30)), 
                };
        }

        private void AddCategory(object sender, RoutedEventArgs e)
        {
            ((ReactiveList<object>) MSC.Categories)?.Add(new Category("Some cat", _rnd.Next(20)));
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

        private void SwitchGridSize(object sender, RoutedEventArgs e)
        {
            MSC.CategoriesColumns = MSC.CategoriesColumns == 3 ? 5 : 3;
        }

        private void MSC_OnCategoryChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var category = e.NewValue as Category;

            if (category == null) return;

            MSC.CategoryItems = category.Items;
        }

        private void MSC_OnCategoryItemSelected(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MessageBox.Show(string.Format("Selected '{0}'", e.NewValue));
        }
    }
}
