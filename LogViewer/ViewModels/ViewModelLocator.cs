using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogViewer.Services;
using LogViewer.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;

namespace LogViewer.ViewModels
{
    public class ViewModelLocator
    {
        public MainViewModel TopViewModel
        {
            get
            {
                return App.ServiceProvider.GetRequiredService<MainViewModel>();
            }
        }

        public ViewModelLocator()
        {

        }
    }
}
