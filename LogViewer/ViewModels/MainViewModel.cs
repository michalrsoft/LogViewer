using LogViewer.Services;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace LogViewer.ViewModels
{
    /// <summary>
    /// Main Window view model that supports a command <see cref="MainViewModel.OpenLogFilesCommand"/> to grab list of files from the 
    /// user with ways for the list of files to be bound to the view and displayed. 
    /// </summary>
    public class MainViewModel
    {
        public const string ZipFileExtension = ".zip";

        private ILogItemsService _logItemsService;

        public ObservableCollection<LogFile> LogFiles { get; private set; }

        public RelayCommand OpenLogFilesCommand { get; }

        public MainViewModel(ILogItemsService logItemsService)
        {
            ArgumentNullException.ThrowIfNull(logItemsService);

            _logItemsService = logItemsService;

            LogFiles = new ObservableCollection<LogFile>();

            OpenLogFilesCommand = new RelayCommand(async () => await OpenLogFilesAsync());
        }

        /// <summary>
        /// Method extracts files from the <paramref name="zipFilePath"/> and grabs the *.log files. 
        /// </summary>
        /// 
        /// <remarks>
        /// Extraction happens into a temporary Windows folder which I don't clean up. 
        /// </remarks>
        protected virtual List<string> ExtractLogsFromZipFile(string zipFilePath)
        {
            ArgumentNullException.ThrowIfNull(zipFilePath);

            string temporaryDirectory = CreateNewTemporaryDirectory();

            List<string> extractedLogFiles = new List<string>();

            ZipFile.ExtractToDirectory(zipFilePath, temporaryDirectory);
            foreach (string filePath in Directory.GetFiles(temporaryDirectory, "*.log"))
            {
                extractedLogFiles.Add(filePath);
            }

            return extractedLogFiles;

            // TODO: 
            // Need to remove the temp folder at some point when we don't need its contents anymore. 
            // Should track it in the application and remove eventually. 
        }

        protected virtual string CreateNewTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        private async Task OpenLogFilesAsync()
        {
            try
            {
                // Open a dialog for the user to choose files. 
                // Then grab *.log and *.zip files. 
                // Extract the *.zip files and grab *.log files from them & build a final list of files. 

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.DefaultExt = ".log";
                openFileDialog.Filter = "Log files (.log)|*.log|Zip files (.zip)|*.zip";
                openFileDialog.Multiselect = true;

                List<string> allLogFilePaths = new List<string>();

                bool? result = openFileDialog.ShowDialog();

                if ((result == true) && (openFileDialog.FileNames.Any()))
                {
                    foreach (string filePath in openFileDialog.FileNames)
                    {
                        if (string.Equals(Path.GetExtension(filePath), MainViewModel.ZipFileExtension, StringComparison.OrdinalIgnoreCase))
                        {
                            allLogFilePaths.AddRange(ExtractLogsFromZipFile(filePath));
                        }
                        else
                        {
                            allLogFilePaths.Add(filePath);
                        }
                    }

                    if (allLogFilePaths.Any())
                    {
                        LogFiles.Clear();
                        foreach (string logFilePath in allLogFilePaths)
                        {
                            string logFileName = Path.GetFileName(logFilePath);
                            LogFiles.Add(new LogFile(_logItemsService, logFileName, logFilePath));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: Handle. 
            }
        }
    }
}
