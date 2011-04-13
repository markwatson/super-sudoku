using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperSudoku
{
    public class PuzzleGrid : ICloneable
    {
        /// <summary>
        /// This is the internal grid element.
        /// Reading of the private grid element is allowed. Setting it is not allowed.
        /// </summary>
        public int[,] Grid { get; private set; }
        const int MAX = 9; //Max number in any part of the array

        /// <summary>
        /// possible indices
        /// </summary>
        private List<int> indexRange = new List<int>(Enumerable.Range(0, 9));

        /// <summary>
        /// This constructor creates the grid.
        /// </summary>
        public PuzzleGrid()
        {
            Grid = new int[9,9];
        }

        public int InitSetCell(int rowA, int columnB, int value)
        {
            int success = 0;
            bool validIndex = false;
            bool validNewVal = false;
            // NOTE: Enumerable.Range takes a starting value, and then a number
            // NOTE: of items to include, not a starting then ending value.
            List<int> valueRange = new List<int>(Enumerable.Range(-9, 19));

            //if ((indexRange.Contains(rowA)) && (indexRange.Contains(columnB)))
            if (rowA >= 0 && rowA < 9 && columnB >= 0 && columnB < 9)
                validIndex = true;       //confirm that valid grid location is used
            else
                validIndex = false;
            if (valueRange.Contains(value))
                validNewVal = true;           //confirm new value is in range -9..9
            else
                validNewVal = false;
            if (validIndex && validNewVal)
            {
                Grid[rowA, columnB] = value;
                success = 1;
            }
            else
                success = 0;
            return (success);
        }


        public int UserSetCell(int rowA, int columnB, int value)
        {
            int success = 0;
            bool validIndex = false;
            bool validNewVal = false;
            bool canReplace = false;
            List<int> valueRange = new List<int>(Enumerable.Range(0, 10));

            //if ((indexRange.Contains(rowA)) && (indexRange.Contains(columnB)))
            if (rowA >= 0 && rowA < 9 && columnB >= 0 && columnB < 9)
                validIndex = true;       //confirm that valid grid location is used
            else
                validIndex = false;
            if (valueRange.Contains(value))
                validNewVal = true;            //confirm new value is in range 0..9
            else
                validNewVal = false;
            if (Grid[rowA, columnB] >= 0)
                canReplace = true;       //confirm value in location is replaceable
            else
                canReplace = false;
            if (validIndex && validNewVal && canReplace)
            {
                Grid[rowA, columnB] = value;
                success = 1;
            }
            else
                success = 0;
            return (success);
        }

        public int GetCell(int rowA, int columnB)
        {                                   //return cell value for comparisons etc
            //if ((indexRange.Contains(rowA)) && (indexRange.Contains(columnB)))
            if (rowA >= 0 && rowA < 9 && columnB >= 0 && columnB < 9)
                return (Grid[rowA, columnB]);
            else
                return (0);
        }

        public object Clone()
        {                           //enable cloning for safe copying of the object
            PuzzleGrid p = new PuzzleGrid();
            p.Grid = this.Grid;
            return p;
        }
    }
}