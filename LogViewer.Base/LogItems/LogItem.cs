namespace LogViewer.Base.Models
{
    /// <summary>
    /// Base class for all log items. 
    /// </summary>
    public class LogItem
    {
        #region Private fields

        private LogEntry _entry;
            
        private LogEntryType _entryType;

        #endregion

        #region Public properties

        public LogEntry Entry => _entry;

        public Nullable<DateTime> Timestamp => _entry.Timestamp;

        public string CodeDebugInfo => _entry.CodeDebugInfo;

        public LogEntryType EntryType => _entryType;

        public virtual IList<string> Lines => _entry.Lines;

        public virtual string Contents => _entry.Contents;

        #endregion

        /// <summary>
        /// Constructor creates a new instance of <see cref="LogItem"/> with log entry and type.
        /// </summary>
        /// 
        /// <param name="entry">Entry from the log file.</param>
        /// <param name="lineEntryType">Type of log entry.</param>
        /// 
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="entry"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="lineEntryType"/> equals <see cref="LogEntryType.Unknown"/>.</exception>
        public LogItem(LogEntry entry, LogEntryType lineEntryType)
        {
            ArgumentNullException.ThrowIfNull(entry);

            if (lineEntryType == LogEntryType.Unknown)
            {
                throw new ArgumentException("The type of log entry is unknown.", nameof(lineEntryType));
            }

            _entry = entry;
            _entryType = lineEntryType;
        }
    }
}
