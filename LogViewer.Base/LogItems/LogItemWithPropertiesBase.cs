namespace LogViewer.Base.Models
{
    /// <summary>
    /// Class represents a log item that corresponds to a single long entry in the log file. It is supposed to come 
    /// with a list of properties (these are line comma-delimeted properties parsed). 
    /// </summary>
    public class LogItemWithPropertiesBase : LogItem
    {
        protected IList<string> _entryProperties;

        public virtual IList<string> EntryProperties => _entryProperties;

        public LogItemWithPropertiesBase(LogEntry entry, LogEntryType entryType, IList<string> entryProperties) 
            : base(entry, entryType) 
        {
            ArgumentNullException.ThrowIfNull(entryProperties);

            _entryProperties = entryProperties;
        }
    }
}
