using LogViewer.Base.Models;
using LogViewer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

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

        public void ProcessFileLogs()
        {
            Task.Run(
                () =>
                {
                    try
                    {
                        _logItemsService.FileLogItemsParsed += LogItemsService_LogItemsParsed;
                        Task<IList<LogItem>> getLogItems = _logItemsService.GetLogItemsAsync(_filePath);
                        getLogItems.Wait();
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        _logItemsService.FileLogItemsParsed -= LogItemsService_LogItemsParsed;
                    }
                });
        }

        private void LogItemsService_LogItemsParsed(object sender, FileLogItemsParsedEventArgs e)
        {
            try
            {
                System.Windows.Application.Current.Dispatcher.BeginInvoke(
                    new Action(() =>
                    {
                        if (string.Equals(e.FilePath, _filePath, StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (LogItem item in e.Items)
                            {
                                if (item is AccountLogItem)
                                {
                                    Accounts.Add(item as AccountLogItem);
                                }
                                else if (item is CalendarLogItem)
                                {
                                    Calendars.Add(item as CalendarLogItem);
                                }
                                else if (item is CurrentCalendarSetLogItem)
                                {
                                    CalendarSets.Add(item as CurrentCalendarSetLogItem);
                                }
                                else if (item is SyncQueuesLogItem)
                                {
                                    SyncQueues.Add(item as SyncQueuesLogItem);
                                }
                            }
                        }
                    }));
            }
            catch (Exception ex)
            {

            }
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
