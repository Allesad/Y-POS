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

            btn.Click += BtnOnClick;
            /*switch (partName)
            {
                case "PART_1":
                    break;
                case "PART_2":

                    break;
                case "PART_3":

                    break;
                case "PART_4":

                    break;
                case "PART_5":

                    break;
                case "PART_6":

                    break;
                case "PART_7":

                    break;
                case "PART_8":

                    break;
                case "PART_9":

                    break;
                case "PART_0":

                    break;
                case "PART_Dot":

                    break;
                case "PART_Delete":

                    break;
                case "PART_Clear":

                    break;
            }*/
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

            var args = new RoutedEventArgs(ButtonClickEvent, code);
            RaiseEvent(args);
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
