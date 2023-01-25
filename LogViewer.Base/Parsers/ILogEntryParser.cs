using LogViewer.Base.Models;

namespace LogViewer.Base.Parsers
{
    /// <summary>
    /// Interface to be implemented by classes that parse log entries. 
    /// </summary>
    public interface ILogEntryParser
    {
        /// <summary>
        /// Event that notifies observers about a number of new log items parsed. 
        /// Can be used for a kind of &quot;asynchronous&quot; notifications. 
        /// </summary>
        event EventHandler<LogItemsParsedEventArgs> LogItemsParsed;

        /// <summary>
        /// <para>Method tries to parse the next <see cref="LogEntry"/> from the queue of log entries 
        /// (<paramref name="logEntriesQueue"/>). If successful, it dequeues the top item from the queue and might attempt to 
        /// proceed parsing and dequeing subsequent log entries from the queue. 
        /// </para>
        /// 
        /// <para>
        /// Once a new <see cref="LogItem"/> has been parsed, the method triggers <see cref="ILogEntryParser.LogItemsParsed"/> 
        /// event to notify observers about it immediately. 
        /// </para>
        /// 
        /// <para>
        /// Method outputs parsed instances of <see cref="LogItem"/> into <paramref name="logEntriesOutput"/> list. 
        /// </para>
        /// </summary>
        /// 
        /// <param name="logEntriesQueue">Queue of log entries to parse.</param>
        /// <param name="logEntriesOutput">Output of the method.</param>
        /// 
        /// <returns><c>True</c> if the method has parsed and dequeued at least one (the top) item from the queue of 
        /// log entries. Otherwise <c>False</c>.</returns>
        bool TryParse(Queue<LogEntry> logEntriesQueue, out IList<LogItem> logEntriesOutput);
    }
}
