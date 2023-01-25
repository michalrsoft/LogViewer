using Microsoft.Extensions.DependencyInjection;

namespace LogViewer.ViewModels
{
    /// <summary>
    /// Class brings a construct to bind the view to the ViewModel with MVVM concept 
    /// having the <see cref="MainViewModel"/> instantiated by Dependency Injection. 
    /// </summary>
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
