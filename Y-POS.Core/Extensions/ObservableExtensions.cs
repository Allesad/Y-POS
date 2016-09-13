using System;
using System.Reactive.Linq;
using YumaPos.Client.Common;
using YumaPos.Client.Helpers;

namespace Y_POS.Core.Extensions
{
    public static class ObservableExtensions
    {
        public static IDisposable SubscribeToObserveOnUi<T>(this IObservable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.ObserveOn(ServiceLocator.Resolve<ISchedulerService>().UiScheduler).Subscribe();
        }

        public static IDisposable SubscribeToObserveOnUi<T>(this IObservable<T> source, Action<T> onNext)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (onNext == null) throw new ArgumentNullException(nameof(onNext));

            return source.ObserveOn(ServiceLocator.Resolve<ISchedulerService>().UiScheduler).Subscribe(onNext);
        }

        public static IDisposable SubscribeToObserveOnUi<T>(this IObservable<T> source, Action<T> onNext,
            Action<Exception> onError)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (onNext == null) throw new ArgumentNullException(nameof(onNext));
            if (onError == null) throw new ArgumentNullException(nameof(onError));

            return source.ObserveOn(ServiceLocator.Resolve<ISchedulerService>().UiScheduler).Subscribe(onNext, onError);
        }

        public static IDisposable SubscribeToObserveOnUi<T>(this IObservable<T> source, Action<T> onNext,
            Action<Exception> onError, Action onCompleted)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (onNext == null) throw new ArgumentNullException(nameof(onNext));
            if (onError == null) throw new ArgumentNullException(nameof(onError));
            if (onCompleted == null) throw new ArgumentNullException(nameof(onCompleted));

            return source.ObserveOn(ServiceLocator.Resolve<ISchedulerService>().UiScheduler)
                .Subscribe(onNext, onError, onCompleted);
        }
    }
}
