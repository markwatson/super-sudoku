using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSudoku
{
    class PuzzleSolver
    {
        public PuzzleGrid SolutionGrid;
        int[] gridRow;
        bool[] list;
        PuzzleGrid[] final;
        int numSolns;
        bool stoplooking;
        

        public PuzzleSolver()
        {
            list = new bool[10];
            gridRow = new int[9];
            final = new PuzzleGrid[2];
        }
        private bool IsSolved(PuzzleGrid grid)
        {
     //       PuzzleGrid grid = new PuzzleGrid();
       //     grid = (PuzzleGrid)g.Clone();
            bool result = true;
            int r, c;
            r = 0;
            while (result == true && r < 9)
            {
                c = 0;
                while (result == true && c < 9)
                {
                    result = (result && grid.GetCell(r, c) != 0);
                    c++;
                }
                r++;
            }
            return result;
        }

        private int FirstTrue(bool[] list)
        {
            int i = 1;
            int result = 0;
            while (result == 0 && i < 10)
            {
                if (list[i] == true)
                {
                    result = i;
                }
                i++;
            }
            return result;
        }
        private int PickOneTrue(bool[] list)
        {
            int i;
            int result = 0;
            Random r = new Random();
            if (FirstTrue(list) != 0)
            {
                i = r.Next(1, 9); //get value 0 - 8
                while (result == 0)
                {
                    if (list[i] == true)
                    {
                        result = i;
                    }
                    else
                    {
                        i++;
                        if (i > 9)
                        {
                            i = 1;
                        }
                    }
                }
            }
            else
            {
                result = 0;
            }
            return result;
        }
        private bool IsInRow(PuzzleGrid grid, int row, int value)
        {
   //         PuzzleGrid grid = new PuzzleGrid();
     //       grid = (PuzzleGrid)g.Clone();
            bool result = false;
            for (int i = 0; i < 9; i++)
            {
                result = result || (grid.GetCell(row, i) == value);
            }
            return result;
        }
        private bool IsInCol(PuzzleGrid grid, int col, int value)
        {
   //         PuzzleGrid grid = new PuzzleGrid();
     //       grid = (PuzzleGrid)g.Clone();
            bool result = false;
            for (int i = 0; i < 9; i++)
            {
                result = result || (grid.GetCell(i, col) == value);
            }
            return result;
        }
        private int GroupNum(int rc)
        {
            //return 0 for rc = 0..2; 1 for rc = 3..5; 2 for rc = 6..9
            int result;
            result = (int)(rc / 3); //Truncate division to whole number value
            return result;
        }

        private bool IsIn3X3(PuzzleGrid g, int row, int col, int value)
        {
   //         PuzzleGrid grid = new PuzzleGrid();
     //       grid = (PuzzleGrid)g.Clone();
            int rLow;
            int cLow;
            rLow = 3 * GroupNum(row);
            cLow = 3 * GroupNum(col);
            bool result = false;
            for (int i = rLow; i < rLow + 3; i++)
            {
                for (int j = cLow; j < cLow + 3; j++)
                {
                    if (g.GetCell(i, j) == value)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
        private bool IsPossible(PuzzleGrid g, int row, int col, int value)
        {//Return true if value can go into [row, col] now
            bool result;
            result = (!IsInRow(g, row, value) && ! IsInCol(g, col, value) &&
                !IsIn3X3(g, row, col, value));
            return result;
        }
        private int ListPossible(int row, int col, PuzzleGrid g, bool[] list)
        {//return count of possible values, update list to indicate true in each avail slot
            int count = 0;
            for (int i = 1; i < 10; i++)
            {
                if (IsPossible(g, row, col, i) == true)
                {
                    list[i] = true;
                    count++;
                }
                else
                {
                    list[i] = false;
                }
            }
            return count;
        }
        private void FillSingleChoices(PuzzleGrid grid)
        {
  //          PuzzleGrid grid = new PuzzleGrid();
    //        grid = (PuzzleGrid)g.Clone();
            bool anyChanges = false;
            bool[] possList = new bool[10];
            list.CopyTo(possList, 0);
            int numChoices;
            do
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (grid.GetCell(i, j) == 0)
                        {
                            numChoices = ListPossible(i, j, grid, possList);
                            if (numChoices == 1)
                            {
                                grid.UserSetCell(i, j, FirstTrue(possList));
                                anyChanges = (grid.GetCell(i, j) == 0);
                            }
                        }
                    }
                }
            } while(anyChanges == false && !IsSolved(grid));

        }

        private bool FindFewestChoices(PuzzleGrid grid, out int r, out int c, ref bool[] list, out int numChoices)
        {
    //        PuzzleGrid grid = new PuzzleGrid();
      //      grid = (PuzzleGrid)g.Clone();
            bool[] minList = new bool[10];
            int numCh, minR, minC, minChoice, i, j;
            bool bad, result;
            minChoice = 10;
            minR = 0;
            minC = 0;
            for (i = 1; i < 10; i++)
            {
                minList[i] = false;
            }
            bad = false;
            i = 0;
            while (!bad && i < 9)
            {
                j = 0;
                while (!bad && j < 9)
                {
                    if (grid.GetCell(i, j) == 0)
                    {
                        numCh = ListPossible(i, j, grid, list);
                        if (numCh == 0)
                        {
                            bad = true;
                        }
                        else
                        {
                            if (numCh < minChoice)
                            {
                                minChoice = numCh;
                                list.CopyTo(minList, 0);
                                minR = i;
                                minC = j;
                            }
                        }
                    }
                    j++;
                }
                i++;
            }
            if (bad || minChoice == 10)
            {
                result = false;
                r = 0;
                c = 0;
                numChoices = 0;
            }
            else
            {
                result = true;
                r = minR;
                c = minC;
                numChoices = minChoice;
                minList.CopyTo(list, 0);
            }
            return result;
        }
        private void RandomizeRow(ref int[] gridr)
        {
            int nVal;
            Random r = new Random();
            for (int i = 0; i < 9; i++)
            {
                list[i] = false;
            }
            for (int i = 0; i < 9; i++)
            {
                nVal = r.Next(0, 8);
                if (list[nVal] == true)
                {
                    do
                    {
                        nVal++;
                        if (nVal > 9)
                        {
                            nVal = 1;
                        }
                    }while(list[nVal] == false);
                    list[nVal] = true;
                    gridr[i] = nVal;
                }
            }
        }
        public bool SolveGrid(PuzzleGrid g, bool checkUnique)
        {
            PuzzleGrid grid = new PuzzleGrid();
            grid = (PuzzleGrid)g.Clone();
            int i, choice, r, c, numChoices;
            bool done, got_one, solved, result;
            got_one = false;
            FillSingleChoices(grid);
            if (IsSolved(grid))
            {
                if (numSolns > 0)
                {
                    stoplooking = true;
                    result = false;
                }
                else
                {
                    numSolns++;
                    final[numSolns] = (PuzzleGrid)g.Clone();
                    result = true;
                    SolutionGrid = grid;
                }
            }
            else
            {
                if (!FindFewestChoices(grid, out r, out c, ref list, out numChoices))
                {
                    result = false;
                }
                else
                {
                    #region
                    i = 1;
                    done = false;
                    got_one = false;
                    while (!done && i <= numChoices)
                    {
                        choice = PickOneTrue(list);
                        list[choice] = false;
                        grid.UserSetCell(r, c, choice);
                        solved = (SolveGrid(g, checkUnique));
                        if (stoplooking == true)
                        {
                            done = true;
                            got_one = true;
                        }
                        else
                        {
                            got_one = (got_one || solved);
                            if (!checkUnique)
                            {
                                done = got_one;
                            }
                        }
                        i++;
                    }
                    #endregion
                    result = got_one;
                }
            }
            return result;
        }
    }
}
