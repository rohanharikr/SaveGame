using IGDB.Models;
using System.Windows;
using System.Windows.Controls;

namespace SaveGame.Controls
{
    /// <summary>
    /// Interaction logic for ScreenshotsCarousel.xaml
    /// </summary>
    public partial class ScreenshotsGallery : UserControl
    {
        public Uri CoverArt
        {
            get { return (Uri)GetValue(CoverArtProperty); }
            set { SetValue(CoverArtProperty, value); }
        }

        public static readonly DependencyProperty CoverArtProperty =
            DependencyProperty.Register("CoverArt", typeof(Uri), typeof(ScreenshotsGallery), new PropertyMetadata(null));

        public IList<Screenshot> Screenshots
        {
            get { return (IList<Screenshot>)GetValue(ScreenshotsProperty); }
            set { SetValue(ScreenshotsProperty, value); }
        }

        public static readonly DependencyProperty ScreenshotsProperty =
             DependencyProperty.Register("Screenshots", typeof(IList<Screenshot>), typeof(ScreenshotsGallery), new PropertyMetadata(new List<Screenshot>()));

        public ScreenshotsGallery()
        {
            InitializeComponent();
        }

        /*private static void ScreenshotsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //preload screenshots - so that you dont see loading of screenshots on modal open
            ScreenshotsGallery gallery = (ScreenshotsGallery)d;
            IList<Screenshot> screenshots = (IList<Screenshot>)e.NewValue;

            for(int i = 0; i<5; i++)
            {
                if (screenshots.Count <= i)
                    continue;

                if (gallery.container.Children[i] is Border border)
                {
                    Uri imageUri = new("https:" + IGDB.ImageHelper.GetImageUrl(imageId: screenshots[i].ImageId, size: ImageSize.Thumb, retina: false));
                    BitmapImage bitmapImage = new(imageUri);

                    border.Background = new ImageBrush
                    {
                        ImageSource = bitmapImage,
                        Stretch = Stretch.UniformToFill
                    };
                }
            }
        }*/
    }
}
