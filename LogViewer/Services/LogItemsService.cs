using LogViewer.Base;
using LogViewer.Base.Models;
using LogViewer.Base.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LogViewer.Services
{
    /// <summary>
    /// Service that parses log items from provided file and notifies with event (<see cref="ILogItemsService.FileLogItemsParsed"/>) 
    /// as it goes forward with parsing. 
    /// </summary>
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

        /// <summary>
        /// Method parses the provided <paramref name="filePath"/> and grabs a list of <see cref="LogItem"/> from it. 
        /// </summary>
        /// 
        /// <remarks>
        /// <para>
        /// The parsing mechanism uses the Chain of Responsibility pattern with the handlers (<see cref="ILogEntryParser"/>) 
        /// registered and checked one by one for each individual <see cref="LogEntry"/> till I find one that can handle the log entry. 
        /// If no suitable handler is found, a log entry is ommitted. 
        /// </para>
        /// 
        /// <para>
        /// The method invokes <see cref="ILogItemsService.FileLogItemsParsed"/> event for log items as soon as they are found. 
        /// </para>
        /// </remarks>
        /// 
        /// <param name="filePath">File to be parsed.</param>
        /// 
        /// <returns>List of parsed <see cref="LogItem"/> objects.</returns>
        /// 
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filePath"/> is null.</exception>
        /// <exception cref="FileNotFoundException">Thrown if the <paramref name="filePath"/> cannot be found or accessed.</exception>
        public async Task<IList<LogItem>> GetLogItemsAsync(string filePath)
        {
            try
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
                        new SyncQueuesLogEntryParser()
                    };

                EventHandler<LogItemsParsedEventArgs> handler = (s, e) => OnFileLogItemsParsed(filePath, e.Items);

                foreach (ILogEntryParser parser in logEntryParsers)
                {
                    parser.LogItemsParsed += handler;
                }

                try
                {
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
                finally
                {
                    foreach (ILogEntryParser parser in logEntryParsers)
                    {
                        parser.LogItemsParsed -= handler;
                    }
                }
            }
            finally
            {
                // Some cleanup code can come here. 
            }
        }
    }
}
