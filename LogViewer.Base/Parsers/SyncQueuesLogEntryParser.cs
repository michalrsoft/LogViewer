using LogViewer.Base.Models;
using System.Text;

namespace LogViewer.Base.Parsers
{
    public class SyncQueuesLogEntryParser : LogEntryParserBase
    {
        public const string EndOfSyncQueueLineDelimeter = ")>";

        #region ILogEntryParser members

        // TODO: 
        // Write a comment here on why I have not used a Regex in this scenario. 

        protected virtual IList<string> ExtractSyncQueueStrings(IList<string> linesToParse)
        {
            ArgumentNullException.ThrowIfNull(linesToParse);

            List<string> syncQueueStrings = new List<string>();

            StringBuilder syncQueryBuilder =
                new StringBuilder
                    ((linesToParse[0].Length > LogItemTypeNames.SyncQueues.Length) ?
                    linesToParse[0].Substring(LogItemTypeNames.SyncQueues.Length).Trim() :
                    string.Empty);

            foreach (string line in linesToParse.Skip(1))
            {
                syncQueryBuilder.AppendLine(line);
                if (line.StartsWith(SyncQueuesLogEntryParser.EndOfSyncQueueLineDelimeter))
                {
                    syncQueueStrings.Add(syncQueryBuilder.ToString().Trim());
                    syncQueryBuilder = new StringBuilder(string.Empty);
                }
            }

            string lastSyncQueueString = syncQueryBuilder.ToString();
            if (!string.IsNullOrWhiteSpace(lastSyncQueueString))
            {
                syncQueueStrings.Add(syncQueryBuilder.ToString());
            }

            return syncQueueStrings;
        }

        protected virtual LogItem GenerateLogEntry(LogEntry logLine, string syncQueue)
        {
            ArgumentNullException.ThrowIfNull(logLine);
            ArgumentNullException.ThrowIfNull(syncQueue);

            return new SyncQueuesLogItem(logLine, syncQueue);
        }

        public override bool TryParse(Queue<LogEntry> queueOfLogLines, out IList<LogItem> logEntriesOutput)
        {
            ArgumentNullException.ThrowIfNull(queueOfLogLines);

            LogEntry logEntry = queueOfLogLines.Peek();
            if (logEntry.Timestamp.HasValue &&
                !string.IsNullOrWhiteSpace(logEntry.CodeDebugInfo) &&
                logEntry.Lines.Any())
            {
                if (logEntry.Lines[0].StartsWith(LogItemTypeNames.SyncQueues, StringComparison.OrdinalIgnoreCase))
                {
                    queueOfLogLines.Dequeue();

                    IList<string> syncQueueStrings = ExtractSyncQueueStrings(logEntry.Lines);
                    if (syncQueueStrings.Any())
                    {
                        List<LogItem> logItems = syncQueueStrings.Select(s => GenerateLogEntry(logEntry, s)).ToList();
                        if (logItems.Any())
                        {
                            this.OnLogItemsParsed(logItems);

                            logEntriesOutput = logItems;
                            return true;
                        }
                    }
                }
            }

            logEntriesOutput = new List<LogItem>();
            return false;
        }

        #endregion

        public SyncQueuesLogEntryParser()
        {
        }
    }
}