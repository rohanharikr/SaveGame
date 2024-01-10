using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using IGDB;
using SaveGame.Models;
using IGDB.Models;

namespace SaveGame.Controls
{
    public partial class GameCard : UserControl
    {
        readonly List<BitmapImage> _screenshots = [];
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


        public ICommand AddToPlay
        {
            get { return (ICommand)GetValue(AddToPlayProperty); }
            set { SetValue(AddToPlayProperty, value); }
        }

        public static readonly DependencyProperty AddToPlayProperty =
            DependencyProperty.Register("AddToPlay", typeof(ICommand), typeof(GameCard));

        public ICommand AddToPlaying
        {
            get { return (ICommand)GetValue(AddToPlayingProperty); }
            set { SetValue(AddToPlayingProperty, value); }
        }

        public static readonly DependencyProperty AddToPlayingProperty =
            DependencyProperty.Register("AddToPlaying", typeof(ICommand), typeof(GameCard));

        public ICommand AddToPlayed
        {
            get { return (ICommand)GetValue(AddToPlayedProperty); }
            set { SetValue(AddToPlayedProperty, value); }
        }

        public static readonly DependencyProperty AddToPlayedProperty =
            DependencyProperty.Register("AddToPlayed", typeof(ICommand), typeof(GameCard));

        public ICommand Remove
        {
            get { return (ICommand)GetValue(RemoveProperty); }
            set { SetValue(RemoveProperty, value); }
        }

        public static readonly DependencyProperty RemoveProperty =
            DependencyProperty.Register("Remove", typeof(ICommand), typeof(GameCard));

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(SaveGame.Models.Game), typeof(GameCard), new PropertyMetadata(null));

        public object State
        {
            get { return GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(SaveGame.Models.PlayStates), typeof(GameCard), new PropertyMetadata(PlayStates.None, new PropertyChangedCallback(StatePropertyChanged)));

        public GameCard()
        {
            InitializeComponent();
            Loaded += GameCard_Loaded;
        }

        private static void StatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GameCard gameCard = (GameCard)d;
            UpdateContext(gameCard);
        }

        static void UpdateContext(GameCard gameCard)
        { 
            PlayStates playState = (PlayStates)gameCard.State;
            gameCard.AddToPlayMenuItem.IsChecked = playState == PlayStates.Play;
            gameCard.AddToPlayingMenuItem.IsChecked = playState == PlayStates.Playing;
            gameCard.AddToPlayedMenuItem.IsChecked = playState == PlayStates.Played;
            if (playState == PlayStates.None)
                gameCard.RemoveMenuItem.Visibility = Visibility.Collapsed;
            else
                gameCard.RemoveMenuItem.Visibility = Visibility.Visible;
        }

        private void GameCard_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateContext(this);

            foreach (var screenshot in Screenshots)
            {
                if (_screenshots.Count >= 3)
                    break;

                Uri imageUri = new("https:" + IGDB.ImageHelper.GetImageUrl(imageId: screenshot.ImageId, size: ImageSize.ScreenshotMed, retina: true));
                BitmapImage bitmapImage = new(imageUri);
                _screenshots.Add(bitmapImage);

            }
            segmentWidth = Border.Width / _screenshots.Count;

            //trying to bind the command in XAML was a major PITA
            AddToPlayMenuItem.Command = AddToPlay;
            AddToPlayMenuItem.CommandParameter = CommandParameter;
            AddToPlayingMenuItem.Command = AddToPlaying;
            AddToPlayingMenuItem.CommandParameter = CommandParameter;
            AddToPlayedMenuItem.Command = AddToPlayed;
            AddToPlayedMenuItem.CommandParameter = CommandParameter;
            RemoveMenuItem.Command = Remove;
            RemoveMenuItem.CommandParameter = CommandParameter;
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
            /*
              TBD Mos of screenshots are currently hard coded to 3
              Handle cases where games might not have 3 screenshot - display hover bars accordingly.
            */

            if (_screenshots.Count <= 1)
                return;

            Point mousePosition = e.GetPosition(Border);
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
