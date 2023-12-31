﻿using IGDB.Models;
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
    /// Interaction logic for GameDetailModal.xaml
    /// </summary>
    public partial class GameDetailModal : UserControl
    {
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

        public GameDetailModal()
        {
            InitializeComponent();
        }
    }
}
