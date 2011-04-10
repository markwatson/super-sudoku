using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSudoku
{
    class PuzzleSolver
    {
        /// <summary>
        /// Constructs a puzzle solver class.
        /// </summary>

        List<int>[,] possValues;  //Matrix of lists holding possible values
        PuzzleGrid puzzle;


        public PuzzleSolver(PuzzleGrid puzz)
        {
            puzzle = puzz;
        }

        /// <summary>
        /// Populates possValues with the possible values for each empty 
        /// location in the game board. Takes the row and column of a given
        /// empty location as parameters.
        /// 
        /// r == row; c == column;
        /// </summary>
        private void PopulatePossibleValues(int r, int c)
        {
            int[] foundValues = new int[10]; //holds values found, 0 == empty
            int cLoc; //location in 3x3 subgrid column
            int rLoc; //location in 3x3 subgrid column
            cLoc = c % 3; //0 == left, 1 == center, 2 == right
            rLoc = r % 3; //0 == top, 1 == center, 2 == bottom

            #region SPECIAL CASES: CORNERS
            ///Deal with special cases: [r, c] is a corner, either of 9x9
            ///grid, or 3x3 subgrid
            if (r == 0 && c == 0 || r == 0 && cLoc == 0)//Top-left
            {
                ///Check (r, c) through (r + 2, c + 2)
                for (int i = r; i < r + 3; i++)
                { 
                    for (int j = c; j < c + 3; j++)
                    {
                        foundValues[puzzle.GetCell(i, j)] = puzzle.GetCell(i, j);
                    }
                }
            }

            else if (r == 8 && c == 0 || rLoc == 2 && cLoc == 0) //Bottom-left
            {
                ///Check (r, c) through (r - 2, c + 2)
                for (int i = r; i > r - 3; i--)
                { 
                    for (int j = c; j < c + 3; j++)
                    {
                        foundValues[puzzle.GetCell(i, j)] = puzzle.GetCell(i, j);
                    }
                }
            }

            else if (r == 0 && c == 8) //Top-right corner
            {
                ///Check (r, c) through (r + 2, c + 2)
                for (int i = r; i < r + 3; i++)
                { 
                    for (int j = c; j > c - 3; j--)
                    {
                        foundValues[puzzle.GetCell(i, j)] = puzzle.GetCell(i, j);
                    }
                }
            }

            else if (r == 8 && c == 8) //bottom right corner
            {
                ///Check (r, c) through (r - 2, c - 2)
                for (int i = r; i > r - 3; i--)
                {
                    for (int j = c; j > c - 3; j--)
                    {
                        foundValues[puzzle.GetCell(i, j)] = puzzle.GetCell(i, j);
                    }
                }
            }
            #endregion

            #region SPECIAL CASES: SIDES (BUT NOT CORNERS)
            //'else' is carry through from SPECIAL CASES: CORNERS
            else if (r == 0) //top side
            {
                

            }

            #endregion
        }


    }
}
