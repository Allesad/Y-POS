using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Y_POS.Controls
{
    [TemplatePart(Name = "PART_1", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_2", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_3", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_4", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_5", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_6", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_7", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_8", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_9", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_0", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_Dot", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_Delete", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_Clear", Type = typeof(ButtonBase))]
    public class NumericKeypadControl : Control
    {
        #region Static constructor

        static NumericKeypadControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericKeypadControl),
                new FrameworkPropertyMetadata(typeof(NumericKeypadControl)));
        }

        #endregion

        #region Target TextBox

        public static readonly DependencyProperty TargetBoxProperty = DependencyProperty.Register(
            "TargetBox", typeof (TextBox), typeof (NumericKeypadControl), new PropertyMetadata(default(TextBox)));

        public TextBox TargetBox
        {
            get { return (TextBox) GetValue(TargetBoxProperty); }
            set { SetValue(TargetBoxProperty, value); }
        }

        #endregion

        #region Show separator symbol

        public static readonly DependencyProperty ShowSeparatorSymbolProperty = DependencyProperty.Register(
            "ShowSeparatorSymbol", typeof(bool), typeof(NumericKeypadControl), new PropertyMetadata(default(bool), OnShowSeparatorSymbolChanged));

        private static void OnShowSeparatorSymbolChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            var control = (NumericKeypadControl) o;
            var showSeparator = (bool) args.NewValue;
            var separatorBtn = control.GetTemplateChild("PART_Dot") as Button;
            var clearBtn = control.GetTemplateChild("PART_Clear") as Button;

            if (separatorBtn != null)
            {
                separatorBtn.Visibility = showSeparator ? Visibility.Visible : Visibility.Collapsed;
            }
            if (clearBtn != null)
            {
                clearBtn.Visibility = showSeparator ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public bool ShowSeparatorSymbol
        {
            get { return (bool) GetValue(ShowSeparatorSymbolProperty); }
            set { SetValue(ShowSeparatorSymbolProperty, value); }
        }

        #endregion

        #region ButtonClick event

        public static readonly RoutedEvent ButtonClickEvent =
            EventManager.RegisterRoutedEvent("ButtonClick", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(MenuSelectorControl));

        public event RoutedEventHandler ButtonClick
        {
            add { AddHandler(ButtonClickEvent, value); }
            remove { RemoveHandler(ButtonClickEvent, value); }
        }

        #endregion

        #region Overridden methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            BindButton("PART_1");
            BindButton("PART_2");
            BindButton("PART_3");
            BindButton("PART_4");
            BindButton("PART_5");
            BindButton("PART_6");
            BindButton("PART_7");
            BindButton("PART_8");
            BindButton("PART_9");
            BindButton("PART_0");
            BindButton("PART_Dot");
            BindButton("PART_Delete");
            BindButton("PART_Clear");
        }

        #endregion

        #region Private methods

        private void BindButton(string partName)
        {
            var btn = GetTemplateChild(partName) as ButtonBase;

            if (btn == null) return;

            if (partName.Equals("PART_Dot", StringComparison.Ordinal))
            {
                btn.Content = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                btn.Visibility = ShowSeparatorSymbol ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (partName.Equals("PART_Clear"))
            {
                btn.Visibility = ShowSeparatorSymbol ? Visibility.Collapsed : Visibility.Visible;
            }
            btn.Click += BtnOnClick;
        }

        private void BtnOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var btn = (ButtonBase) sender;

            NumericKeypadButtonCode code = NumericKeypadButtonCode.None;
            switch (btn.Name)
            {
                case "PART_1":
                    code = NumericKeypadButtonCode.Button1;
                    break;
                case "PART_2":
                    code = NumericKeypadButtonCode.Button2;
                    break;
                case "PART_3":
                    code = NumericKeypadButtonCode.Button3;
                    break;
                case "PART_4":
                    code = NumericKeypadButtonCode.Button4;
                    break;
                case "PART_5":
                    code = NumericKeypadButtonCode.Button5;
                    break;
                case "PART_6":
                    code = NumericKeypadButtonCode.Button6;
                    break;
                case "PART_7":
                    code = NumericKeypadButtonCode.Button7;
                    break;
                case "PART_8":
                    code = NumericKeypadButtonCode.Button8;
                    break;
                case "PART_9":
                    code = NumericKeypadButtonCode.Button9;
                    break;
                case "PART_0":
                    code = NumericKeypadButtonCode.Button0;
                    break;
                case "PART_Dot":
                    code = NumericKeypadButtonCode.Dot;
                    break;
                case "PART_Delete":
                    code = NumericKeypadButtonCode.Delete;
                    break;
                case "PART_Clear":
                    code = NumericKeypadButtonCode.Clear;
                    break;
            }

            if (code == NumericKeypadButtonCode.None) return;

            if (TargetBox != null)
            {
                ProcessClick(code);
            }
            else
            {
                var args = new RoutedEventArgs(ButtonClickEvent, code);
                RaiseEvent(args);
            }
        }

        private void ProcessClick(NumericKeypadButtonCode code)
        {
            switch (code)
            {
                case NumericKeypadButtonCode.Clear:
                    TargetBox.Clear();
                    return;
                case NumericKeypadButtonCode.Delete:
                    if (TargetBox.SelectionLength > 0)
                    {
                        TargetBox.SelectedText = string.Empty;
                        return;
                    }
                    if (TargetBox.Text.Length > 0)
                    {
                        TargetBox.Text = TargetBox.Text.Remove(TargetBox.Text.Length - 1);
                    }
                    return;
                case NumericKeypadButtonCode.Dot:
                    TargetBox.Text += CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                    return;
                case NumericKeypadButtonCode.Button0:
                    SetText("0");
                    return;
                case NumericKeypadButtonCode.Button1:
                    SetText("1");
                    return;
                case NumericKeypadButtonCode.Button2:
                    SetText("2");
                    return;
                case NumericKeypadButtonCode.Button3:
                    SetText("3");
                    return;
                case NumericKeypadButtonCode.Button4:
                    SetText("4");
                    return;
                case NumericKeypadButtonCode.Button5:
                    SetText("5");
                    return;
                case NumericKeypadButtonCode.Button6:
                    SetText("6");
                    return;
                case NumericKeypadButtonCode.Button7:
                    SetText("7");
                    return;
                case NumericKeypadButtonCode.Button8:
                    SetText("8");
                    return;
                case NumericKeypadButtonCode.Button9:
                    SetText("9");
                    return;
            }
        }

        private void SetText(string text)
        {
            if (TargetBox.SelectionLength > 0)
            {
                TargetBox.SelectedText = text;
                TargetBox.SelectionLength = 0;
                TargetBox.CaretIndex = TargetBox.Text.Length - 1;
            }
            else
            {
                TargetBox.Text += text;
            }
        }

        #endregion

        public enum NumericKeypadButtonCode
        {
            None = 0,
            Button1,
            Button2,
            Button3,
            Button4,
            Button5,
            Button6,
            Button7,
            Button8,
            Button9,
            Button0,
            Dot,
            Delete,
            Clear
        }
    }
}
