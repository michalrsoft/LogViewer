using LogViewer.Base.Models;

namespace LogViewer.Base.Parsers
{
    public abstract class LogEntryCommaDelimetedParserBase : LogEntryParserBase
    {
        private static readonly string[] LineSplitSeparators = new string[] { ", " };

        #region ILogEntryParser members

        protected abstract LogItem GenerateLogEntry(LogEntry logLine, List<string> logEntryParts);

        protected virtual bool TryParseWithEntryHeader(
            string logEntryHeader, 
            Queue<LogEntry> queueOfLogLines, 
            out IList<LogItem> logEntriesOutput)
        {
            ArgumentNullException.ThrowIfNull(logEntryHeader);
            ArgumentNullException.ThrowIfNull(queueOfLogLines);

            List<LogItem> logEntries = new List<LogItem>();

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
                        LogItem initialLogEntry = GenerateLogEntry(initialLine, logEntryParts);
                        if (initialLogEntry != null)
                        {
                            logEntries.Add(initialLogEntry);
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

                            LogItem nextLogEntry = GenerateLogEntry(nextLine, SplitCommaDelimeted(nextLine.Contents));
                            if (nextLogEntry == null)
                            {
                                break;
                            }

                            logEntries.Add(nextLogEntry);
                            queueOfLogLines.Dequeue();
                        }
                    }

                    if (logEntries.Any())
                    {
                        this.OnLinesParsed(logEntries);
                    }

                    logEntriesOutput = logEntries;
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
