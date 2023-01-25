namespace LogViewer.Base.Models
{
    /// <summary>
    /// Enumeration represents log entry types. I have planned to use it in the main application but I skipped it for now.
    /// </summary>
    public enum LogEntryType
    {
        Unknown = 0, 

        Account = 1, 

        Calendar = 2, 
        CurrentCalendarSet = 3, 

        SyncQueues = 4

        // TODO: 
        // Other log types to be introduced. 
    }
}
