using System.Windows;
using System.Windows.Input;

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

        private void CloseApp(object sender, RoutedEventArgs e) => Close();

        private void DragApp(object sender, MouseButtonEventArgs e) => DragMove();

        private void MinimizeApp(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    }
}