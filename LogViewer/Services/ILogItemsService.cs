using LogViewer.Base.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogViewer.Services
{
    /// <summary>
    /// Interface to be implemented by classes that parse log items. 
    /// </summary>
    public interface ILogItemsService
    {
        /// <summary>
        /// Event fired when log items from a file were parsed. 
        /// </summary>
        event EventHandler<FileLogItemsParsedEventArgs> FileLogItemsParsed;

        /// <summary>
        /// Parses log items from <paramref name="filePath"/> and notifies observers with firing 
        /// <see cref="ILogItemsService.FileLogItemsParsed"/> event as it goes forward. 
        /// </summary>
        /// 
        /// <returns>>List of parsed log items.</returns>
        Task<IList<LogItem>> GetLogItemsAsync(string filePath);
    }
}
