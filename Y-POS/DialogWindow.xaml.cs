using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

            Title = !string.IsNullOrWhiteSpace(dialog.Title) ? dialog.Title : string.Empty;

            var dlgType = dialog.GetType();
            
            if (dlgType == typeof(ConfirmationDialog))
            {
                Message.Text = ((TextDialogContent)dialog.Content).Message;
            }
            else if (dlgType == typeof(CustomContentDialog))
            {
                Message.Text = ((TextDialogContent)dialog.Content).Message;
            }

            foreach (var btn in dialog.Buttons.Select(CreateButtonFromConfig))
            {
                ActionsContainer.Children.Add(btn);
                ActionsContainer.Columns++;
            }
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
                    return "OK";
                case DialogButtonType.Cancel:
                    return "CANCEL";
                case DialogButtonType.No:
                    return "NO";
                case DialogButtonType.Yes:
                    return "YES";
            }
            return null;
        }

        private void DialogWindow_OnClosed(object sender, EventArgs e)
        {
            _dialog.OnButtonSelected(_resultType);
        }
    }
}
