using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Base.Models
{
    public enum LogEntryType
    {
        Unknown = 0, 

        Account = 1, 

        Calendar = 2, 
        CurrentCalendarSet = 3, 

        SyncQueues = 4

        // TODO: 
        // Other log types to be introduced. 
        // ... 
    }
}
