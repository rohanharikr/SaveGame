using SaveGame.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace SaveGame.Controls
{
    public partial class GameDetailModal : UserControl
    {
        #region Dependency properties
        public ICommand AddToPlay
        {
            get { return (ICommand)GetValue(AddToPlayProperty); }
            set { SetValue(AddToPlayProperty, value); }
        }
        public static readonly DependencyProperty AddToPlayProperty =
            DependencyProperty.Register("AddToPlay", typeof(ICommand), typeof(GameDetailModal));

        public ICommand AddToPlaying
        {
            get { return (ICommand)GetValue(AddToPlayingProperty); }
            set { SetValue(AddToPlayingProperty, value); }
        }
        public static readonly DependencyProperty AddToPlayingProperty =
            DependencyProperty.Register("AddToPlaying", typeof(ICommand), typeof(GameDetailModal));

        public ICommand AddToPlayed
        {
            get { return (ICommand)GetValue(AddToPlayedProperty); }
            set { SetValue(AddToPlayedProperty, value); }
        }
        public static readonly DependencyProperty AddToPlayedProperty =
            DependencyProperty.Register("AddToPlayed", typeof(ICommand), typeof(GameDetailModal));

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(Game), typeof(GameDetailModal), new PropertyMetadata(null));

        public ICommand Remove
        {
            get { return (ICommand)GetValue(RemoveProperty); }
            set { SetValue(RemoveProperty, value); }
        }
        public static readonly DependencyProperty RemoveProperty =
            DependencyProperty.Register("Remove", typeof(ICommand), typeof(GameDetailModal));

        public Game Detail
        {
            get { return (Game)GetValue(DetailProperty); }
            set { SetValue(DetailProperty, value); }
        }
        public static readonly DependencyProperty DetailProperty =
            DependencyProperty.Register("Detail", typeof(Game), typeof(GameDetailModal), new PropertyMetadata(null));

        public ICommand CloseModal
        {
            get { return (ICommand)GetValue(CloseModalProperty); }
            set { SetValue(CloseModalProperty, value); }
        }
        public static readonly DependencyProperty CloseModalProperty =
            DependencyProperty.Register("CloseModal", typeof(ICommand), typeof(GameDetailModal));
        #endregion

        readonly Storyboard? showModalSb;

        public GameDetailModal()
        {
            InitializeComponent();
            
            showModalSb = FindResource("ShowModalAnimation") as Storyboard;
            
            DependencyPropertyDescriptor.FromProperty(UserControl.VisibilityProperty, typeof(UserControl))
                .AddValueChanged(this, VisibilityPropertyChanged);
        }

        private void VisibilityPropertyChanged(object? sender, EventArgs e)
        {
            if (Visibility == Visibility.Collapsed)
            {
                showModalSb?.Remove();
                scrollviewer.ScrollToTop();
            }

            if (Visibility == Visibility.Visible)
                showModalSb?.Begin();
        }
    }
}
