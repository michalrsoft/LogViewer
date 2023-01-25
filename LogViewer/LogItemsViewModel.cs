using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Accessibility;
using LogViewer.Base;
using LogViewer.Base.Models;
using LogViewer.Base.Parsers;
using Microsoft.Toolkit.Mvvm.Input;

namespace LogViewer
{
    public class LogItemsViewModel
    {
        RelayCommand _copyLogEntryCommand;

        public ICommand CopyLogEntryCommand
        {
            get
            {
                if (_copyLogEntryCommand == null)
                {
                    _copyLogEntryCommand = new RelayCommand(() => System.Windows.MessageBox.Show("Run"));
                }
                return _copyLogEntryCommand;
            }
        }

        public ObservableCollection<AccountLogItem> Accounts { get; private set; }

        public ObservableCollection<CalendarLogItem> Calendars { get; private set; }

        public ObservableCollection<CurrentCalendarSetLogItem> CalendarSets { get; private set; }

        public ObservableCollection<SyncQueuesLogItem> SyncQueues { get; private set; }

        public LogItemsViewModel(IList<LogItem> logItems)
        {
            Accounts = new ObservableCollection<AccountLogItem>(logItems.OfType<AccountLogItem>());
            Calendars = new ObservableCollection<CalendarLogItem>(logItems.OfType<CalendarLogItem>());
            CalendarSets = new ObservableCollection<CurrentCalendarSetLogItem>(logItems.OfType<CurrentCalendarSetLogItem>());
            SyncQueues = new ObservableCollection<SyncQueuesLogItem>(logItems.OfType<SyncQueuesLogItem>());
        }
    }
}
