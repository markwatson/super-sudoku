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

            #region SPECIAL CASES: CORNERS
            ///Deal with special cases: [r, c] is a corner
            if (r == 0 && c == 0)//Top-left corner
            {
                foundValues[puzzle.GetCell(r, c)] = puzzle.GetCell(r, c);
            }

            #endregion
        }


    }
}
