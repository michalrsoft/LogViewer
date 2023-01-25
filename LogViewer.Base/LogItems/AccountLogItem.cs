namespace LogViewer.Base.Models
{
    /// <summary>
    /// Class represents Account log item with the default properties of an Account. 
    /// </summary>
    /// 
    /// <remarks>
    /// Lazy initialization is used for retrieving the actual properties of an Account. It was my initial idea to use Lazy 
    /// initialization in this scenario, which it has turned out to be an overdo I guess. This can be changed. 
    /// </remarks>
    public class AccountLogItem : LogItemWithPropertiesBase
    {
        #region Private fields

        private Lazy<string> _accountName;

        private Lazy<string> _identifier;

        private Lazy<Nullable<bool>> _isEnabled;

        private Lazy<string> _type;

        private Lazy<string> _userName;

        private Lazy<string> _serverURL;

        #endregion

        #region Properties

        public string AccountName => _accountName.Value;

        public string Identifier => _identifier.Value;

        public Nullable<bool> IsEnabled => _isEnabled.Value;

        public string Type => _type.Value;

        public string UserName => _userName.Value;

        public string ServerURL => _serverURL.Value;

        #endregion

        public AccountLogItem(LogEntry logEntry, IList<string> entryProperties)
            : base(logEntry, LogEntryType.Account, entryProperties)
        {
            // Retrieving the properties in an order that was provided to me 
            // from Kent as a way to interpret a single Account log entry. 

            _accountName = new Lazy<string>(() => EntryProperties.FirstOrDefault());
            _identifier = new Lazy<string>(() => EntryProperties.Skip(1).FirstOrDefault());

            _isEnabled = new Lazy<Nullable<bool>>(
                () =>
                {
                    string isEnabledString = EntryProperties.Skip(2).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(isEnabledString))
                    {
                        if (bool.TryParse(isEnabledString, out var isEnabled))
                        {
                            return isEnabled;
                        }
                    }

                    return null;
                });

            _type = new Lazy<string>(() => EntryProperties.Skip(3).FirstOrDefault());
            _userName = new Lazy<string>(() => EntryProperties.Skip(4).FirstOrDefault());
            _serverURL = new Lazy<string>(() => EntryProperties.Skip(5).FirstOrDefault());
        }
    }
}
