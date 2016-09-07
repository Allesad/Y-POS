using System.Windows;
using System.Windows.Controls;

namespace Y_POS.Controls
{
    /// <summary>
    /// Content presenter with ability to notify about content changes
    /// </summary>
    internal class ContentPresenterEx : ContentPresenter
    {
        #region Events

        public event DependencyPropertyChangedEventHandler ContentChanged;

        #endregion

        #region Static Constructor

        static ContentPresenterEx()
        {
            ContentProperty.OverrideMetadata(typeof(ContentPresenterEx),
                new FrameworkPropertyMetadata(OnContentChanged));
        }

        private static void OnContentChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var cp = (ContentPresenterEx) o;
            
            var args = new DependencyPropertyChangedEventArgs(ContentProperty, e.OldValue, e.NewValue);
            cp.ContentChanged(cp, args);
        }

        #endregion
    }
}
