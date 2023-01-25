using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Base.Models
{
    public class CurrentCalendarSetLogItem : LogItemWithPropertiesBase
    {
        #region Private fields

        private Lazy<string> _calendarName;

        private Lazy<string> _calendarSetIdentifier;

        #endregion

        public string CalendarName => _calendarName.Value;

        public string CalendarSetIdentifier => _calendarSetIdentifier.Value;

        public CurrentCalendarSetLogItem(LogEntry logLine, IList<string> entryItems)
            : base(logLine, LogEntryType.CurrentCalendarSet, entryItems)
        {
            _calendarName = new Lazy<string>(() => EntryItems.FirstOrDefault());

            _calendarSetIdentifier = new Lazy<string>(() => EntryItems.Skip(1).FirstOrDefault());
        }
    }
}
