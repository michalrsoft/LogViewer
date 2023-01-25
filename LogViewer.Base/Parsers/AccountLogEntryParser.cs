using LogViewer.Base.Models;

namespace LogViewer.Base.Parsers
{
    /// <summary>
    /// Parser that produces instances of <see cref="AccountLogItem"/> from provided properties. 
    /// </summary>
    public class AccountLogEntryParser : LogEntryCommaDelimetedParserBase
    {
        #region ILogEntryParser members

        protected override LogItem GenerateLogEntry(LogEntry entry, List<string> entryProperties)
        {
            ArgumentNullException.ThrowIfNull(entry);
            ArgumentNullException.ThrowIfNull(entryProperties);

            if (!entryProperties.Any())
            {
                return null;
            }

            return new AccountLogItem(entry, entryProperties);
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