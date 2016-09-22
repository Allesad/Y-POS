using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DialogManagement.Contracts;
using DialogManagement.Core;

namespace Y_POS
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        private readonly IBaseDialog _dialog;
        private DialogButtonType _resultType = DialogButtonType.Cancel;

        public DialogWindow(IBaseDialog dialog)
        {
            InitializeComponent();

            _dialog = dialog;
            
            if (string.IsNullOrWhiteSpace(dialog.Title))
            {
                HeaderContainer.Visibility = Visibility.Collapsed;
            }

            DataContext = _dialog;

            foreach (var btn in dialog.Buttons.Select(CreateButtonFromConfig))
            {
                ActionsContainer.Children.Add(btn);
                ActionsContainer.Columns++;
            }

            ActionsContainer.Children[0].Focus();
        }

        private Button CreateButtonFromConfig(DialogButtonConfig config)
        {
            var btn = new Button
            {
                Content = !string.IsNullOrEmpty(config.Title) ? config.Title : GetDefaultTitle(config.Type),
                Style = FindResource("DialogActionButton") as Style
            };

            btn.Click += (sender, args) =>
            {
                _resultType = config.Type;
                Close();
            };

            return btn;
        }
        
        private static string GetDefaultTitle(DialogButtonType type)
        {
            switch (type)
            {
                case DialogButtonType.Ok:
                    return Core.Properties.Resources.Ok.ToUpper();
                case DialogButtonType.Cancel:
                    return Core.Properties.Resources.Cancel.ToUpper();
                case DialogButtonType.No:
                    return Core.Properties.Resources.No.ToUpper();
                case DialogButtonType.Yes:
                    return Core.Properties.Resources.Yes.ToUpper();
            }
            return null;
        }

        private void DialogWindow_OnClosed(object sender, EventArgs e)
        {
            _dialog.OnButtonSelected(_resultType);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window 
            DragMove();
        }
    }
}
