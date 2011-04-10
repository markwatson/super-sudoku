using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperSudoku
{
    public class PuzzleGrid : ICloneable
    {
        private int[,] grid = new int[9, 9]; //Uses 9x9 intArray to store values of puzzle
        private List<int> indexRange = new List<int>(Enumerable.Range(0, 8)); //possible indices

        public int InitSetCell(int rowA, int columnB, int value)
        {
            int success = 0;
            bool validIndex = false;
            bool validNewVal = false;
            List<int> valueRange = new List<int>(Enumerable.Range(-9, 9));

            if ((indexRange.Contains(rowA)) && (indexRange.Contains(columnB)))
                validIndex = true;       //confirm that valid grid location is used
            else
                validIndex = false;
            if (valueRange.Contains(value))
                validNewVal = true;           //confirm new value is in range -9..9
            else
                validNewVal = false;
            if (validIndex && validNewVal)
            {
                grid[rowA, columnB] = value;
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
            List<int> valueRange = new List<int>(Enumerable.Range(0, 9));

            if ((indexRange.Contains(rowA)) && (indexRange.Contains(columnB)))
                validIndex = true;       //confirm that valid grid location is used
            else
                validIndex = false;
            if (valueRange.Contains(value))
                validNewVal = true;            //confirm new value is in range 0..9
            else
                validNewVal = false;
            if (grid[rowA, columnB] >= 0)
                canReplace = true;       //confirm value in location is replaceable
            else
                canReplace = false;
            if (validIndex && validNewVal && canReplace)
            {
                grid[rowA, columnB] = value;
                success = 1;
            }
            else
                success = 0;
            return (success);
        }

        public int GetCell(int rowA, int columnB)
        {                                   //return cell value for comparisons etc
            if ((indexRange.Contains(rowA)) && (indexRange.Contains(columnB)))
                return (grid[rowA, columnB]);
            else
                return (0);
        }

        public object Clone()
        {                           //enable cloning for safe copying of the object
            PuzzleGrid p = new PuzzleGrid();
            p.grid = this.grid;
            return p;
        }
    }
}