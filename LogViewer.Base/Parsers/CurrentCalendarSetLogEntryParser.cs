using LogViewer.Base.Models;

namespace LogViewer.Base.Parsers
{
    /// <summary>
    /// Parser that produces instances of <see cref="CurrentCalendarSetLogItem"/> from provided properties. 
    /// </summary>
    public class CurrentCalendarSetLogEntryParser : LogEntryCommaDelimetedParserBase
    {
        #region ILogEntryParser members

        protected override LogItem GenerateLogEntry(LogEntry logEntry, List<string> entryItems)
        {
            ArgumentNullException.ThrowIfNull(logEntry);
            ArgumentNullException.ThrowIfNull(entryItems);

            if (!entryItems.Any())
            {
                return null;
            }

            return new CurrentCalendarSetLogItem(logEntry, entryItems);
        }

        public override bool TryParse(Queue<LogEntry> queueOfLogLines, out IList<LogItem> logEntriesOutput)
        {
            ArgumentNullException.ThrowIfNull(queueOfLogLines);

            return base.TryParseWithEntryHeader(LogItemTypeNames.CurrentCalendarSet, queueOfLogLines, out logEntriesOutput);
        }

        #endregion

        public CurrentCalendarSetLogEntryParser()
        {
        }
    }
}