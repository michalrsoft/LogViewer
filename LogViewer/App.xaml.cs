using LogViewer.Services;
using LogViewer.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LogViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost host;

        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            host = 
                Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, builder) => { })
                .ConfigureServices((context, services) => ConfigureServices(context.Configuration, services))
                .Build();

            ServiceProvider = host.Services;
        }

        private void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            // Registering all services. 
            services.AddTransient<ILogItemsService, LogItemsService>(
                (serviceProvider) =>
                {
                    return new LogItemsService();
                });

            // Registering all view models. 
            services.AddSingleton<MainViewModel>();

            services.AddTransient(typeof(MainWindow));
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await host.StartAsync();

            var window = ServiceProvider.GetRequiredService<MainWindow>();
            window.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (host)
            {
                await host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }
    }
}
