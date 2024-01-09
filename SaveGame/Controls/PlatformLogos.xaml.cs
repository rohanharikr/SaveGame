using IGDB.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SaveGame.Controls
{
    /// <summary>
    /// Interaction logic for Platforms.xaml
    /// </summary>
    public partial class PlatformLogos : UserControl
    {
        public Platform[] Platforms
        {
            get { return (Platform[])GetValue(PlatformsProperty); }
            set { SetValue(PlatformsProperty, value); }
        }

        private static readonly DependencyProperty PlatformsProperty = DependencyProperty.Register(
           "Platforms", typeof(Platform[]), typeof(PlatformLogos), new PropertyMetadata(Array.Empty<Platform>(), new PropertyChangedCallback(PlatformChanged)));

        public PlatformLogos()
        {
            InitializeComponent();
        }

        private static void PlatformChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlatformLogos platformsLogos = (PlatformLogos)d;
            platformsLogos.stackpanel.Children.Clear();

            Platform[] platforms = (Platform[])e.NewValue;
            string imageBasePath = "pack://application:,,,/Media/Images/";

            Enumerable<string> platformSlugs = platforms.Select(platform => platform.PlatformFamily?.Value?.Slug ?? platform.Slug).Distinct();
            foreach(string platformSlug in platformSlugs)
            {
                Image platformLogo = new()
                {
                    Height = 18,
                    Margin = new Thickness(10, 0, 0, 0)
                };

                switch (platformSlug)
                {
                    case "win":
                        platformLogo.Source = new BitmapImage(new Uri(imageBasePath + "pc.png"));
                        break;
                    case "mac":
                        platformLogo.Source = new BitmapImage(new Uri(imageBasePath + "mac.png"));
                        break;
                    case "linux":
                        platformLogo.Source = new BitmapImage(new Uri(imageBasePath + "linux.png"));
                        break;
                    case "playstation":
                        platformLogo.Source = new BitmapImage(new Uri(imageBasePath + "ps.png"));
                        break;
                    case "xbox":
                        platformLogo.Source = new BitmapImage(new Uri(imageBasePath + "xbox.png"));
                        break;
                    case "nintendo":
                        platformLogo.Source = new BitmapImage(new Uri(imageBasePath + "nintendo.png"));
                        break;
                }

                if (platformLogo.Source == null)
                    continue;

                platformsLogos.stackpanel.Children.Add(platformLogo);
            }
        }
    }
}
