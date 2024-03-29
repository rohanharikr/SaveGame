﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SaveGame.Controls
{
    public partial class NeonText : UserControl
    {
        #region Dependency properties
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(NeonText), new PropertyMetadata("Mouse Events"));

        public Color GlowColor
        {
            get { return (Color)GetValue(GlowColorProperty); }
            set { SetValue(GlowColorProperty, value); }
        }
        static Color _defaultColor = (Color)ColorConverter.ConvertFromString("#47bdfc");
        public static readonly DependencyProperty GlowColorProperty =
            DependencyProperty.Register("GlowColor", typeof(Color), typeof(NeonText), new PropertyMetadata(_defaultColor));

        public bool ActivateBlink
        {
            get { return (bool)GetValue(ActivateBlinkProperty); }
            set { SetValue(ActivateBlinkProperty, value); }
        }
        public static readonly DependencyProperty ActivateBlinkProperty =
            DependencyProperty.Register("ActivateBlink", typeof(bool), typeof(NeonText), new PropertyMetadata(false));
        #endregion


        public NeonText()
        {
            InitializeComponent();
        }
    }
}
