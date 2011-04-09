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

        /// <summary>
        /// The grid GUI elements are labeled by grid section and grid element. This function
        /// gets items to the left of a grid element.
        /// 
        /// Elements are in the form "_1x1" where the first number is the grid and the second is the element.
        /// Returns itself on fail.
        /// 
        /// These algorithms follow a very computer sciency way to approach
        /// the problem: screw math, just brute force it.
        /// </summary>
        private string GetLeftGridItem(string element)
        {
            string ret = element;

            if (element.IndexOf('x') != -1 && element.IndexOf('_') == 0)
            {
                // first chop off the leading underscore and split by x, then put into ints
                string[] elements = element.Substring(1).Split('x');
                int grid = Convert.ToInt32(elements[0]);
                int elem = Convert.ToInt32(elements[1]);

                // if we're on the left side of the board
                if (grid == 1 || grid == 4 || grid == 7)
                {
                    // and if we're not on the left side of the 9x9 grids
                    if (!(elem == 1 || elem == 4 || elem == 7))
                    {
                        // subtract one from the element to move left
                        elem--;
                    }
                }
                // otherwise if we're in the center or right column of the board
                else
                {
                    // and if we're not on the left side of the 9x9 grids
                    if (!(elem == 1 || elem == 4 || elem == 7))
                    {
                        // subtract one from the element to move left
                        elem--;
                    }
                    // otherwise we are on the left side
                    else
                    {
                        // select the far right box on the column to the left
                        elem += 2;
                        grid--;
                    }
                }

                ret = String.Format("_{0:d}x{1:d}", grid, elem);
            }

            return ret;
        }

        /// <summary>
        /// This function is similar to the GetLeftGridItem function, but gets the item to the right.
        /// </summary>
        private string GetRightGridItem(string element)
        {
            string ret = element;

            if (element.IndexOf('x') != -1 && element.IndexOf('_') == 0)
            {
                // first chop off the leading underscore and split by x, then put into ints
                string[] elements = element.Substring(1).Split('x');
                int grid = Convert.ToInt32(elements[0]);
                int elem = Convert.ToInt32(elements[1]);

                // if we're on the right side of the board
                if (grid == 3 || grid == 6 || grid == 9)
                {
                    // and if we're not on the right side of the 9x9 grids
                    if (!(elem == 3 || elem == 6 || elem == 9))
                    {
                        // subtract one from the element to move left
                        elem++;
                    }
                }
                // otherwise if we're in the center or left column of the board
                else
                {
                    // and if we're not on the right side of the 9x9 grids
                    if (!(elem == 3 || elem == 6 || elem == 9))
                    {
                        // subtract one from the element to move right
                        elem++;
                    }
                    // otherwise we are on the right side
                    else
                    {
                        // select the far left box on the column to the right
                        elem -= 2;
                        grid++;
                    }
                }

                ret = String.Format("_{0:d}x{1:d}", grid, elem);
            }

            return ret;
        }

        /// <summary>
        /// This function is similar to the GetLeftGridItem function, but gets the item above.
        /// </summary>
        private string GetAboveGridItem(string element)
        {
            string ret = element;

            if (element.IndexOf('x') != -1 && element.IndexOf('_') == 0)
            {
                // first chop off the leading underscore and split by x, then put into ints
                string[] elements = element.Substring(1).Split('x');
                int grid = Convert.ToInt32(elements[0]);
                int elem = Convert.ToInt32(elements[1]);

                // if we're on the top of the board
                if (grid == 1 || grid == 2 || grid == 3)
                {
                    // and if we're not on the very top of the 9x9 grids
                    if (!(elem == 1 || elem == 2 || elem == 3))
                    {
                        // subtract 3 from the element to move up
                        elem -= 3;
                    }
                }
                // otherwise if we're in the center or bottom column of the board
                else
                {
                    // and if we're not on the top of the 9x9 grids
                    if (!(elem == 1 || elem == 2 || elem == 3))
                    {
                        // subtract three from the element to move up
                        elem -= 3;
                    }
                    // otherwise we are on the top
                    else
                    {
                        // select the bottom box on the column above
                        elem += 6;
                        grid -= 3;
                    }
                }

                ret = String.Format("_{0:d}x{1:d}", grid, elem);
            }

            return ret;
        }

        /// <summary>
        /// This function is similar to the GetLeftGridItem function, but gets the item below.
        /// </summary>
        private string GetBelowGridItem(string element)
        {
            string ret = element;

            if (element.IndexOf('x') != -1 && element.IndexOf('_') == 0)
            {
                // first chop off the leading underscore and split by x, then put into ints
                string[] elements = element.Substring(1).Split('x');
                int grid = Convert.ToInt32(elements[0]);
                int elem = Convert.ToInt32(elements[1]);

                // if we're on the bottom of the board
                if (grid == 7 || grid == 8 || grid == 9)
                {
                    // and if we're not on the very bottom of the 9x9 grids
                    if (!(elem == 7 || elem == 8 || elem == 9))
                    {
                        // add 3 to the element to move down
                        elem += 3;
                    }
                }
                // otherwise if we're in the center or top column of the board
                else
                {
                    // and if we're not on the bottom of the 9x9 grids
                    if (!(elem == 7 || elem == 8 || elem == 9))
                    {
                        // add three from the element to move down
                        elem += 3;
                    }
                    // otherwise we are on the bottom
                    else
                    {
                        // select the top box on the column below
                        elem -= 6;
                        grid += 3;
                    }
                }

                ret = String.Format("_{0:d}x{1:d}", grid, elem);
            }

            return ret;
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

        private void GridElement_KeyDown(object sender, KeyEventArgs e)
        {
            string name = ((TextBox)sender).Name;

            switch (e.Key)
            {
                case Key.D1:
                    ((TextBox)sender).Text = "1";
                    break;
                case Key.D2:
                    ((TextBox)sender).Text = "2";
                    break;
                case Key.D3:
                    ((TextBox)sender).Text = "3";
                    break;
                case Key.D4:
                    ((TextBox)sender).Text = "4";
                    break;
                case Key.D5:
                    ((TextBox)sender).Text = "5";
                    break;
                case Key.D6:
                    ((TextBox)sender).Text = "6";
                    break;
                case Key.D7:
                    ((TextBox)sender).Text = "7";
                    break;
                case Key.D8:
                    ((TextBox)sender).Text = "8";
                    break;
                case Key.D9:
                    ((TextBox)sender).Text = "9";
                    break;
                case Key.Delete:
                case Key.Back:
                case Key.D0:
                case Key.Space:
                    ((TextBox)sender).Text = "";
                    break;
                case Key.Left:
                    // Calc left item and set it to be focused
                    ((TextBox)FindName(GetLeftGridItem(name))).Focus();
                    break;
                case Key.Right:
                    ((TextBox)FindName(GetRightGridItem(name))).Focus();
                    break;
                case Key.Up:
                    ((TextBox)FindName(GetAboveGridItem(name))).Focus();
                    break;
                case Key.Down:
                    ((TextBox)FindName(GetBelowGridItem(name))).Focus();
                    break;
            }
        }

        private void GridElement_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // We need to send these key presses down the line, otherwise
            // they're blocked by this method.
            GridElement_KeyDown(sender, e);
        }
    }
}