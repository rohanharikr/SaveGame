using System.Windows.Controls;
using SaveGame.Services;
using SaveGame.Stores;
using SaveGame.ViewModels;

namespace SaveGame.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : Page
    {
        public HomeView(ModalNavigationStore modalNavigationStore, GameStore gameStore, IGDBService igdbService)
        {
            InitializeComponent();
            DataContext = new HomeViewModel(modalNavigationStore, gameStore, igdbService);
        }
    }
}
