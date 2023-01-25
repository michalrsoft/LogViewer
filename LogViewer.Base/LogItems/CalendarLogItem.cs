using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogViewer.Base.Models
{
    public class CalendarLogItem : LogItemWithPropertiesBase
    {
        public const string ParseSupportAndCountPattern = @"([0-1])\/([0-1])\s+\(count\:\s+([0-9]+)\)";

        private static readonly Regex _parseSupportAndCountRegex = new Regex(CalendarLogItem.ParseSupportAndCountPattern);

        #region Private fields

        private string _calendarName;

        private string _accountIdentifier;

        private string _calendarIdentifier;

        private bool _supportsStoringEvents = false;

        private bool _supportsStoringTasks = false;

        private int _numberOfItems = 0;

        #endregion

        public string CalendarName => _calendarName;

        public string AccountIdentifier => _accountIdentifier;

        public string CalendarIdentifier => _calendarIdentifier;

        public bool SupportsStoringEvents => _supportsStoringEvents;

        public bool SupportsStoringTasks => _supportsStoringTasks;

        public int NumberOfItems => _numberOfItems;

        public CalendarLogItem(LogEntry logEntry, IList<string> entryContentItems)
            : base(logEntry, LogEntryType.Calendar, entryContentItems)
        {
            _calendarName = EntryItems.FirstOrDefault();

            _accountIdentifier = EntryItems.Skip(1).FirstOrDefault();
            _calendarIdentifier = EntryItems.Skip(2).FirstOrDefault();

            string calendarSupportAndCount = EntryItems.Skip(3).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(calendarSupportAndCount))
            {
                Match logEntryPrefixMatch = _parseSupportAndCountRegex.Match(calendarSupportAndCount);
                if ((logEntryPrefixMatch.Success) && (logEntryPrefixMatch.Groups.Count == 4))
                {
                    _supportsStoringEvents = string.Equals(logEntryPrefixMatch.Groups[1].Value, "1");
                    _supportsStoringTasks = string.Equals(logEntryPrefixMatch.Groups[2].Value, "1");

                    Int32.TryParse(logEntryPrefixMatch.Groups[3].Value, out _numberOfItems);
                }
            }
        }
    }
}
