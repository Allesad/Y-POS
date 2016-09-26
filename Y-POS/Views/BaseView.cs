using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using YumaPos.Client.UI.ViewModels.Contracts;

namespace Y_POS.Views
{
    public abstract class BaseView : UserControl
    {
        #region Fields

        /// <summary>
        /// Use this observable in observable subscriptions in derived views.
        /// Example of usage:
        /// <code>
        /// Observable.FromEventPattern("...args...")
        ///     .TakeUntil(closingObservable)
        ///     .Subscribe();
        /// </code>
        /// </summary>
        protected readonly IObservable<EventPattern<RoutedEventArgs>> closingObservable;

        #endregion

        protected ILifecycleVm TypedDataContext => DataContext as ILifecycleVm;

        protected BaseView()
        {
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            DataContextChanged += OnDataContextChanged;

            closingObservable = Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                handler => (s, a) => handler(s, a),
                handler => Unloaded += handler,
                handler => Unloaded -= handler);
        }

        protected virtual void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var oldViewModel = dependencyPropertyChangedEventArgs.OldValue as ILifecycleVm;
            var newViewModel = dependencyPropertyChangedEventArgs.NewValue as ILifecycleVm;

            if (oldViewModel == null || newViewModel == null)
                return;

            oldViewModel.CommandViewUnloaded?.Execute(null);
            newViewModel.CommandViewLoaded?.Execute(null);
        }

        protected virtual void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var viewModel = TypedDataContext;
            viewModel?.CommandViewUnloaded?.Execute(null);
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var viewModel = TypedDataContext;
            viewModel?.CommandViewLoaded?.Execute(null);
        }
    }
}
