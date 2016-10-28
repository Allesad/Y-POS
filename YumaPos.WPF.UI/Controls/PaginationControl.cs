using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace YumaPos.WPF.UI.Controls
{
    public enum PageNumberDisplayType
    {
        PageOnly,
        WithPagesCount
    }

    [TemplatePart(Name = "PART_CurrentPage", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_Begin", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_End", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_PrevPage", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_NextPage", Type = typeof(ButtonBase))]
    public class PaginationControl : Control
    {
        #region Fields

        private static readonly int[] DefaultPageSizes = { 10 };

        #endregion

        #region Contructors

        static PaginationControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PaginationControl),
                new FrameworkPropertyMetadata(typeof(PaginationControl)));
        }

        #endregion

        #region Begin command

        public static readonly DependencyProperty BeginProperty = DependencyProperty.Register(
            "Begin", typeof(ICommand), typeof(PaginationControl), new PropertyMetadata(default(ICommand),
                (o, args) =>
                {
                    ((PaginationControl)o).BindCommand("PART_Begin");
                }));

        public ICommand Begin
        {
            get { return (ICommand)GetValue(BeginProperty); }
            set { SetValue(BeginProperty, value); }
        }

        #endregion

        #region End command

        public static readonly DependencyProperty EndProperty = DependencyProperty.Register(
            "End", typeof(ICommand), typeof(PaginationControl), new PropertyMetadata(default(ICommand), (o, args) =>
            {
                ((PaginationControl)o).BindCommand("PART_End");
            }));

        public ICommand End
        {
            get { return (ICommand)GetValue(EndProperty); }
            set { SetValue(EndProperty, value); }
        }

        #endregion

        #region Previous page command

        public static readonly DependencyProperty PrevPageProperty = DependencyProperty.Register(
            "PrevPage", typeof(ICommand), typeof(PaginationControl), new PropertyMetadata(default(ICommand),
                (o, args) =>
                {
                    ((PaginationControl)o).BindCommand("PART_PrevPage");
                }));

        public ICommand PrevPage
        {
            get { return (ICommand)GetValue(PrevPageProperty); }
            set { SetValue(PrevPageProperty, value); }
        }

        #endregion

        #region Next page command

        public static readonly DependencyProperty NextPageProperty = DependencyProperty.Register(
            "NextPage", typeof(ICommand), typeof(PaginationControl), new PropertyMetadata(default(ICommand),
                (o, args) =>
                {
                    ((PaginationControl)o).BindCommand("PART_NextPage");
                }));

        public ICommand NextPage
        {
            get { return (ICommand)GetValue(NextPageProperty); }
            set { SetValue(NextPageProperty, value); }
        }

        #endregion

        #region Current page

        public static readonly DependencyProperty CurrentPageProperty = DependencyProperty.Register(
            "CurrentPage", typeof(int), typeof(PaginationControl),
            new PropertyMetadata(0));

        public int CurrentPage
        {
            get { return (int)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }

        #endregion

        #region Pages count

        public static readonly DependencyProperty PagesCountProperty = DependencyProperty.Register(
            "PagesCount", typeof(int), typeof(PaginationControl), new PropertyMetadata(default(int)));

        public int PagesCount
        {
            get { return (int) GetValue(PagesCountProperty); }
            set { SetValue(PagesCountProperty, value); }
        }

        #endregion

        #region PageSizes

        public static readonly DependencyProperty PageSizesProperty = DependencyProperty.Register(
            "PageSizes", typeof(IEnumerable<int>), typeof(PaginationControl),
            new PropertyMetadata(DefaultPageSizes));

        public IEnumerable<int> PageSizes
        {
            get { return (IEnumerable<int>)GetValue(PageSizesProperty); }
            set { SetValue(PageSizesProperty, value); }
        }

        #endregion

        #region Selected PageSize

        public static readonly DependencyProperty SelectedPageSizeProperty = DependencyProperty.Register(
            "SelectedPageSize", typeof(int), typeof(PaginationControl), new PropertyMetadata(DefaultPageSizes[0]));

        public int SelectedPageSize
        {
            get { return (int)GetValue(SelectedPageSizeProperty); }
            set { SetValue(SelectedPageSizeProperty, value); }
        }

        #endregion

        #region Page number display type

        public static readonly DependencyProperty PageDisplayTypeProperty = DependencyProperty.Register(
            "PageDisplayType", typeof(PageNumberDisplayType), typeof(PaginationControl), new PropertyMetadata(default(PageNumberDisplayType)));

        public PageNumberDisplayType PageDisplayType
        {
            get { return (PageNumberDisplayType) GetValue(PageDisplayTypeProperty); }
            set { SetValue(PageDisplayTypeProperty, value); }
        }

        #endregion

        #region Overridden methods

        public override void OnApplyTemplate()
        {
            BindCurrentPage();
            BindCommand("PART_Begin");
            BindCommand("PART_End");
            BindCommand("PART_PrevPage");
            BindCommand("PART_NextPage");
            BindPageSizes();
            BindSelectedPageSize();
        }

        #endregion

        #region Private methods

        private void BindCurrentPage()
        {
            var text = GetTemplateChild("PART_CurrentPage") as TextBlock;
            if (text == null)
                return;

            /*var binding = new Binding("CurrentPage")
            {
                Source = this,
                Mode = BindingMode.OneWay,

            };*/

            var b = new MultiBinding {Converter = new PageNumberToTextConverter()};
            b.Bindings.Add(new Binding("PageDisplayType") { Source = this, Mode = BindingMode.OneWay });
            b.Bindings.Add(new Binding("CurrentPage") {Source = this, Mode = BindingMode.OneWay});
            b.Bindings.Add(new Binding("PagesCount") {Source = this, Mode = BindingMode.OneWay});
            b.NotifyOnSourceUpdated = true;

            text.SetBinding(TextBlock.TextProperty, b);
        }

        private void BindCommand(string partName)
        {
            var btn = GetTemplateChild(partName) as ButtonBase;
            if (btn == null)
                return;

            Binding binding = null;
            switch (partName)
            {
                case "PART_Begin":
                    if (Begin == null)
                        return;
                    binding = new Binding("Begin");
                    break;
                case "PART_End":
                    if (End == null)
                        return;
                    binding = new Binding("End");
                    break;
                case "PART_PrevPage":
                    if (PrevPage == null)
                        return;
                    binding = new Binding("PrevPage");
                    break;
                case "PART_NextPage":
                    if (NextPage == null)
                        return;
                    binding = new Binding("NextPage");
                    break;
            }
            binding.Source = this;
            binding.Mode = BindingMode.OneTime;

            btn.SetBinding(ButtonBase.CommandProperty, binding);
        }

        private void BindPageSizes()
        {
            var selector = GetTemplateChild("PART_PageSizeSelector") as Selector;
            if (selector == null || PageSizes == null)
                return;

            var binding = new Binding("PageSizes")
            {
                Source = this,
                Mode = BindingMode.OneWay
            };

            selector.SetBinding(ItemsControl.ItemsSourceProperty, binding);
        }

        private void BindSelectedPageSize()
        {
            var selector = GetTemplateChild("PART_PageSizeSelector") as Selector;
            if (selector == null)
                return;

            var binding = new Binding("SelectedPageSize")
            {
                Source = this,
                Mode = BindingMode.TwoWay
            };

            selector.SetBinding(Selector.SelectedItemProperty, binding);
        }

        #endregion

        private class PageNumberToTextConverter : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                var displayType = (PageNumberDisplayType) values[0];
                var pageNumber = (int) values[1];
                var pagesCount = (int) values[2];

                return displayType == PageNumberDisplayType.WithPagesCount
                    ? pageNumber + " / " + pagesCount
                    : pageNumber.ToString("D");
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
