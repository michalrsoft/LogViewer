using LogViewer.Base.Models;

namespace LogViewer.Base.Parsers
{
    public interface ILogEntryParser
    {
        event EventHandler<LogItemsParsedEventArgs> LinesParsed;

        bool TryParse(Queue<LogEntry> logLines, out IList<LogItem> logEntriesOutput);
    }
}
