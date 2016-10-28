using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using Y_POS.Controls;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for PinLoginView.xaml
    /// </summary>
    public partial class PinLoginView : BaseView
    {
        #region Fields

        private SecureString _securedPin;

        public SecureString SecuredPin
        {
            get
            {
                _securedPin = _securedPin ?? new SecureString();
                return _securedPin;
            }
        }

        #endregion

        #region Constructor

        public PinLoginView()
        {
            InitializeComponent();

            LoginBtn.CommandParameter = ClockIntBtn.CommandParameter =
                ClockOutBtn.CommandParameter = BreakBtn.CommandParameter = new Func<string>(() =>
                {
                    var ret = SecureStringToString(SecuredPin);
                    OnPinClear();
                    return ret;
                });
        }

        #endregion

        #region Overridden methods

        protected override void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            base.OnUnloaded(sender, routedEventArgs);

            SecuredPin.Dispose();
            _securedPin = null;
        }

        #endregion

        #region Private methods

        private void Keypad_OnButtonClick(object sender, RoutedEventArgs e)
        {
            var key = (NumericKeypadControl.NumericKeypadButtonCode) e.OriginalSource;

            switch (key)
            {
                case NumericKeypadControl.NumericKeypadButtonCode.Delete:
                    OnPinDelete();
                    return;
                case NumericKeypadControl.NumericKeypadButtonCode.Clear:
                    OnPinClear();
                    return;
                case NumericKeypadControl.NumericKeypadButtonCode.Button0:
                    OnPinAdd('0');
                    return;
                case NumericKeypadControl.NumericKeypadButtonCode.Button1:
                    OnPinAdd('1');
                    return;
                case NumericKeypadControl.NumericKeypadButtonCode.Button2:
                    OnPinAdd('2');
                    return;
                case NumericKeypadControl.NumericKeypadButtonCode.Button3:
                    OnPinAdd('3');
                    return;
                case NumericKeypadControl.NumericKeypadButtonCode.Button4:
                    OnPinAdd('4');
                    return;
                case NumericKeypadControl.NumericKeypadButtonCode.Button5:
                    OnPinAdd('5');
                    return;
                case NumericKeypadControl.NumericKeypadButtonCode.Button6:
                    OnPinAdd('6');
                    return;
                case NumericKeypadControl.NumericKeypadButtonCode.Button7:
                    OnPinAdd('7');
                    return;
                case NumericKeypadControl.NumericKeypadButtonCode.Button8:
                    OnPinAdd('8');
                    return;
                case NumericKeypadControl.NumericKeypadButtonCode.Button9:
                    OnPinAdd('9');
                    return;
            }
        }

        private void OnPinAdd(char c)
        {
            if (SecuredPin.Length >= 4) return;

            SecuredPin.AppendChar(c);
            UpdateOutput();
        }

        private void OnPinDelete()
        {
            if (SecuredPin.Length == 0) return;

            SecuredPin.RemoveAt(SecuredPin.Length - 1);
            UpdateOutput();
        }

        private void OnPinClear()
        {
            SecuredPin.Clear();
            UpdateOutput();
        }

        private void UpdateOutput()
        {
            PinOutputTb.Text = new string('*', SecuredPin.Length);
        }

        private static string SecureStringToString(SecureString secureString)
        {
            IntPtr bstr = Marshal.SecureStringToBSTR(secureString);

            try
            {
                return Marshal.PtrToStringBSTR(bstr);
            }
            finally
            {
                Marshal.FreeBSTR(bstr);
            }
        }

        #endregion
    }
}
