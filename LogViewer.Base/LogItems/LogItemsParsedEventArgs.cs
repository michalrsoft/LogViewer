﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Base.Models
{
    public class LogItemsParsedEventArgs : EventArgs
    {
        private IList<LogItem> _items;

        public IList<LogItem> Items => _items;

        public LogItemsParsedEventArgs(IList<LogItem> items)
            : base()
        {
            ArgumentNullException.ThrowIfNull(items);

            _items = items;
        }
    }
}
