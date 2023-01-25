using LogViewer.Base.Models;
using LogViewer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LogViewer.ViewModels
{
    /// <summary>
    /// ViewModel that grabs log items data from the provided service and can have it displayed. It contains logic to initiate 
    /// process of parsing the provided file to grab log items and receives the results in an event-driven manner by building 
    /// Reactive Extensions subscriptions for the data. This way I can grab the data as it comes and have a way to display it. 
    /// </summary>
    public class LogsViewModel
    {
        private ObservableCollection<AccountLogItem> _accounts;

        private ObservableCollection<CalendarLogItem> _calendars;

        private ObservableCollection<CurrentCalendarSetLogItem> _calendarSets;

        private ObservableCollection<SyncQueuesLogItem> _syncQueues;

        private ILogItemsService _logItemsService;

        private string _filePath;

        #region Public properties

        public ObservableCollection<AccountLogItem> Accounts => _accounts;

        public ObservableCollection<CalendarLogItem> Calendars => _calendars;

        public ObservableCollection<CurrentCalendarSetLogItem> CalendarSets => _calendarSets;

        public ObservableCollection<SyncQueuesLogItem> SyncQueues => _syncQueues;

        #endregion

        /// <summary>
        /// Method initiates Reactive Extensions subscriptions for different item types and then invokes the operation 
        /// that parses the file. 
        /// </summary>
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
                        // TODO: Need to handle errors here. 
                    }
                });
        }

        /// <summary>
        /// Method creates a Reactive Extensions subscription with logic that subscribes to a particular type of events 
        /// (represented by <typeparamref name="TLogItem"/>). The observer running with <see cref="SynchronizationContext.Current"/> 
        /// to update the collections nicely without explicit calls to <see cref="Dispatcher.BeginInvoke"/>.
        /// </summary>
        /// 
        /// <typeparam name="TLogItem">Type of log items to register for and accept.</typeparam>
        /// 
        /// <param name="observableCollection">Collection to be populated with items.</param>
        /// 
        /// <returns>Subscription to <see cref="IObservable{T}"/>.</returns>
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
