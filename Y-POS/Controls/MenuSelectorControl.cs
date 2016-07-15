using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;

namespace Y_POS.Controls
{
    [TemplatePart(Name = "PART_Categories", Type = typeof(Selector))]
    [TemplatePart(Name = "PART_CategoryItems", Type = typeof(ItemsControl))]
    [TemplatePart(Name = "PART_CategoriesNext", Type = typeof(Button))]
    [TemplatePart(Name = "PART_CategoriesBack", Type = typeof(Button))]
    [TemplatePart(Name = "PART_ItemsNext", Type = typeof(Button))]
    [TemplatePart(Name = "PART_ItemsBack", Type = typeof(Button))]
    class MenuSelectorControl : Control
    {
        static MenuSelectorControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuSelectorControl), 
                new FrameworkPropertyMetadata(typeof(MenuSelectorControl)));
        }

        #region Categories property

        public static readonly DependencyProperty CategoriesProperty = DependencyProperty.Register(
            "Categories", typeof (IEnumerable), typeof (MenuSelectorControl), new PropertyMetadata(default(IEnumerable)));

        public IEnumerable Categories
        {
            get { return (IEnumerable) GetValue(CategoriesProperty); }
            set { SetValue(CategoriesProperty, value); }
        }

        #endregion

        #region Category Items property

        public static readonly DependencyProperty CategoryItemsProperty = DependencyProperty.Register(
            "CategoryItems", typeof (IEnumerable), typeof (MenuSelectorControl), new PropertyMetadata(default(IEnumerable)));

        public IEnumerable CategoryItems
        {
            get { return (IEnumerable) GetValue(CategoryItemsProperty); }
            set { SetValue(CategoryItemsProperty, value); }
        }

        #endregion

        #region Categories columns property

        public static readonly DependencyProperty CategoriesColumnsProperty = DependencyProperty.Register(
            "CategoriesColumns", typeof (int), typeof (MenuSelectorControl), new FrameworkPropertyMetadata(5, null,
                (o, value) =>
                {
                    int v = (int) value;
                    if (v < 2) return 2;
                    if (v > 5) return 5;
                    return v;
                }));

        /// <summary>
        /// Min: 2, Max: 5
        /// </summary>
        public int CategoriesColumns
        {
            get { return (int) GetValue(CategoriesColumnsProperty); }
            set { SetValue(CategoriesColumnsProperty, value); }
        }

        #endregion

        #region Categories rows property

        public static readonly DependencyProperty CategoriesRowsProperty = DependencyProperty.Register(
            "CategoriesRows", typeof (int), typeof (MenuSelectorControl), new FrameworkPropertyMetadata(1, null,
                (o, value) =>
                {
                    int v = (int) value;
                    if (v < 1) return 1;
                    if (v > 2) return 2;
                    return v;
                }));

        /// <summary>
        /// Min: 1, Max: 2
        /// </summary>
        public int CategoriesRows
        {
            get { return (int) GetValue(CategoriesRowsProperty); }
            set { SetValue(CategoriesRowsProperty, value); }
        }

        #endregion

        #region Category items columns property

        public static readonly DependencyProperty CategoryItemsColumnsProperty = DependencyProperty.Register(
            "CategoryItemsColumns", typeof (int), typeof (MenuSelectorControl), new PropertyMetadata(5));

        public int CategoryItemsColumns
        {
            get { return (int) GetValue(CategoryItemsColumnsProperty); }
            set { SetValue(CategoryItemsColumnsProperty, value); }
        }

        #endregion

        #region Category items rows property

        public static readonly DependencyProperty CategoryItemsRowsProperty = DependencyProperty.Register(
            "CategoryItemsRows", typeof (int), typeof (MenuSelectorControl), new PropertyMetadata(4));

        public int CategoryItemsRows
        {
            get { return (int) GetValue(CategoryItemsRowsProperty); }
            set { SetValue(CategoryItemsRowsProperty, value); }
        }

        #endregion

        #region CategoryChanged event

        public static readonly RoutedEvent CategoryChangedEvent = 
            EventManager.RegisterRoutedEvent("CategoryChanged", RoutingStrategy.Bubble, 
                typeof(RoutedPropertyChangedEventHandler<object>), typeof(MenuSelectorControl));

        public event RoutedPropertyChangedEventHandler<object> CategoryChanged
        {
            add { AddHandler(CategoryChangedEvent, value); }
            remove { RemoveHandler(CategoryChangedEvent, value); }
        }

        #endregion

        #region CategoryItemSelected event

        public static readonly RoutedEvent CategoryItemSelectedEvent =
            EventManager.RegisterRoutedEvent("CategoryItemSelected", RoutingStrategy.Bubble,
                typeof (RoutedPropertyChangedEventHandler<object>), typeof (MenuSelectorControl));

        public event RoutedPropertyChangedEventHandler<object> CategoryItemSelected
        {
            add { AddHandler(CategoryItemSelectedEvent, value); }
            remove { RemoveHandler(CategoryItemSelectedEvent, value); }
        }

        #endregion

        #region Overriden styles

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            BindCategories("PART_Categories");
            BindCategoryItems("PART_CategoryItems");
        }

        #endregion

        #region Private methods

        private void BindCategories(string partName)
        {
            var categoriesList = GetTemplateChild(partName) as Selector;

            var contentList = new List<string>
            {
                "Coffee",
                "Tea",
                "Desserts",
                "Meat",
                "Fish",
                "Drinks"
            };

            categoriesList.ItemsSource = contentList;
        }

        private void BindCategoryItems(string partName)
        {
            var categoriesItemsList = GetTemplateChild(partName) as ItemsControl;

            var contentList = new List<Button>
            {
                new Button(){Content = "Chocolate mousse with prunes"},
                new Button(){Content = "Italian-style bakewell tart"},
                new Button(){Content = "Lemon Sorbet"},
                new Button(){Content = "Chocolate mousse with prunes"},
                new Button(){Content = "Italian-style bakewell tart"},
                new Button(){Content = "Lemon Sorbet"},
                new Button(){Content = "Chocolate mousse with prunes"},
                new Button(){Content = "Italian-style bakewell tart"},
                new Button(){Content = "Lemon Sorbet"},
                new Button(){Content = "Chocolate mousse with prunes"},
                new Button(){Content = "Italian-style bakewell tart"},
                new Button(){Content = "Lemon Sorbet"},
                new Button(){Content = "Chocolate mousse with prunes"},
                new Button(){Content = "Italian-style bakewell tart"},
                new Button(){Content = "Lemon Sorbet"},
                new Button(){Content = "Chocolate mousse with prunes"},
                new Button(){Content = "Italian-style bakewell tart"},
                new Button(){Content = "Lemon Sorbet"},
                new Button(){Content = "Chocolate mousse with prunes"},
                new Button(){Content = "Italian-style bakewell tart"},
                new Button(){Content = "Lemon Sorbet"},
                new Button(){Content = "Chocolate mousse with prunes"},
                new Button(){Content = "Italian-style bakewell tart"},
                new Button(){Content = "Lemon Sorbet"},
            };

            categoriesItemsList.ItemsSource = contentList;
        }

        private void CategoriesListOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        #endregion
    }
}
