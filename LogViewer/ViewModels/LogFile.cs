using LogViewer.Services;
using System;

namespace LogViewer.ViewModels
{
    /// <summary>
    /// View Model for a single file entry on the list of files loaded into the application. It contains logic for building 
    /// <see cref="LogsViewModel"/> in a lazy manner when the logs data from the file is requested. 
    /// </summary>
    public class LogFile
    {
        private ILogItemsService _logItemsService;

        private Lazy<LogsViewModel> _logsViewModel;

        #region Properties

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public LogsViewModel LogsViewModel
        {
            get
            {
                return _logsViewModel.Value;
            }
        }

        #endregion

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
                        // Build a new View Model to display Log Files Data. The data will be parsed on a separate thread and then 
                        // bound back to the UI. 

                        LogsViewModel logsViewModel = new LogsViewModel(_logItemsService, FilePath);
                        logsViewModel.StartLogProcessing();

                        return logsViewModel;
                    });
        }
    }
}
