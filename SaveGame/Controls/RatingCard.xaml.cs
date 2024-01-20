using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SaveGame.Controls
{
    public partial class RatingCard : UserControl
    {
        #region Dependency properties
        public double Rating
        {
            get { return (double)GetValue(RatingProperty); }
            set { SetValue(RatingProperty, value); }
        }
        private static readonly DependencyProperty RatingProperty = DependencyProperty.Register(
           "Rating", typeof(double), typeof(RatingCard), new PropertyMetadata(0.0, new PropertyChangedCallback(RatingChanged)));
        #endregion

        public RatingCard()
        {
            InitializeComponent();
            Border.Background = Brushes.Gray; //TBD This is hack - RatingChanged does not get fired the first time
        }

        private static void RatingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RatingCard ratingCard = (RatingCard)d;
            double newRating = (double)e.NewValue;

            if (newRating >= 66)
                ratingCard.Border.Background = Brushes.Green;
            else if (newRating >= 33)
                ratingCard.Border.Background = Brushes.Yellow;
            else if (newRating > 0)
                ratingCard.Border.Background = Brushes.Red;
            else
                ratingCard.Border.Background = Brushes.Gray;
        }
    }
}
