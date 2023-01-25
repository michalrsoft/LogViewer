using System.Globalization;
using System.Text.RegularExpressions;

namespace LogViewer.Base.Models
{
    /// <summary>
    /// Class represents a SyncQueue log item. 
    /// </summary>
    /// 
    /// <remarks>It uses Regex to parse properties of the SyncQueue log item.</remarks>
    public class SyncQueuesLogItem : LogItem
    {
        public const string ClassAccountPattern = @"([\w\-]+)\s*\/\s*([\w\-]+)\s*";

        public const string UserNameDatePattern = @"\(([\w\.\-@]+)\,\s+last\ssync\:\s+([0-9-]+)\s+([0-9\:]+\s+\+[0-9]+)\,\s+[0-9]+\)\:\s+";

        private static readonly Regex _classAccountPatternRegex = new Regex(SyncQueuesLogItem.ClassAccountPattern);

        private static readonly Regex _userNameDatePatternRegex = new Regex(SyncQueuesLogItem.UserNameDatePattern);

        #region Private fields

        private string _className;

        private string _accountIdentifier;

        private string _userName;

        private DateTime _lastSyncDate;

        private string _objectLog;

        private string _syncQueueString;

        #endregion

        #region Properties

        public string ClassName => _className;

        public string AccountIdentifier => _accountIdentifier;

        public string UserName => _userName;

        public DateTime LastSyncDate => _lastSyncDate;

        public string ObjectLog => _objectLog;

        public string SyncQueueString => _syncQueueString;

        public override IList<string> Lines => new List<string>() { _syncQueueString };

        public override string Contents => _syncQueueString;

        #endregion

        public SyncQueuesLogItem(LogEntry logLine, string syncQueueString)
            : base(logLine, LogEntryType.SyncQueues)
        {
            // Retrieving the properties in an order that was provided to me 
            // from Kent as a way to interpret a single Sync Queue log entry.

            // Also retrieving the object debug information log as one entry - this one is not being parsed to details.
            // TODO: Parse the object debug information from _objectLog. 

            ArgumentNullException.ThrowIfNull(syncQueueString);

            _syncQueueString = syncQueueString;

            Match classAccountPatternMatch = _classAccountPatternRegex.Match(syncQueueString);
            if ((classAccountPatternMatch.Success) && (classAccountPatternMatch.Groups.Count == 3))
            {
                _className = classAccountPatternMatch.Groups[1].Value;
                _accountIdentifier = classAccountPatternMatch.Groups[2].Value;

                string syncQueueNextPart = syncQueueString.Substring(classAccountPatternMatch.Groups[0].Value.Length).Trim();
                if (!string.IsNullOrWhiteSpace(syncQueueNextPart))
                {
                    Match userNameDatePatternMatch = _userNameDatePatternRegex.Match(syncQueueNextPart);
                    if ((userNameDatePatternMatch.Success) && (userNameDatePatternMatch.Groups.Count == 4))
                    {
                        _userName = userNameDatePatternMatch.Groups[1].Value;

                        string date = userNameDatePatternMatch.Groups[2].Value;
                        string time = userNameDatePatternMatch.Groups[3].Value;

                        DateTime.TryParse($"{date} {time}", CultureInfo.InvariantCulture, DateTimeStyles.None, out _lastSyncDate);

                        _objectLog = syncQueueNextPart.Substring(userNameDatePatternMatch.Groups[0].Value.Length).Trim();
                    }
                }
            }
        }
    }
}
