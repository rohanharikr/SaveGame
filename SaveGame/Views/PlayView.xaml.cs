using SaveGame.Stores;
using SaveGame.ViewModels;
using System.Windows.Controls;

namespace SaveGame.Views
{
    /// <summary>
    /// Interaction logic for PlayView.xaml
    /// </summary>
    public partial class PlayView : Page
    {
        public PlayView(ModalNavigationStore modalNavigationStore, GameStore gameStore)
        {
            InitializeComponent();
            DataContext = new PlayViewModel(modalNavigationStore, gameStore);
        }
    }
}
