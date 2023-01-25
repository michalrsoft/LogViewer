using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Base.Models
{
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
