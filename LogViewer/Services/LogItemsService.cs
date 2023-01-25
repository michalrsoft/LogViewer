using LogViewer.Base;
using LogViewer.Base.Models;
using LogViewer.Base.Parsers;
using LogViewer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace LogViewer.Services
{
    public class LogItemsService : ILogItemsService
    {
        public event EventHandler<FileLogItemsParsedEventArgs> FileLogItemsParsed = null;

        protected virtual void OnFileLogItemsParsed(string filePath, IList<LogItem> entries)
        {
            ArgumentNullException.ThrowIfNull(filePath);
            ArgumentNullException.ThrowIfNull(entries);

            if (entries.Any())
            {
                FileLogItemsParsed?.Invoke(this, new FileLogItemsParsedEventArgs(filePath, entries));
            }
        }

        public async Task<IList<LogItem>> GetLogItemsAsync(string filePath)
        {
            ArgumentNullException.ThrowIfNull(filePath);

            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path was not provided.", nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File was not found or not accessible at {filePath}.");
            }

            string[] allLines = await File.ReadAllLinesAsync(filePath);

            Queue<LogEntry> logEntries = new Queue<LogEntry>(new LogFileSplitter().SplitEntries(allLines));

            List<ILogEntryParser> logEntryParsers =
                new List<ILogEntryParser>()
                {
                    new AccountLogEntryParser(),
                    new CalendarLogEntryParser(),
                    new CurrentCalendarSetLogEntryParser(),
                    new SyncQueuesLogEntryParser(),
                };

            foreach (ILogEntryParser parser in logEntryParsers)
            {
                parser.LogItemsParsed += (sender, ea) => OnFileLogItemsParsed(filePath, ea.Items);
            }

            List<LogItem> allLogItems = new List<LogItem>();
            while (logEntries.Any())
            {
                bool lineParsed = false;
                foreach (ILogEntryParser parser in logEntryParsers)
                {
                    if (parser.TryParse(logEntries, out var newlyParsedEntries))
                    {
                        lineParsed = true;
                        allLogItems.AddRange(newlyParsedEntries);

                        break;
                    }
                }

                if (!lineParsed)
                {
                    logEntries.Dequeue();
                }
            }

            return allLogItems;
        }
    }
}
