using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace SuperSudoku
{
	/// <summary>
	/// GUI Logic goes in this MainWindow class. It also contains the state of the application.
	/// </summary>
	public partial class MainWindow : Window
	{
        // Game elements are implemented in seperate classes and instantiated to properties of this class here:
        FileHandler fileHandler;
        GameBoard gameBoard;

        // State properties are defined here
        private bool showHints = false;
        private bool showErrors = false;

        // GUI elements are defined here
        SaveFileDialog saveGameDialog;

        /// <summary>
        /// This function instantiates all the game objects and sets up the game window.
        /// </summary>
		public MainWindow()
		{
			this.InitializeComponent();

            // Initialize game elements
            fileHandler = new FileHandler();
            gameBoard = new GameBoard();

			// Initialize the save game dialog
            InitializeComponent();
            saveGameDialog = new SaveFileDialog();
            saveGameDialog.Filter = "Sudoku Games | *.sud";
            saveGameDialog.DefaultExt = ".sud";
		}

        // The following methods are helper methods
        /// <summary>
        /// This method shows the save file dialog box. It's called by the save game menu items.
        /// </summary>
        private void SaveFile(bool saveUnsolved = false)
        {
            // Show save file dialog box
            Nullable<bool> result = saveGameDialog.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string fileName = saveGameDialog.FileName;

                bool success = false;
                if (!saveUnsolved)
                {
                    success = fileHandler.SaveFile(gameBoard, fileName);
                }
                else
                {
                    success = fileHandler.SaveFileUnsolved(gameBoard, fileName);
                }

                if (!success)
                {
                    MessageBox.Show("There was a problem saving your file. Please try again.");
                }
            }
        }

        // The following methods are GUI event handlers. The two arguments are the same for all event handlers.
        /// <summary>
        /// This function is called when the exit menu item is clicked.
        /// </summary>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// This function sets the show hints option to on, and is called when the show hints menu item is checked.
        /// </summary>
        private void AlwaysShowHints_Checked(object sender, RoutedEventArgs e)
        {
            showHints = true;
        }

        /// <summary>
        /// This sets the hints option to off, and is called when the show hints menu item is unchecked.
        /// </summary>
        private void AlwaysShowHints_Unchecked(object sender, RoutedEventArgs e)
        {
            showHints = false;
        }

        /// <summary>
        /// This function sets the show errors option to on and is called when the show errors menu option is checked.
        /// </summary>
        private void ShowErrors_Checked(object sender, RoutedEventArgs e)
        {
            showErrors = true;
        }

        /// <summary>
        /// This function sets the show errors option to off and is called when the show errors menu option is unchecked.
        /// </summary>
        private void ShowErrors_Unchecked(object sender, RoutedEventArgs e)
        {
            showErrors = false;
        }

        /// <summary>
        /// This function is called when the save game menu item is clicked.
        /// </summary>
        private void SaveGame_Click(object sender, RoutedEventArgs e)
        {
            this.SaveFile();
        }

        /// <summary>
        /// This function is called when the save game unsolved menu item is clicked.
        /// </summary>
        private void SaveGameUnsolved_Click(object sender, RoutedEventArgs e)
        {
            this.SaveFile(true);
        }

        private void GridElement_GotFocus(object sender, RoutedEventArgs e)
        {
            //((TextBox)sender).Text = "3";
            ((TextBox)sender).Style = (Style)(this.Resources["GridElementFocused"]);
        }

        private void GridElement_LostFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Style = (Style)(this.Resources["GridElement"]);
        }


    }
}