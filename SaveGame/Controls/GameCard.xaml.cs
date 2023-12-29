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
    /// Interaction logic for GameCard.xaml
    /// </summary>
    public partial class GameCard : UserControl
    {
        public GameCard()
        {
            InitializeComponent();
        }

        public Uri CoverArt
        {
            get { return (Uri)GetValue(CoverArtProperty); }
            set { SetValue(CoverArtProperty, value); }
        }

        public static readonly DependencyProperty CoverArtProperty =
            DependencyProperty.Register("CoverArt", typeof(Uri), typeof(GameCard), new PropertyMetadata(null));

        int? selectedBar = null;
        private void Preview(object sender, MouseEventArgs e)
        {
            string[] images =
            {
                "https://content.halocdn.com/media/Default/community/blogs/hi_campaign_sniper_4k-e327d439d8714ed481c5de4b1b7fcc81.png",
                "https://www.nme.com/wp-content/uploads/2021/12/Stalker-2-screenshot-featured-2000x1270-1.jpg",
                "https://gamingbolt.com/wp-content/uploads/2013/12/1.-Assassins-Creed-4.jpg",
                "https://xxboxnews.blob.core.windows.net/prod/sites/2/2023/10/AFOP_Screenshot_ATTACKING_THE_RDA-551e7d6f75844a992285.jpg",
                "https://cdn.mos.cms.futurecdn.net/ekWPT2WBsFikaoXdav9qNC-1200-80.jpg"
            };
            Bars.Visibility = Visibility.Visible;
            Point mousePosition = e.GetPosition(Grid);
            double mouseX = mousePosition.X;
            double segmentWidth = 180 / 3;
            int segmentIndex = (int)(mouseX / segmentWidth);
            segmentIndex = Math.Max(0, Math.Min(segmentIndex, 3 - 1));
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
                    ImageSource = new BitmapImage(new Uri(images[segmentIndex])),
                    Stretch = Stretch.UniformToFill
                };
                selectedBar = segmentIndex;
            }
        }

        private void ExitPreview(object sender, MouseEventArgs e)
        {
            Bars.Visibility = Visibility.Collapsed;
            Grid.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(CoverArt),
                Stretch = Stretch.UniformToFill
            };
        }
    }
}
