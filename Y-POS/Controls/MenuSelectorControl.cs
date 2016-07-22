using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using ReactiveUI;

namespace Y_POS.Controls
{
    [TemplatePart(Name = "PART_Categories", Type = typeof (Selector))]
    [TemplatePart(Name = "PART_CategoryItems", Type = typeof (ItemsControl))]
    [TemplatePart(Name = "PART_CategoriesNext", Type = typeof (Button))]
    [TemplatePart(Name = "PART_CategoriesBack", Type = typeof (Button))]
    [TemplatePart(Name = "PART_ItemsNext", Type = typeof (Button))]
    [TemplatePart(Name = "PART_ItemsBack", Type = typeof (Button))]
    public class MenuSelectorControl : Control
    {
        static MenuSelectorControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (MenuSelectorControl),
                new FrameworkPropertyMetadata(typeof (MenuSelectorControl)));
        }

        #region Categories property

        public static readonly DependencyProperty CategoriesProperty = DependencyProperty.Register(
            "Categories", typeof (IEnumerable<object>), typeof (MenuSelectorControl),
            new PropertyMetadata(default(IEnumerable<object>), OnCategoriesChanged));

        public IEnumerable<object> Categories
        {
            get { return (IEnumerable<object>) GetValue(CategoriesProperty); }
            set { SetValue(CategoriesProperty, value); }
        }

        private static void OnCategoriesChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = (MenuSelectorControl) obj;

            control.InnerCategories = ((IEnumerable<object>) e.NewValue).Take(control.CategoriesColumns);

            var oldCollection = e.OldValue as INotifyCollectionChanged;
            if (oldCollection != null)
            {
                oldCollection.CollectionChanged -= control.OnCategoriesCollectionChanged;
            }

            var col = e.NewValue as INotifyCollectionChanged;
            if (col != null)
            {
                col.CollectionChanged += control.OnCategoriesCollectionChanged;
            }
        }

        private void OnCategoriesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            InnerCategories = ((IEnumerable<object>) sender)
                .Skip((CurrentCategoriesPage - 1) * CategoriesColumns * CategoriesRows)
                .Take(CategoriesColumns * CategoriesRows);
        }

        #endregion

        #region Categories Page property

        public static readonly DependencyProperty CurrentCategoriesPageProperty = DependencyProperty.Register(
            "CurrentCategoriesPage", typeof (int), typeof (MenuSelectorControl),
            new PropertyMetadata(1, OnCategoriesPageChanged));

        public int CurrentCategoriesPage
        {
            get { return (int) GetValue(CurrentCategoriesPageProperty); }
            set { SetValue(CurrentCategoriesPageProperty, value); }
        }

        private static void OnCategoriesPageChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = (MenuSelectorControl) obj;

            if (control.Categories == null) return;

            control.InnerCategories = control.Categories
                .Skip((control.CurrentCategoriesPage - 1) * control.CategoriesColumns)
                .Take(control.CategoriesColumns);
        }

        #endregion

        #region SelectedCategory property

        public static readonly DependencyProperty SelectedCategoryProperty = DependencyProperty.Register(
            "SelectedCategory", typeof (object), typeof (MenuSelectorControl),
            new PropertyMetadata(default(object), OnSelectedCategoryChanged));

        public object SelectedCategory
        {
            get { return GetValue(SelectedCategoryProperty); }
            set { SetValue(SelectedCategoryProperty, value); }
        }

        private static void OnSelectedCategoryChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = (MenuSelectorControl) obj;

            if (e.NewValue != null)
            {
                control.ItemsPage = 1;
            }

            var args = new RoutedPropertyChangedEventArgs<object>(
                e.OldValue,
                e.NewValue,
                CategoryChangedEvent);
            control.RaiseEvent(args);
        }

        #endregion

        #region Category Items property

        public static readonly DependencyProperty CategoryItemsProperty = DependencyProperty.Register(
            "CategoryItems", typeof (IEnumerable<object>), typeof (MenuSelectorControl),
            new PropertyMetadata(default(IEnumerable<object>), OnCategoryItemsChanged));

        public IEnumerable<object> CategoryItems
        {
            get { return (IEnumerable<object>) GetValue(CategoryItemsProperty); }
            set { SetValue(CategoryItemsProperty, value); }
        }

        private static void OnCategoryItemsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = (MenuSelectorControl) obj;

            control.InnerItems = control.CategoryItems
                .Skip((control.ItemsPage - 1) * control.CategoryItemsColumns * control.CategoryItemsRows)
                .Take(control.CategoryItemsColumns * control.CategoryItemsRows);
        }

        #endregion

        #region ItemsPage property

        public static readonly DependencyProperty ItemsPageProperty = DependencyProperty.Register(
            "ItemsPage", typeof (int), typeof (MenuSelectorControl), new PropertyMetadata(1, OnItemsPageChanged));

        private static void OnItemsPageChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = (MenuSelectorControl) obj;

            control.InnerItems = control.CategoryItems
                .Skip((control.ItemsPage - 1) * control.CategoryItemsColumns * control.CategoryItemsRows)
                .Take(control.CategoryItemsColumns * control.CategoryItemsRows);
        }

        public int ItemsPage
        {
            get { return (int) GetValue(ItemsPageProperty); }
            set { SetValue(ItemsPageProperty, value); }
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
                typeof (RoutedPropertyChangedEventHandler<object>), typeof (MenuSelectorControl));

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

        #region Internal Categories

        private static readonly DependencyProperty InnerCategoriesProperty = DependencyProperty.Register(
            "InnerCategories", typeof (IEnumerable<object>), typeof (MenuSelectorControl),
            new PropertyMetadata(default(IEnumerable<object>)));

        private IEnumerable<object> InnerCategories
        {
            get { return (IEnumerable<object>) GetValue(InnerCategoriesProperty); }
            set { SetValue(InnerCategoriesProperty, value); }
        }

        #endregion

        #region Internal Category Items

        private static readonly DependencyProperty InnerItemsProperty = DependencyProperty.Register(
            "InnerItems", typeof (IEnumerable<object>), typeof (MenuSelectorControl),
            new PropertyMetadata(default(IEnumerable<object>)));

        private IEnumerable<object> InnerItems
        {
            get { return (IEnumerable<object>) GetValue(InnerItemsProperty); }
            set { SetValue(InnerItemsProperty, value); }
        }

        #endregion

        #region Category Item click command

        #endregion

        #region Overriden styles

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            BindCategories("PART_Categories");
            BindCategoryItems("PART_CategoryItems");
            BindCategoriesNavigation("PART_CategoriesNext");
            BindCategoriesNavigation("PART_CategoriesBack");
            BindItemsNavigation("PART_ItemsNext");
            BindItemsNavigation("PART_ItemsBack");
        }

        #endregion

        #region Private methods

        private void BindItemsNavigation(string partName)
        {
            var btn = GetTemplateChild(partName) as ButtonBase;

            if (btn == null)
            {
                throw new Exception(string.Format("Control part with name '{0}' and type {1} is required!", partName,
                    typeof (ButtonBase)));
            }

            IObservable<bool> canExecute;
            Action execute;
            switch (partName)
            {
                case "PART_ItemsNext":
                    canExecute = this.WhenAny(control => control.ItemsPage, control => control.SelectedCategory,
                        (page, seletedCategory) => page.Value < GetItemsPagesCount());
                    execute = () => ItemsPage++;
                    break;
                case "PART_ItemsBack":
                    canExecute = this.WhenAnyValue(control => control.ItemsPage).Select(page => page > 1);
                    execute = () => ItemsPage--;
                    break;
                default:
                    throw new ArgumentException(string.Format("Template part name {0} is not defined", partName));
            }

            var cmd = ReactiveCommand.Create(canExecute);
            cmd.Subscribe(_ => execute());

            btn.Command = cmd;
        }

        private void BindCategoriesNavigation(string partName)
        {
            var btn = GetTemplateChild(partName) as ButtonBase;

            if (btn == null)
            {
                throw new Exception(string.Format("Control part with name '{0}' and type {1} is required!", partName,
                    typeof (ButtonBase)));
            }

            IObservable<bool> canExecute;
            Action execute;
            switch (partName)
            {
                case "PART_CategoriesNext":
                    canExecute = this.WhenAny(control => control.CurrentCategoriesPage, control => control.Categories,
                        (page, categories) => categories.Value != null && page.Value < GetCategoriesPagesCount());
                    execute = () => { CurrentCategoriesPage++; };
                    break;
                case "PART_CategoriesBack":
                    canExecute = this.WhenAnyValue(control => control.CurrentCategoriesPage).Select(page => page > 1);
                    execute = () => { CurrentCategoriesPage--; };
                    break;
                default:
                    throw new ArgumentException(string.Format("Template part name {0} is not defined", partName));
            }
            var cmd = ReactiveCommand.Create(canExecute);

            cmd.Subscribe(_ => execute());

            btn.Command = cmd;
        }

        private void BindCategories(string partName)
        {
            var categoriesList = GetTemplateChild(partName) as Selector;

            if (categoriesList == null)
            {
                throw new Exception(string.Format("Control part with name '{0}' and type {1} is required!", partName,
                    typeof (Selector)));
            }

            // Set ItemsPanel for control
            var columnsBinding = new Binding("CategoriesColumns")
            {
                Source = this,
                Mode = BindingMode.OneWay
            };

            var rowsBinding = new Binding("CategoriesRows")
            {
                Source = this,
                Mode = BindingMode.OneWay
            };

            var elementFactory = new FrameworkElementFactory(typeof (UniformGrid));
            elementFactory.SetValue(Panel.IsItemsHostProperty, true);
            elementFactory.SetBinding(UniformGrid.ColumnsProperty, columnsBinding);
            elementFactory.SetBinding(UniformGrid.RowsProperty, rowsBinding);

            categoriesList.ItemsPanel = new ItemsPanelTemplate(elementFactory);

            // Set items binding
            var innerCategoriesBinding = new Binding("InnerCategories")
            {
                Source = this,
                Mode = BindingMode.OneWay
            };

            categoriesList.SetBinding(ItemsControl.ItemsSourceProperty, innerCategoriesBinding);

            // Set SelectedCategory binding
            var selectedCategoryBinding = new Binding("SelectedCategory")
            {
                Source = this,
                Mode = BindingMode.TwoWay
            };

            categoriesList.SetBinding(Selector.SelectedItemProperty, selectedCategoryBinding);
        }

        private void BindCategoryItems(string partName)
        {
            var categoriesItemsList = GetTemplateChild(partName) as ItemsControl;

            if (categoriesItemsList == null)
            {
                throw new Exception(string.Format("Control part with name '{0}' and type {1} is required!", partName,
                    typeof (ItemsControl)));
            }

            // Set ItemsPanel

            var columnsBinding = new Binding("CategoryItemsColumns")
            {
                Source = this,
                Mode = BindingMode.OneWay
            };

            var rowsBinding = new Binding("CategoryItemsRows")
            {
                Source = this,
                Mode = BindingMode.OneWay
            };

            var elementFactory = new FrameworkElementFactory(typeof (UniformGrid));
            elementFactory.SetValue(Panel.IsItemsHostProperty, true);
            elementFactory.SetBinding(UniformGrid.ColumnsProperty, columnsBinding);
            elementFactory.SetBinding(UniformGrid.RowsProperty, rowsBinding);
            categoriesItemsList.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler((sender, e) =>
            {
                var data = ((FrameworkElement) e.OriginalSource).Tag;
                var args = new RoutedPropertyChangedEventArgs<object>(null, data, CategoryItemSelectedEvent);
                RaiseEvent(args);
            }));

            categoriesItemsList.ItemsPanel = new ItemsPanelTemplate(elementFactory);

            // Set InnerItems binding
            var b = new Binding("InnerItems")
            {
                Source = this,
                Mode = BindingMode.OneWay
            };

            categoriesItemsList.SetBinding(ItemsControl.ItemsSourceProperty, b);
        }

        private int GetCategoriesPagesCount()
        {
            return Categories != null && Categories.Any()
                ? (int) Math.Ceiling(Categories.Count() / (double) CategoriesColumns)
                : 0;
        }

        private int GetItemsPagesCount()
        {
            return CategoryItems != null && CategoryItems.Any()
                ? (int)
                    Math.Ceiling(CategoryItems.Count() / (double) (CategoryItemsColumns * CategoryItemsRows))
                : 0;
        }

        #endregion
    }

    public static class MenuSelectorCommands
    {
        public static readonly RoutedUICommand CategoryItemClick = new RoutedUICommand("Item Click", "CategoryItemClick",
            typeof (MenuSelectorControl));
    }
}