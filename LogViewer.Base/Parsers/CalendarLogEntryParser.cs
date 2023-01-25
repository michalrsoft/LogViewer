using LogViewer.Base.Models;

namespace LogViewer.Base.Parsers
{
    public class CalendarLogEntryParser : LogEntryCommaDelimetedParserBase
    {
        #region ILogEntryParser members

        protected override LogItem GenerateLogEntry(LogEntry logEntry, List<string> entryParts)
        {
            ArgumentNullException.ThrowIfNull(logEntry);
            ArgumentNullException.ThrowIfNull(entryParts);

            if (!entryParts.Any())
            {
                return null;
            }

            return new CalendarLogItem(logEntry, entryParts);
        }

        public override bool TryParse(Queue<LogEntry> queueOfLogLines, out IList<LogItem> logEntriesOutput)
        {
            ArgumentNullException.ThrowIfNull(queueOfLogLines);

            return base.TryParseWithEntryHeader(LogItemTypeNames.Calendars, queueOfLogLines, out logEntriesOutput);
        }

        #endregion

        public CalendarLogEntryParser()
        {
        }
    }
}