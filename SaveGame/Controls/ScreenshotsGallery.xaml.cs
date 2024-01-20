using IGDB.Models;
using System.Windows;
using System.Windows.Controls;

namespace SaveGame.Controls
{
    public partial class ScreenshotsGallery : UserControl
    {
        #region Dependency properties
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
        #endregion

        public ScreenshotsGallery()
        {
            InitializeComponent();
        }
    }
}
