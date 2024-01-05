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
        public int Rating
        {
            get { return (int)GetValue(RatingProperty); }
            set { SetValue(RatingProperty, value); }
        }

        public static readonly DependencyProperty RatingProperty =
            DependencyProperty.Register("Rating", typeof(int), typeof(RatingCard), new PropertyMetadata(0));


        public RatingCard()
        {
            InitializeComponent();
        }

        private void RatingChanged()
        {
            if (Rating == 100)
                border.Background = Brushes.Green;
            else if (Rating >= 66)
                border.Background = Brushes.Yellow;
            else if (Rating >= 33)
                border.Background = Brushes.Red;
            else if (Rating == 0)
                border.Background = Brushes.Gray;
        }
    }
}
