namespace LogViewer.Base
{
    /// <summary>
    /// Record that represents the prefix or header of a log entry. 
    /// </summary>
    /// 
    /// <param name="Timestamp">Timestamp of the log entry.</param>
    /// <param name="CodeDebugInfo">Code debug info.</param>
    public record LogEntryHeader(DateTime Timestamp, string CodeDebugInfo);
}
