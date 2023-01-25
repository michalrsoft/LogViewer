using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Accessibility;
using LogViewer.Base;
using LogViewer.Base.Models;
using LogViewer.Base.Parsers;
using LogViewer.Services;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;

namespace LogViewer.ViewModels
{
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
