using SaveGame.Stores;
using SaveGame.ViewModels;
using System.Windows.Controls;

namespace SaveGame.Views
{
    /// <summary>
    /// Interaction logic for PlayingView.xaml
    /// </summary>
    public partial class PlayingView : Page
    {
        public PlayingView(ModalNavigationStore modalNavigationStore, GameStore gameStore)
        {
            InitializeComponent();
            DataContext = new PlayingViewModel(modalNavigationStore, gameStore);
        }
    }
}
