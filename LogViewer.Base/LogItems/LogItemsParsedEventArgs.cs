namespace LogViewer.Base.Models
{
    /// <summary>
    /// Event args used to pass information about new log items parsed. 
    /// </summary>
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
