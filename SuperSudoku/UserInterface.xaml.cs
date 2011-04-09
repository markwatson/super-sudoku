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
	    private readonly PuzzleSolver puzzleSolver;
	    private readonly PuzzleGenerator puzzleGenerator;

        // game grid size
	    private const int GameBoardCols = 9;
        private const int GameBoardRows = 9;

        // except for game board, which is just an array indexed by row, col.
        private readonly int[,] gameBoard;

        // State properties are defined here
        private bool showHintsOn;
	    private bool showErrorsOn;

        // helper enum for moving around the grid
        private enum Direction
        {
            Up, Down, Left, Right
        }

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
            puzzleSolver = new PuzzleSolver();
            puzzleGenerator = new PuzzleGenerator(puzzleSolver);

            gameBoard = new int[GameBoardRows, GameBoardCols];

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
        /// The grid GUI elements are labeled by row and column.
        /// 
        /// Elements are in the form "_1x1" where the first number is the row and the second is the column.
        /// Returns itself on fail.
        /// </summary>
        private static string GetMovedGridItem(string element, Direction direction)
        {
            var ret = element;

            var rowCol = GetRowColumnFromTextboxName(element);

            if (rowCol != null)
            {
                var row = rowCol.Item1;
                var col = rowCol.Item2;

                switch (direction)
                {
                    case Direction.Left:
                        // get the new column if we're not on the left edge
                        if (col != 1)
                        {
                            col -= 1;
                        }
                        break;
                    case Direction.Right:
                        // if we're not on the right edge
                        if (col != GameBoardCols)
                        {
                            col += 1;
                        }
                        break;
                    case Direction.Up:
                        // not on the top
                        if (row != 1)
                        {
                            row -= 1;
                        }
                        break;
                    case Direction.Down:
                        // not on bottom
                        if (row != GameBoardRows)
                        {
                            row += 1;
                        }
                        break;
                }

                ret = String.Format("_{0:d}x{1:d}", row, col);
            }

            return ret;
        }

        /// <summary>
        /// This takes a textbox name and returns row and column integers.
        /// </summary>
        /// <param name="textBoxName">The name of the textbox in the format _AxB, where
        /// A is the row and B is the column.</param>
        /// <returns>
        /// A a tuple of two ints in the form of: (Row, Column) if the textBoxName is valid.
        /// Returns null if the textBoxName is invalid.</returns>
        private static Tuple<int,int> GetRowColumnFromTextboxName(string textBoxName)
        {
            Tuple<int,int> ret;

            if (textBoxName.IndexOf('x') != -1 && textBoxName.IndexOf('_') == 0)
            {
                // first chop off the leading underscore and split by x, then put into ints
                var temp = textBoxName.Substring(1).Split('x');
                var row = Convert.ToInt32(temp[0]);
                var col = Convert.ToInt32(temp[1]);

                ret = Tuple.Create(row, col);
            }
            else
            {
                ret = null;
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
            showHintsOn = true;
        }

        /// <summary>
        /// This sets the hints option to off, and is called when the show hints menu item is unchecked.
        /// </summary>
        private void AlwaysShowHintsUnchecked(object sender, RoutedEventArgs e)
        {
            showHintsOn = false;
        }

        /// <summary>
        /// This function sets the show errors option to on and is called when the show errors menu option is checked.
        /// </summary>
        private void ShowErrorsChecked(object sender, RoutedEventArgs e)
        {
            showErrorsOn = true;
        }

        /// <summary>
        /// This function sets the show errors option to off and is called when the show errors menu option is unchecked.
        /// </summary>
        private void ShowErrorsUnchecked(object sender, RoutedEventArgs e)
        {
            showErrorsOn = false;
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

            // get the row/column of the text box
            var rowCol = GetRowColumnFromTextboxName(name);

            switch (e.Key)
            {
                    // When we're setting the boxes we also keep the game grid up to date.
                case Key.D1:
                    ((TextBox)sender).Text = "1";
                    gameBoard[rowCol.Item1, rowCol.Item2] = 1;
                    break;
                case Key.D2:
                    ((TextBox)sender).Text = "2";
                    gameBoard[rowCol.Item1, rowCol.Item2] = 2;
                    break;
                case Key.D3:
                    ((TextBox)sender).Text = "3";
                    gameBoard[rowCol.Item1, rowCol.Item2] = 3;
                    break;
                case Key.D4:
                    ((TextBox)sender).Text = "4";
                    gameBoard[rowCol.Item1, rowCol.Item2] = 4;
                    break;
                case Key.D5:
                    ((TextBox)sender).Text = "5";
                    gameBoard[rowCol.Item1, rowCol.Item2] = 5;
                    break;
                case Key.D6:
                    ((TextBox)sender).Text = "6";
                    gameBoard[rowCol.Item1, rowCol.Item2] = 6;
                    break;
                case Key.D7:
                    ((TextBox)sender).Text = "7";
                    gameBoard[rowCol.Item1, rowCol.Item2] = 7;
                    break;
                case Key.D8:
                    ((TextBox)sender).Text = "8";
                    gameBoard[rowCol.Item1, rowCol.Item2] = 8;
                    break;
                case Key.D9:
                    ((TextBox)sender).Text = "9";
                    gameBoard[rowCol.Item1, rowCol.Item2] = 9;
                    break;
                case Key.Delete:
                case Key.Back:
                case Key.D0:
                case Key.Space:
                    ((TextBox)sender).Text = "";
                    break;
                case Key.Left:
                    // Calc left item and set it to be focused
                    var left = (TextBox) FindName(GetMovedGridItem(name, Direction.Left));
                    if (left != null)
                    {
                        left.Focus();
                    }
                    break;
                case Key.Right:
                    var right = (TextBox) FindName(GetMovedGridItem(name, Direction.Right));
                    if (right != null)
                    {
                        right.Focus();
                    }
                    break;
                case Key.Up:
                    var above = (TextBox) FindName(GetMovedGridItem(name, Direction.Up));
                    if (above != null)
                    {
                        above.Focus();
                    }
                    break;
                case Key.Down:
                    var below = (TextBox) FindName(GetMovedGridItem(name, Direction.Down));
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