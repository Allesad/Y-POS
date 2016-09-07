using System.Reactive.Concurrency;
using ReactiveUI;
using YumaPos.Client.Common;

namespace Y_POS
{
    internal class WpfSchedulerService : ISchedulerService
    {
        public IScheduler UiScheduler
        {
            get { return RxApp.MainThreadScheduler; }
        }

        public IScheduler TaskpoolScheduler
        {
            get { return RxApp.TaskpoolScheduler; }
        }

        public IScheduler NewThreadScheduler { get { return System.Reactive.Concurrency.NewThreadScheduler.Default; } }
    }
}
