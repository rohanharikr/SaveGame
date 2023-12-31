using IGDB;
using IGDB.Models;
using System;
using System.Collections.Generic;
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

namespace SaveGame.Controls
{
    /// <summary>
    /// Interaction logic for ScreenshotsCarousel.xaml
    /// </summary>
    public partial class ScreenshotsCarousel : UserControl
    {
        public Uri CoverArt
        {
            get { return (Uri)GetValue(CoverArtProperty); }
            set { SetValue(CoverArtProperty, value); }
        }

        public static readonly DependencyProperty CoverArtProperty =
            DependencyProperty.Register("CoverArt", typeof(Uri), typeof(ScreenshotsCarousel), new PropertyMetadata(null));

        public IList<Screenshot> Screenshots
        {
            get { return (IList<Screenshot>)GetValue(ScreenshotsProperty); }
            set { SetValue(ScreenshotsProperty, value); }
        }

        public static readonly DependencyProperty ScreenshotsProperty =
             DependencyProperty.Register("Screenshots", typeof(IList<Screenshot>), typeof(ScreenshotsCarousel), new PropertyMetadata(new List<Screenshot>()));

        public ScreenshotsCarousel()
        {
            InitializeComponent();

            Loaded += ScreenshotsCarousel_Loaded;
        }

        private void ScreenshotsCarousel_Loaded(object sender, RoutedEventArgs e)
        {
            if(Screenshots.Count > 1)
            {
                PrevScreenshotButton.Visibility = Visibility.Visible;
                NextScreenshotButton.Visibility = Visibility.Visible;
            }
        }

        int currentScreenshotIndex = 1;
        private void NextScreenshot(object sender, RoutedEventArgs e)
        {
            PrevScreenshotButton.Visibility = Visibility.Visible;

            if (currentScreenshotIndex >= Screenshots.Count)
                return;

            if (currentScreenshotIndex == 1)
            {
                CarouselScrollViewer.ScrollToHorizontalOffset(500);
            }
            else if(currentScreenshotIndex == Screenshots.Count - 1)
            {
                CarouselScrollViewer.ScrollToRightEnd();
                NextScreenshotButton.Visibility = Visibility.Collapsed;
            }
            else 
            { 
                CarouselScrollViewer.ScrollToHorizontalOffset(460 * currentScreenshotIndex);
            }
            currentScreenshotIndex++;
        }

        private void PrevScreenshot(object sender, RoutedEventArgs e)
        {
            NextScreenshotButton.Visibility = Visibility.Visible;

            if (currentScreenshotIndex <= 1)
                return;

            if (currentScreenshotIndex == Screenshots.Count)
            {
                CarouselScrollViewer.ScrollToHorizontalOffset(CarouselScrollViewer.ScrollableWidth - 310);
            }
            else if (currentScreenshotIndex == 2)
            {
                CarouselScrollViewer.ScrollToLeftEnd();
                PrevScreenshotButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                CarouselScrollViewer.ScrollToHorizontalOffset((450 * currentScreenshotIndex) - 860);
            }
            currentScreenshotIndex--;
        }
    }
}
