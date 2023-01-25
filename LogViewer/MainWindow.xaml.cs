using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading.Tasks;
using Accessibility;
using LogViewer.Base;
using LogViewer.Base.Models;
using LogViewer.Base.Parsers;

namespace LogViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // TODO: 
            // Change this. It's a temporary solution for an initial quick test to see the data displayed. 

            string[] allLines = System.IO.File.ReadAllLines("85C27NK92C.com.flexibits.fantastical2.mac.helper 2022-11-26--03-25-05-612.log");

            Queue<LogEntry> logEntries = new Queue<LogEntry>(new LogFileSplitter().SplitEntries(allLines));

            List<ILogEntryParser> logEntryParsers =
                new List<ILogEntryParser>()
                {
                    new AccountLogEntryParser(),
                    new CalendarLogEntryParser(),
                    new CurrentCalendarSetLogEntryParser(),
                    new SyncQueuesLogEntryParser(),
                };

            List<LogItem> allLogItems = new List<LogItem>();
            while (logEntries.Any())
            {
                bool lineParsed = false;
                foreach (ILogEntryParser parser in logEntryParsers)
                {
                    if (parser.TryParse(logEntries, out var newlyParsedEntries))
                    {
                        lineParsed = true;
                        allLogItems.AddRange(newlyParsedEntries);

                        break;
                    }
                }

                if (!lineParsed)
                {
                    logEntries.Dequeue();
                }
            }

            LogItemsViewModel logItemsViewModel = new LogItemsViewModel(allLogItems);
            DataContext = logItemsViewModel;
        }
    }
}
