using System.Windows;
using System.Windows.Controls;

namespace Y_POS.Controls
{
    /// <summary>
    /// Interaction logic for CardNumberInput.xaml
    /// </summary>
    public partial class CardNumberInput : UserControl
    {
        public CardNumberInput()
        {
            InitializeComponent();
        }

        #region Card number property

        public static readonly DependencyProperty CardNumberProperty = DependencyProperty.Register(
            "CardNumber", typeof (string), typeof (CardNumberInput), new PropertyMetadata(default(string)));

        public string CardNumber
        {
            get { return (string) GetValue(CardNumberProperty); }
            set { SetValue(CardNumberProperty, value); }
        }

        #endregion
    }
}
