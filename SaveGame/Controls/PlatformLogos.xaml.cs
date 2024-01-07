using IGDB.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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
           "Platforms", typeof(Platform[]), typeof(PlatformLogos), new PropertyMetadata(new Platform[0], new PropertyChangedCallback(PlatformChanged)));

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

            IEnumerable<string> platformSlugs = platforms.Select(platform => platform.PlatformFamily?.Value?.Slug ?? platform.Slug).Distinct();
            foreach(string platformSlug in platformSlugs)
            {
                Image platformLogo = new Image();
                platformLogo.Height = 18;
                platformLogo.Margin= new Thickness(10, 0, 0, 0);

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
