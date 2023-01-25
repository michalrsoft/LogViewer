namespace LogViewer.Base.Models
{
    /// <summary>
    /// Class represents Current Calendar Set log item with the default properties. 
    /// </summary>
    public class CurrentCalendarSetLogItem : LogItemWithPropertiesBase
    {
        #region Private fields

        private Lazy<string> _calendarName;

        private Lazy<string> _calendarSetIdentifier;

        #endregion

        #region Properties

        public string CalendarName => _calendarName.Value;

        public string CalendarSetIdentifier => _calendarSetIdentifier.Value;

        #endregion

        public CurrentCalendarSetLogItem(LogEntry logLine, IList<string> entryItems)
            : base(logLine, LogEntryType.CurrentCalendarSet, entryItems)
        {
            // Retrieving the properties in an order that was provided to me 
            // from Kent as a way to interpret a single Calendar Set log entry. 

            _calendarName = new Lazy<string>(() => EntryProperties.FirstOrDefault());

            _calendarSetIdentifier = new Lazy<string>(() => EntryProperties.Skip(1).FirstOrDefault());
        }
    }
}
