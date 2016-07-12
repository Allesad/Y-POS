using System.Windows.Controls;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            Page.Content = new LoginView();
            /*var vm = new AppMainVm();
            vm.Init();
            DataContext = vm;*/
        }
    }
}
