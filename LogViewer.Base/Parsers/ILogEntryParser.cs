using LogViewer.Base.Models;

namespace LogViewer.Base.Parsers
{
    public interface ILogEntryParser
    {
        event EventHandler<LogItemsParsedEventArgs> LogItemsParsed;

        bool TryParse(Queue<LogEntry> logLines, out IList<LogItem> logEntriesOutput);
    }
}
