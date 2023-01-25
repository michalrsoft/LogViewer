using LogViewer.Base.Models;

namespace LogViewer.Base.Parsers
{
    /// <summary>
    /// Base abstract class for log parsers that implements the <see cref="ILogEntryParser.LogItemsParsed"/> event and provides 
    /// ways for invoking it.
    /// </summary>
    public abstract class LogEntryParserBase : ILogEntryParser
    {
        #region ILogEntryParser members

        public event EventHandler<LogItemsParsedEventArgs> LogItemsParsed = null;

        public abstract bool TryParse(Queue<LogEntry> logLines, out IList<LogItem> logEntriesOutput);

        #endregion

        protected virtual void OnLogItemsParsed(IList<LogItem> entries)
        {
            ArgumentNullException.ThrowIfNull(entries);

            if (entries.Any())
            {
                LogItemsParsed?.Invoke(this, new LogItemsParsedEventArgs(entries));
            }
        }

        public LogEntryParserBase()
        {
        }
    }
}
