using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Base.Models
{
    public class LogItemsParsedEventArgs : EventArgs
    {
        private IList<LogItem> _entries;

        public IList<LogItem> Entries => _entries;

        public LogItemsParsedEventArgs(IList<LogItem> entries)
            : base()
        {
            ArgumentNullException.ThrowIfNull(entries);

            _entries = entries;
        }
    }
}
