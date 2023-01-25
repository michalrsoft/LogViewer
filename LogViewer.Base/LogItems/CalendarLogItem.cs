using System.Text.RegularExpressions;

namespace LogViewer.Base.Models
{
    /// <summary>
    /// Class represents Calendar log item with the default properties. 
    /// </summary>
    /// 
    /// <remarks>Using Regex to parse the string like &quot;1/0 (count: 804)&quot; 
    /// which describes Calendar support and items count.</remarks>
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

        private Nullable<int> _numberOfItems = null;

        #endregion

        #region Properties

        public string CalendarName => _calendarName;

        public string AccountIdentifier => _accountIdentifier;

        public string CalendarIdentifier => _calendarIdentifier;

        public bool SupportsStoringEvents => _supportsStoringEvents;

        public bool SupportsStoringTasks => _supportsStoringTasks;

        public Nullable<int> NumberOfItems => _numberOfItems;

        #endregion

        public CalendarLogItem(LogEntry logEntry, IList<string> entryContentItems)
            : base(logEntry, LogEntryType.Calendar, entryContentItems)
        {
            // Retrieving the properties in an order that was provided to me 
            // from Kent as a way to interpret a single Calendar log entry. 

            _calendarName = EntryProperties.FirstOrDefault();

            _accountIdentifier = EntryProperties.Skip(1).FirstOrDefault();
            _calendarIdentifier = EntryProperties.Skip(2).FirstOrDefault();

            string calendarSupportAndCount = EntryProperties.Skip(3).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(calendarSupportAndCount))
            {
                Match logEntryPrefixMatch = _parseSupportAndCountRegex.Match(calendarSupportAndCount);
                if ((logEntryPrefixMatch.Success) && (logEntryPrefixMatch.Groups.Count == 4))
                {
                    _supportsStoringEvents = string.Equals(logEntryPrefixMatch.Groups[1].Value, "1");
                    _supportsStoringTasks = string.Equals(logEntryPrefixMatch.Groups[2].Value, "1");

                    if (Int32.TryParse(logEntryPrefixMatch.Groups[3].Value, out var count))
                    {
                        _numberOfItems = count;
                    }
                    else
                    {
                        _numberOfItems = null;
                    }
                }
            }
        }
    }
}
