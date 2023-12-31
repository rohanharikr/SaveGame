using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IGDB;
using IGDB.Models;

namespace SaveGame.Controls
{
    public partial class GameCard : UserControl
    {
        List<BitmapImage> _screenshots = new();
        double segmentWidth = 0;

        public IList<Screenshot> Screenshots
        {
            get { return (IList<Screenshot>)GetValue(ScreenshotsProperty); }
            set { SetValue(ScreenshotsProperty, value); }
        }

        public static readonly DependencyProperty ScreenshotsProperty =
             DependencyProperty.Register("Screenshots", typeof(IList<Screenshot>), typeof(GameCard), new PropertyMetadata(new List<Screenshot>()));

        public Uri CoverArt
        {
            get { return (Uri)GetValue(CoverArtProperty); }
            set { SetValue(CoverArtProperty, value); }
        }

        public static readonly DependencyProperty CoverArtProperty =
            DependencyProperty.Register("CoverArt", typeof(Uri), typeof(GameCard), new PropertyMetadata(null));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(GameCard));

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(Game), typeof(GameCard), new PropertyMetadata(null));

        public GameCard()
        {
            InitializeComponent();
            Loaded += GameCard_Loaded;
        }

        private void GameCard_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(var screenshot in Screenshots)
            {
                if (_screenshots.Count >= 3)
                    break;

                Uri imageUri = new Uri("https:" + IGDB.ImageHelper.GetImageUrl(imageId: screenshot.ImageId, size: ImageSize.ScreenshotMed, retina: true));
                BitmapImage bitmapImage = new BitmapImage(imageUri);
                _screenshots.Add(bitmapImage);

            }
            segmentWidth = Grid.Width / _screenshots.Count;
        }

        private void EnterPreview(object sender, MouseEventArgs e)
        {
            if (_screenshots.Count <= 1)
                return;

            Bars.Visibility = Visibility.Visible;
        }

        private void ExitPreview(object sender, MouseEventArgs e)
        {
            if (_screenshots.Count <= 1)
                return;

            Bars.Visibility = Visibility.Collapsed;
            Grid.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(CoverArt),
                Stretch = Stretch.UniformToFill
            };
        }

        int selectedBar = -1;

        private void Preview(object sender, MouseEventArgs e)
        {
            if (_screenshots.Count <= 1)
                return;

            Point mousePosition = e.GetPosition(Grid);
            double mouseX = mousePosition.X;
            int segmentIndex = (int)(mouseX / segmentWidth);
            segmentIndex = Math.Max(0, Math.Min(segmentIndex, _screenshots.Count - 1));

            if(selectedBar != segmentIndex)
            {
                for (int i=0; i<Bars.Children.Count; i++)
                {
                    if (segmentIndex == i)
                        Bars.Children[segmentIndex].Opacity = 1;
                    else
                        Bars.Children[i].Opacity = 0.65;
                };
                Grid.Background = new ImageBrush
                {
                    ImageSource = _screenshots[segmentIndex],
                    Stretch = Stretch.UniformToFill
                };
                selectedBar = segmentIndex;
            }
        }
    }
}
