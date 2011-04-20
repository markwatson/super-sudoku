using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SuperSudoku
{
    /// <summary>
    /// Interaction logic for the new game box.
    /// </summary>
    public partial class NewGameDifficultyDialogBox : Window
    {
        /// <summary>
        /// The difficulty selected.
        /// </summary>
        public Difficulty HowHard;

        /// <summary>
        /// The cancel button wasn't click, the game is a go.
        /// </summary>
        public bool CreateGame;

        /// <summary>
        /// Construct me a boat.
        /// </summary>
        public NewGameDifficultyDialogBox(bool noCancel=false)
        {
            InitializeComponent();

            HowHard = Difficulty.Easy;
            CreateGame = false;
            if (noCancel)
            {
                cancelButton.Visibility = Visibility.Hidden;
                this.WindowStyle = WindowStyle.None;
            }
        }

        /// <summary>
        /// When the OK button is clicked.
        /// </summary>
        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            // Find the correct difficulty
            if (easy.IsChecked != null && (bool) easy.IsChecked)
            {
                HowHard = Difficulty.Easy;
            }
            if (medium.IsChecked != null && (bool)medium.IsChecked)
            {
                HowHard = Difficulty.Medium;
            }
            if (hard.IsChecked != null && (bool)hard.IsChecked)
            {
                HowHard = Difficulty.Hard;
            }

            // the game is a go, weapons hot.
            CreateGame = true;

            Close();
        }
    }
}
