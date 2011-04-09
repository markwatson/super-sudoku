using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace SuperSudoku
{
	/// <summary>
	/// GUI Logic goes in this MainWindow class. It also contains the state of the application.
	/// </summary>
	public partial class UserInterface
	{
        // Game elements are implemented in seperate classes and instantiated to properties of this class here:
        private readonly FileHandler fileHandler;

        // except for game board, which is just an array.
        public int[][] GameBoard;

        // State properties are defined here
        private bool ShowHintsOn;
	    private bool ShowErrorsOn;

        // GUI elements are defined here
        private readonly SaveFileDialog saveGameDialog;

        /// <summary>
        /// This function instantiates all the game objects and sets up the game window.
        /// </summary>
		public UserInterface()
		{
			InitializeComponent();

            // Initialize game elements
            fileHandler = new FileHandler();

			// Initialize the save game dialog
            saveGameDialog = new SaveFileDialog {Filter = "Sudoku Games | *.sud", DefaultExt = ".sud"};
		}

        // The following methods are helper methods
        /// <summary>
        /// This method shows the save file dialog box. It's called by the save game menu items.
        /// </summary>
        private void SaveFile(bool saveUnsolved = false)
        {
            // Show save file dialog box
            var result = saveGameDialog.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string fileName = saveGameDialog.FileName;

                bool success;
                if (!saveUnsolved)
                {
                    success = fileHandler.SaveFile(GameBoard, fileName);
                }
                else
                {
                    success = fileHandler.SaveFileUnsolved(GameBoard, fileName);
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
        private static string GetLeftGridItem(string element)
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
        private static string GetRightGridItem(string element)
        {
            var ret = element;

            if (element.IndexOf('x') != -1 && element.IndexOf('_') == 0)
            {
                // first chop off the leading underscore and split by x, then put into ints
                var elements = element.Substring(1).Split('x');
                var grid = Convert.ToInt32(elements[0]);
                var elem = Convert.ToInt32(elements[1]);

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
        private static string GetAboveGridItem(string element)
        {
            var ret = element;

            if (element.IndexOf('x') != -1 && element.IndexOf('_') == 0)
            {
                // first chop off the leading underscore and split by x, then put into ints
                var elements = element.Substring(1).Split('x');
                var grid = Convert.ToInt32(elements[0]);
                var elem = Convert.ToInt32(elements[1]);

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
        private static string GetBelowGridItem(string element)
        {
            var ret = element;

            if (element.IndexOf('x') != -1 && element.IndexOf('_') == 0)
            {
                // first chop off the leading underscore and split by x, then put into ints
                var elements = element.Substring(1).Split('x');
                var grid = Convert.ToInt32(elements[0]);
                var elem = Convert.ToInt32(elements[1]);

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
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// This function sets the show hints option to on, and is called when the show hints menu item is checked.
        /// </summary>
        private void AlwaysShowHintsChecked(object sender, RoutedEventArgs e)
        {
            ShowHintsOn = true;
        }

        /// <summary>
        /// This sets the hints option to off, and is called when the show hints menu item is unchecked.
        /// </summary>
        private void AlwaysShowHintsUnchecked(object sender, RoutedEventArgs e)
        {
            ShowHintsOn = false;
        }

        /// <summary>
        /// This function sets the show errors option to on and is called when the show errors menu option is checked.
        /// </summary>
        private void ShowErrorsChecked(object sender, RoutedEventArgs e)
        {
            ShowErrorsOn = true;
        }

        /// <summary>
        /// This function sets the show errors option to off and is called when the show errors menu option is unchecked.
        /// </summary>
        private void ShowErrorsUnchecked(object sender, RoutedEventArgs e)
        {
            ShowErrorsOn = false;
        }

        /// <summary>
        /// This function is called when the save game menu item is clicked.
        /// </summary>
        private void SaveGameClick(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        /// <summary>
        /// This function is called when the save game unsolved menu item is clicked.
        /// </summary>
        private void SaveGameUnsolvedClick(object sender, RoutedEventArgs e)
        {
            SaveFile(true);
        }

        /// <summary>
        /// This function is called when a grid element is clicked or gets focused.
        /// </summary>
        private void GridElementGotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Style = (Style)(Resources["GridElementFocused"]);
        }

        /// <summary>
        /// This function is called when a grid element no longer has focus.
        /// </summary>
        private void GridElementLostFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Style = (Style)(Resources["GridElement"]);
        }

        /// <summary>
        /// This function is called when a grid item is selected and a key is pressed. It enforces 
        /// all the rules about how keys can manipulate the game grid.
        /// </summary>
        private void GridElementKeyDown(object sender, KeyEventArgs e)
        {
            var name = ((TextBox)sender).Name;

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
                    var left = (TextBox) FindName(GetLeftGridItem(name));
                    if (left != null)
                    {
                        left.Focus();
                    }
                    break;
                case Key.Right:
                    var right = (TextBox) FindName(GetRightGridItem(name));
                    if (right != null)
                    {
                        right.Focus();
                    }
                    break;
                case Key.Up:
                    var above = (TextBox)FindName(GetAboveGridItem(name));
                    if (above != null)
                    {
                        above.Focus();
                    }
                    break;
                case Key.Down:
                    var below = (TextBox)FindName(GetBelowGridItem(name));
                    if (below != null)
                    {
                        below.Focus();
                    }
                    break;
            }
        }

        /// <summary>
        /// This function is also called when buttons are pressed, but supresses some keys like the spacebar. 
        /// I modified it so that it passes all key signals down to the function above this.
        /// </summary>
        private void GridElementPreviewKeyDown(object sender, KeyEventArgs e)
        {
            // We need to send these key presses down the line, otherwise
            // they're blocked by this method.
            GridElementKeyDown(sender, e);
        }
    }
}