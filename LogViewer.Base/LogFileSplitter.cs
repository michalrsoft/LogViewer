using System.Globalization;
using System.Text.RegularExpressions;

namespace LogViewer.Base
{
    /// <summary>
    /// Class meant for splitting lines that come from a log file into instances of <see cref="LogEntry"/>. Each log entry should 
    /// consider a prefix / header at the beginning of the line with a Timestamp and Code Debug Info. 
    /// </summary>
    public class LogFileSplitter
    {
        /// <summary>
        /// Pattern for matching the prefix / header at the beginning of the line. Must be matched to consider a log entry valid. 
        /// </summary>
        public const string LogEntryLinePrefixPattern = @"([0-9]+\/[0-9]+\/[0-9]+)\s+([0-9]+:[0-9]+:[0-9]+:[0-9]+)\s+\[([^\]]+)\]\s";

        private static readonly Regex _logEntryLinePrefixRegex = new Regex(LogFileSplitter.LogEntryLinePrefixPattern);

        public const string DateTimeFormat = "yyyy/MM/dd hh:mm:ss:fff";

        /// <summary>
        /// Method parses provided collection of lines into log entries. 
        /// </summary>
        /// 
        /// <param name="allLogLines">Input lines.</param>
        /// 
        /// <returns>A list of log entries.</returns>
        /// 
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="allLogLines"/> is null.</exception>
        public IList<LogEntry> SplitEntries(IEnumerable<string> allLogLines)
        {
            ArgumentNullException.ThrowIfNull(allLogLines);

            List<LogEntry> logEntries = new List<LogEntry>();

            LogEntryHeader currentLogEntryHeader = null;
            List<string> logEntryLines = null;

            bool insideLogEntry = false;

            foreach (string line in allLogLines)
            {
                Match logEntryPrefixMatch = _logEntryLinePrefixRegex.Match(line);
                if ((logEntryPrefixMatch.Success) && 
                    (logEntryPrefixMatch.Groups.Count == 4) &&
                    (DateTime.TryParseExact(
                        $"{logEntryPrefixMatch.Groups[1]} {logEntryPrefixMatch.Groups[2]}", 
                        DateTimeFormat, CultureInfo.InvariantCulture, 
                        DateTimeStyles.None, 
                        out var timestamp)))
                {
                    if (insideLogEntry)
                    {
                        logEntries.Add(new LogEntry(currentLogEntryHeader, logEntryLines));
                    }

                    string codeDebugInfo = logEntryPrefixMatch.Groups[3].Value;
                    currentLogEntryHeader = new LogEntryHeader(timestamp, codeDebugInfo);
                    string lineEnding = line.Substring(logEntryPrefixMatch.Groups[0].Length);
                    logEntryLines = new List<string>() { lineEnding };
                    insideLogEntry = true;
                }
                else
                {
                    if (insideLogEntry)
                    {
                        logEntryLines.Add(line);
                    }
                }
            }

            if (insideLogEntry)
            {
                logEntries.Add(new LogEntry(currentLogEntryHeader, logEntryLines));
            }

            return logEntries;
        }

        public LogFileSplitter() 
        { 
        }
    }
}
