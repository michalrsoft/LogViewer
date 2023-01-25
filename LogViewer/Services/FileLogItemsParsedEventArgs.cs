using System;
using System.Collections.Generic;

namespace LogViewer.Base.Models
{
    /// <summary>
    /// Event args to pass information about log items (<see cref="FileLogItemsParsedEventArgs.Items"/>) found in the specific file 
    /// (<see cref="FileLogItemsParsedEventArgs.FilePath"/>). 
    /// </summary>
    public class FileLogItemsParsedEventArgs : EventArgs
    {
        private string _filePath;

        private IList<LogItem> _items;

        public IList<LogItem> Items => _items;

        public string FilePath => _filePath;

        public FileLogItemsParsedEventArgs(string filePath, IList<LogItem> items)
            : base()
        {
            ArgumentNullException.ThrowIfNull(filePath);
            ArgumentNullException.ThrowIfNull(items);

            _filePath = filePath;
            _items = items;
        }
    }
}
