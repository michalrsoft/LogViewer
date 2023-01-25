using LogViewer.Base.Models;

namespace LogViewer.Base.Parsers
{
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
