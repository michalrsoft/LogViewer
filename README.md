# LogViewer

Contains: 
- Log Parsing Application
- Log Viewer (WPF)
- Console test app

Tooling: 
- Visual Studio 2022 (Community Edition) 
- C# 10 and .NET 6 

Projects: 

  - LogViewer.Base - basic project with the data types, models, parsing

  - LogViewer.ConsoleTest - a simple console app to test how the parsing works
  
  - LogViewer - Windows WPF application that displays a window. It allows the user to choose a *.log file (or multiple files, or a *.zip with *.log files). It then parses the contents and displays results in DataGrids. 

Concepts, design patterns, and framworks used: 

   - Chain Of Responsibility Pattern - used for parsing. Parsers defined in individual classes, each can handle one type of log items. Can extend with other item types.

   - MVVM and Dependency Injection - used for the LogViewer WPF application. 

   - Reactive Extensions for .NET - used for subscribing to events as file gets parsed and output results in an elegant event-driven manner.

Testing done: 

   - Manual testing with the provided log file 85C27NK92C.com.flexibits.fantastical2.mac.helper 2022-11-26--03-25-05-612 and some excerpts of it. 
   
   - LogViewer project comes with a logs.zip file that contains a few log files that can be fed to the application. 

TODOs: 

   - Exception Handling - the LogViewer Windows application needs to get a proper exception and error handling which I didn't spend time on. 
   - UI Testing and Parsing logic testing with Unit Tests 
   - Support for other log item types
   
