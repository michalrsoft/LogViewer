using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogViewer.Base.Models;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace LogViewer.Base
{
    public record LogEntryHeader(DateTime Timestamp, string CodeDebugInfo);

    public class LogFileSplitter
    {
        public const string LogEntryLinePrefixPattern = @"([0-9]+\/[0-9]+\/[0-9]+)\s+([0-9]+:[0-9]+:[0-9]+:[0-9]+)\s+\[([^\]]+)\]\s";

        private static readonly Regex _logEntryLinePrefixRegex = new Regex(LogFileSplitter.LogEntryLinePrefixPattern);

        public const string DateTimeFormat = "yyyy/MM/dd hh:mm:ss:fff";

        public IList<LogEntry> SplitEntries(IEnumerable<string> allLogLines)
        {
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
