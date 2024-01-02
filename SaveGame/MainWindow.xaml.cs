using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SaveGame.ViewModels;

namespace SaveGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DragApp(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void MinimizeApp(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}