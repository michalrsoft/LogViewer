using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Base.Models
{
    public class LogItem
    {
        #region Private fields

        private LogEntry _entry;
            
        private LogEntryType _entryType;

        #endregion

        #region Public properties

        public LogEntry Entry => _entry;

        public Nullable<DateTime> Timestamp => _entry.Timestamp;

        public string CodeDebugInfo => _entry.CodeDebugInfo;

        public LogEntryType EntryType => _entryType;

        public virtual IList<string> Lines => _entry.Lines;

        public virtual string Contents => _entry.Contents;

        #endregion

        public LogItem(LogEntry entry, LogEntryType lineEntryType)
        {
            ArgumentNullException.ThrowIfNull(entry);

            _entry = entry;
            _entryType = lineEntryType;
        }
    }
}
