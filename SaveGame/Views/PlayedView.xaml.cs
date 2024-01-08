using SaveGame.Stores;
using SaveGame.ViewModels;
using System.Windows.Controls;

namespace SaveGame.Views
{
    /// <summary>
    /// Interaction logic for PlayedView.xaml
    /// </summary>
    public partial class PlayedView : Page
    {
        public PlayedView(ModalNavigationStore modalNavigationStore, GameStore gameStore)
        {
            InitializeComponent();
            DataContext = new PlayedViewModel(modalNavigationStore, gameStore);
        }
    }
}
