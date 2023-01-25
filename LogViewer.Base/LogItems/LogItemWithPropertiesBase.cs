using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Base.Models
{
    public class LogItemWithPropertiesBase : LogItem
    {
        protected IList<string> _entryItems;

        public virtual IList<string> EntryItems => _entryItems;

        public LogItemWithPropertiesBase(LogEntry entry, LogEntryType entryType, IList<string> entryItems) 
            : base(entry, entryType) 
        {
            ArgumentNullException.ThrowIfNull(entryItems);

            _entryItems = entryItems;
        }
    }
}
