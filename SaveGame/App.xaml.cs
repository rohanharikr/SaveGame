using Microsoft.EntityFrameworkCore.Infrastructure;
using SaveGame.Services;
using SaveGame.Stores;
using SaveGame.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;

namespace SaveGame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ModalNavigationStore _modalNavigationStore;

        public App()
        {
            _modalNavigationStore = new ModalNavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_modalNavigationStore)
            };
            MainWindow.Show();
            base.OnStartup(e);
            //DatabaseFacade facade = new(new SQLiteService());
            //facade.EnsureCreated();
        }
    }

}
