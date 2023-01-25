using LogViewer.Base.Models;
using LogViewer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Threading;
using System.Threading;
using System.Windows;

namespace LogViewer.ViewModels
{
    public class LogsViewModel
    {
        private ObservableCollection<AccountLogItem> _accounts;

        private ObservableCollection<CalendarLogItem> _calendars;

        private ObservableCollection<CurrentCalendarSetLogItem> _calendarSets;

        private ObservableCollection<SyncQueuesLogItem> _syncQueues;

        private ILogItemsService _logItemsService;

        private string _filePath;

        public ObservableCollection<AccountLogItem> Accounts => _accounts;

        public ObservableCollection<CalendarLogItem> Calendars => _calendars;

        public ObservableCollection<CurrentCalendarSetLogItem> CalendarSets => _calendarSets;

        public ObservableCollection<SyncQueuesLogItem> SyncQueues => _syncQueues;

        public void StartLogProcessing()
        {
            IDisposable accountsSubscription = CreateSubscription(_accounts);
            IDisposable calendarsSubscription = CreateSubscription(_calendars);
            IDisposable calendarSetsSubscription = CreateSubscription(_calendarSets);
            IDisposable syncQueuesSubscription = CreateSubscription(_syncQueues);

            Task.Run(
                () =>
                {
                    try
                    {
                        Task<IList<LogItem>> getLogItems = _logItemsService.GetLogItemsAsync(_filePath);
                        getLogItems.Wait();
                    }
                    catch (Exception ex)
                    {

                    }
                });
        }

        protected virtual IDisposable CreateSubscription<TLogItem>(ObservableCollection<TLogItem> observableCollection) 
            where TLogItem : LogItem
        {
            IObservable<EventPattern<FileLogItemsParsedEventArgs>> observableFromEvent =
                Observable
                    .FromEventPattern<EventHandler<FileLogItemsParsedEventArgs>, FileLogItemsParsedEventArgs>(
                        h => _logItemsService.FileLogItemsParsed += h,
                        h => _logItemsService.FileLogItemsParsed -= h);

            return
                observableFromEvent
                    .Where(logevents => string.Equals(logevents.EventArgs.FilePath, _filePath, StringComparison.OrdinalIgnoreCase))
                    .Select(logevents => logevents.EventArgs.Items.OfType<TLogItem>())
                    .ObserveOn(SynchronizationContext.Current)
                    .Subscribe(
                        change =>
                        {
                            foreach (TLogItem logItem in change)
                            {
                                observableCollection.Add(logItem);
                            }
                        });
        }

        public LogsViewModel(ILogItemsService logItemsService, string filePath)
        {
            ArgumentNullException.ThrowIfNull(logItemsService);
            ArgumentNullException.ThrowIfNull(filePath);

            _logItemsService = logItemsService;
            _filePath = filePath;

            _accounts = new ObservableCollection<AccountLogItem>();
            _calendars = new ObservableCollection<CalendarLogItem>();
            _calendarSets = new ObservableCollection<CurrentCalendarSetLogItem>();
            _syncQueues = new ObservableCollection<SyncQueuesLogItem>();
        }
    }
}
