using LogViewer.Base.Models;

namespace LogViewer.Base.Parsers
{
    /// <summary>
    /// Abstract class that is supposed to deal with log entries that contain comma-delimeted list of properties, ex. Accounts, 
    /// Calendars, Calendar Sets. It can parse multiple log entries of the same type till it finds one it cannot handle. 
    /// </summary>
    public abstract class LogEntryCommaDelimetedParserBase : LogEntryParserBase
    {
        private static readonly string[] LineSplitSeparators = new string[] { ", " };

        #region ILogEntryParser members

        /// <summary>
        /// Abstract method to be implemented in classes that build specific instances of <see cref="LogItem"/> based on 
        /// provided <paramref name="logEntry"/> and its properties (<paramref name="logEntryParts"/>). 
        /// </summary>
        protected abstract LogItem GenerateLogEntry(LogEntry logEntry, List<string> logEntryParts);

        /// <summary>
        /// Method attempts to parse instances of <see cref="LogEntry"/> with a provided entry line prefix / header 
        /// (represented by <paramref name="logEntryHeader"/>). 
        /// </summary>
        /// 
        /// <param name="logEntryHeader">Expected prefix / header.</param>
        /// <param name="queueOfLogLines">Queue of log entries to parse.</param>
        /// <param name="logEntriesOutput">Output of the method results.</param>
        /// 
        /// <returns><c>True</c> if at least one log entry was parsed. Otherwise <c>False</c>.</returns>
        protected virtual bool TryParseWithEntryHeader(
            string logEntryHeader, 
            Queue<LogEntry> queueOfLogLines, 
            out IList<LogItem> logEntriesOutput)
        {
            ArgumentNullException.ThrowIfNull(logEntryHeader);
            ArgumentNullException.ThrowIfNull(queueOfLogLines);

            List<LogItem> logItemsParsed = new List<LogItem>();

            LogEntry initialLine = queueOfLogLines.Peek();

            if (initialLine.Timestamp.HasValue &&
                !string.IsNullOrWhiteSpace(initialLine.CodeDebugInfo) &&
                !string.IsNullOrWhiteSpace(initialLine.Contents))
            {
                if (initialLine.Contents.StartsWith(logEntryHeader, StringComparison.OrdinalIgnoreCase))
                {
                    if (initialLine.Contents.Length > logEntryHeader.Length)
                    {
                        List<string> logEntryParts = SplitCommaDelimeted(initialLine.Contents.Substring(logEntryHeader.Length));
                        LogItem initialLogItem = GenerateLogEntry(initialLine, logEntryParts);
                        if (initialLogItem != null)
                        {
                            logItemsParsed.Add(initialLogItem);
                            this.OnLogItemsParsed(new List<LogItem>() { initialLogItem });
                        }
                    }

                    queueOfLogLines.Dequeue();

                    while (queueOfLogLines.Any())
                    {
                        LogEntry nextLine = queueOfLogLines.Peek();
                        if (nextLine.Timestamp.HasValue &&
                            !string.IsNullOrWhiteSpace(nextLine.CodeDebugInfo) &&
                            !string.IsNullOrWhiteSpace(nextLine.Contents))
                        {
                            if (!nextLine.Contents.StartsWith('\t'))
                            {
                                break;
                            }

                            LogItem nextLogItem = GenerateLogEntry(nextLine, SplitCommaDelimeted(nextLine.Contents));
                            if (nextLogItem == null)
                            {
                                break;
                            }

                            logItemsParsed.Add(nextLogItem);
                            this.OnLogItemsParsed(new List<LogItem>() { nextLogItem });

                            queueOfLogLines.Dequeue();
                        }
                    }

                    logEntriesOutput = logItemsParsed;
                    return true;
                }
            }

            logEntriesOutput = new List<LogItem>();
            return false;
        }

        protected virtual List<string> SplitCommaDelimeted(string line)
        {
            ArgumentNullException.ThrowIfNull(line);

            if (!string.IsNullOrWhiteSpace(line))
            {
                string logLineTrimmed = line.Trim();
                if (!string.IsNullOrWhiteSpace(logLineTrimmed))
                {
                    return logLineTrimmed.Split(LineSplitSeparators, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
            }

            return new List<string>();
        }

        #endregion

        public LogEntryCommaDelimetedParserBase()
        {

        }
    }
}
