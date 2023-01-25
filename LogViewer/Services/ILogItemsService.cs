using LogViewer.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Services
{
    public interface ILogItemsService
    {
        event EventHandler<FileLogItemsParsedEventArgs> FileLogItemsParsed;

        Task<IList<LogItem>> GetLogItemsAsync(string filePath);
    }
}
