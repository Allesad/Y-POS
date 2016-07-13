using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

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
            "CategoriesColumns", typeof (int), typeof (MenuSelectorControl), new PropertyMetadata(5));

        public int CategoriesColumns
        {
            get { return (int) GetValue(CategoriesColumnsProperty); }
            set { SetValue(CategoriesColumnsProperty, value); }
        }

        #endregion

        #region Categories rows property

        public static readonly DependencyProperty CategoriesRowsProperty = DependencyProperty.Register(
            "CategoriesRows", typeof (int), typeof (MenuSelectorControl), new PropertyMetadata(1));

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


        }

        #endregion

        #region Private methods

        private void BindCategories(string partName)
        {
            var categoriesList = GetTemplateChild(partName) as Selector;

            categoriesList.SelectionChanged += CategoriesListOnSelectionChanged;

            var b = new Binding("Items");
            b.Source = this;

            categoriesList.SetBinding(ItemsControl.ItemsSourceProperty, b);
        }

        private void CategoriesListOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        #endregion
    }
}
