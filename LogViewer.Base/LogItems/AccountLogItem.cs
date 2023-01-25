using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Base.Models
{
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

        public string AccountName => _accountName.Value;

        public string Identifier => _identifier.Value;

        public Nullable<bool> IsEnabled => _isEnabled.Value;

        public string Type => _type.Value;

        public string UserName => _userName.Value;

        public string ServerURL => _serverURL.Value;

        public AccountLogItem(LogEntry logEntry, IList<string> entryParts)
            : base(logEntry, LogEntryType.Account, entryParts)
        {
            _accountName = new Lazy<string>(() => EntryItems.FirstOrDefault());
            _identifier = new Lazy<string>(() => EntryItems.Skip(1).FirstOrDefault());

            _isEnabled = new Lazy<Nullable<bool>>(
                () =>
                {
                    string? isEnabledString = EntryItems.Skip(2).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(isEnabledString))
                    {
                        if (bool.TryParse(isEnabledString, out var isEnabled))
                        {
                            return isEnabled;
                        }
                    }

                    return null;
                });

            _type = new Lazy<string>(() => EntryItems.Skip(3).FirstOrDefault());
            _userName = new Lazy<string>(() => EntryItems.Skip(4).FirstOrDefault());
            _serverURL = new Lazy<string>(() => EntryItems.Skip(5).FirstOrDefault());
        }
    }
}
