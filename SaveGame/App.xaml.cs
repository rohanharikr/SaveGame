using Microsoft.EntityFrameworkCore.Infrastructure;
using SaveGame.Services;
using SaveGame.Stores;
using SaveGame.ViewModels;
using System.Windows;

namespace SaveGame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly GameStore _gameStore;
        private readonly IGDBService _igdbService;

        public App()
        {
            _modalNavigationStore = new ModalNavigationStore();
            _igdbService = new IGDBService();
            _gameStore = new GameStore(_igdbService);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_modalNavigationStore, _gameStore, _igdbService)
            };
            MainWindow.Show();

            DatabaseFacade facade = new(new SQLiteService());
            facade.EnsureCreated();

            base.OnStartup(e);
        }
    }

}
