using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
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
    public partial class RatingCard : UserControl
    {
        private static readonly DependencyProperty RatingProperty = DependencyProperty.Register(
           "Rating", typeof(double), typeof(RatingCard), new PropertyMetadata(0.0, new PropertyChangedCallback(RatingChanged)));

        public double Rating
        {
            get { return (double)GetValue(RatingProperty); }
            set { SetValue(RatingProperty, value); }
        }

        public RatingCard()
        {
            InitializeComponent();
            border.Background = Brushes.Gray; //this is hack - RatingChanged does not get fired 
        }

        private static void RatingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RatingCard ratingCard = (RatingCard)d;
            double newRating = (double)e.NewValue;

            if (newRating >= 66)
                ratingCard.border.Background = Brushes.Green;
            else if (newRating >= 33)
                ratingCard.border.Background = Brushes.Yellow;
            else if (newRating > 0)
                ratingCard.border.Background = Brushes.Red;
            else
                ratingCard.border.Background = Brushes.Gray;
        }
    }
}
