using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Base
{
    public class LogEntry
    {
        #region Private fields

        private LogEntryHeader _logEntryHeader;

        private IList<string> _lines;

        private Lazy<string> _contents;

        #endregion

        #region Public properties

        public IList<string> Lines => _lines;

        public DateTime? Timestamp => _logEntryHeader.Timestamp;

        public string CodeDebugInfo => _logEntryHeader.CodeDebugInfo;

        public string Contents => _contents.Value;

        #endregion

        public LogEntry(LogEntryHeader logEntryHeader, IList<string> lines)
        {
            ArgumentNullException.ThrowIfNull(logEntryHeader);
            ArgumentNullException.ThrowIfNull(lines);

            _logEntryHeader = logEntryHeader;
            _lines = lines;

            _contents = new Lazy<string>(() => string.Join(Environment.NewLine, _lines)); 
        }
    }
}
