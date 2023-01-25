using LogViewer.Base.Models;
using LogViewer.Base.Parsers;
using LogViewer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogViewer.Services;

namespace LogViewer.ViewModels
{
    public class LogFile
    {
        private ILogItemsService _logItemsService;

        private Lazy<LogsViewModel> _logsViewModel;

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public LogsViewModel LogsViewModel
        {
            get
            {
                return _logsViewModel.Value;
            }
        }

        public LogFile(ILogItemsService logItemsService, string fileName, string filePath)
        {
            ArgumentNullException.ThrowIfNull(logItemsService);
            ArgumentNullException.ThrowIfNull(fileName);
            ArgumentNullException.ThrowIfNull(filePath);

            _logItemsService = logItemsService;

            FileName = fileName;
            FilePath = filePath;

            _logsViewModel =
                new Lazy<LogsViewModel>(
                    () =>
                    {
                        LogsViewModel logsViewModel = new LogsViewModel(_logItemsService, FilePath);
                        logsViewModel.StartLogProcessing();

                        return logsViewModel;
                    });
        }
    }
}
