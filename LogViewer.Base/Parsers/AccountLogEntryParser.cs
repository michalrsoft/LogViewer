using LogViewer.Base.Models;

namespace LogViewer.Base.Parsers
{
    public class AccountLogEntryParser : LogEntryCommaDelimetedParserBase
    {
        #region ILogEntryParser members

        protected override LogItem GenerateLogEntry(LogEntry entry, List<string> entryParts)
        {
            ArgumentNullException.ThrowIfNull(entry);
            ArgumentNullException.ThrowIfNull(entryParts);

            if (!entryParts.Any())
            {
                return null;
            }

            return new AccountLogItem(entry, entryParts);
        }

        public override bool TryParse(Queue<LogEntry> queueOfLogLines, out IList<LogItem> logEntriesOutput)
        {
            ArgumentNullException.ThrowIfNull(queueOfLogLines);

            return base.TryParseWithEntryHeader(LogItemTypeNames.Accounts, queueOfLogLines, out logEntriesOutput);
        }

        #endregion

        public AccountLogEntryParser()
        {
        }
    }
}