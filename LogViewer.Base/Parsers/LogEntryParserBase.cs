using LogViewer.Base.Models;

namespace LogViewer.Base.Parsers
{
    public abstract class LogEntryParserBase : ILogEntryParser
    {
        #region ILogEntryParser members

        public event EventHandler<LogItemsParsedEventArgs> LinesParsed = null;

        public abstract bool TryParse(Queue<LogEntry> logLines, out IList<LogItem> logEntriesOutput);

        #endregion

        protected virtual void OnLinesParsed(IList<LogItem> entries)
        {
            ArgumentNullException.ThrowIfNull(entries);

            if (entries.Any())
            {
                LinesParsed?.Invoke(this, new LogItemsParsedEventArgs(entries));
            }
        }

        public LogEntryParserBase()
        {
        }
    }
}
