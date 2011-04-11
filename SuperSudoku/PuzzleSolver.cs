﻿using System;
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
        int numSolns;
        bool foundSoln;
        bool isSolved = false;
        bool stopLooking = false;
        PuzzleGrid solutionGrid;
        Random rng = new Random();


        public PuzzleSolver()
        {
            
        }

        /// <summary>
        /// Populates possValues with the possible values for each empty 
        /// location in the game board. Takes the row and column of a given
        /// empty location as parameters.
        /// 
        /// r == row; c == column;
        /// </summary>
        private void PopulatePossibleValues(int r, int c, PuzzleGrid puzzle)
        {
            int[] foundValues = new int[10]; //holds values found, 0 == empty
            int cLoc; //location in 3x3 subgrid column
            int rLoc; //location in 3x3 subgrid column
            possValues = new List<int>[9,9];
            cLoc = c % 3; //0 == left, 1 == center, 2 == right
            rLoc = r % 3; //0 == top, 1 == center, 2 == bottom

            #region CHECK 3X3 GRID VALUES

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

            else if (r == 0 && c == 8 || rLoc == 0 && cLoc == 2) //Top-right
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

            else if (r == 8 && c == 8 || rLoc == 2 && cLoc == 2) //bottom right
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
                for (int i = r; i < r + 3; i++)
                {
                    for (int j = c - 1; j < c + 2; j++)
                    {
                        foundValues[puzzle.GetCell(i, j)] = puzzle.GetCell(i, j);
                    }
                }
            }

            else if (r == 8) //bottom side
            {
                for (int i = r; i > r - 3; i--)
                {
                    for (int j = c - 1; j < c + 2; j++)
                    {
                        foundValues[puzzle.GetCell(i, j)] = puzzle.GetCell(i, j);
                    }
                }
            }

            else if (c == 0) //left side
            {
                for (int i = r - 1; i < r + 2; i++)
                {
                    for (int j = c; j < c + 3; j++)
                    {
                        foundValues[puzzle.GetCell(i, j)] = puzzle.GetCell(i, j);
                    }
                }
            }

            else if (c == 8) //right side
            {
                for (int i = r - 1; i < r + 2; i++)
                {
                    for (int j = c; j > c - 3; j--)
                    {
                        foundValues[puzzle.GetCell(i, j)] = puzzle.GetCell(i, j);
                    }
                }
            }

            #endregion

            else //center of 3x3 grid
            {
                for (int i = r - 1; i < r + 2; i++)
                {
                    for (int j = c; j > c - 3; j--)
                    {
                        foundValues[puzzle.GetCell(i, j)] = puzzle.GetCell(i, j);
                    }
                }
            }

            #endregion

            ///Check row for given values
            for (int i = 0; i < 9; i++)
            {
                foundValues[puzzle.GetCell(r, i)] = puzzle.GetCell(r, i);
            }

            ///Check column for given values
            for (int i = 0; i < 9; i++)
            {
                foundValues[puzzle.GetCell(i, c)] = puzzle.GetCell(i, c);
            }

            //Populate lists with values
            for (int i = 1; i < 9; i++)
            {
                ///Iterate through foundValues, where the value held in 
                ///foundValues[i] will indicate that value has been found.
                ///If array slot is empty, value was not found and is 
                ///possible value for the empty slot. Because the found value
                ///array utilizes natural indexing, i == the value 1 - 9. If
                ///[i] is null, the possible value for the empty cell is i.
                if (foundValues[i] == null)
                {
                    possValues[r, c].Add(i);
                }
            }
        }

        private PuzzleGrid FillSingleChoices(PuzzleGrid puzzle)
        {
            bool replacementMade = false;
            int value;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (possValues[i,j].Count == 1) //If only 1 possible value
                    {
                        value = possValues[i, j].Max();
                        puzzle.UserSetCell(i, j, value);
                        replacementMade = true;
                    }
                }
            }

            if (replacementMade == true)
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        PopulatePossibleValues(i, j, puzzle); //Repopulate new values
                    }
                }
                ///Check for empty locations which now have only 1 available
                ///slot. I.e. Had two possible values before filling in 
                ///single choices, and after a 1-value slot was set, now only
                ///has 1 choice available.
                puzzle = FillSingleChoices(puzzle);
            }

            return puzzle;
        }

        private bool IsSolved(PuzzleGrid puzzle)
        {
            isSolved = true; //Assume no blank squares
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (puzzle.GetCell(i, j) == 0)
                    {
                        isSolved = false; //Empty cell found, not solved
                    }
                }
            }
            return isSolved;
        }

        private int[] FindFewestChoices(PuzzleGrid puzzle)
        {
            int[] location = new int[2]; //returns [r,c] of fewest choices
            int r = -1; //row of location with fewest choices
            int c = -1; //row of location with fewest choices
            int minChoices = 9; //current lowest number of choices available

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (puzzle.GetCell(i, j) == 0) //If blank cell
                    {
                        if (possValues[i,j].Count < minChoices)
                        {
                            minChoices = possValues[i, j].Count;
                            r = i; //save location
                            c = j;
                        }
                    }
                }
            }
            if (minChoices == 0) //If location has no possible values
            {
                r = -1; //save invalid location
                c = -1;
            }
            location[0] = r;
            location[1] = c;

            return location;
        }

        private int PickOneTrue(int r, int c)
        {
            int possible = possValues[r, c].Count;
            int choice = rng.Next(0, possible);
            return choice;
        }

        public bool SolveGrid(PuzzleGrid puzzle)
        {
            #region Populate Possible values for all empty (0) locations
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (puzzle.GetCell(i, j) == 0)
                    {
                        PopulatePossibleValues(i, j, puzzle);
                    }
                }
            }
            #endregion

            FillSingleChoices(puzzle);

            if (IsSolved(puzzle) == true)
            {
                if (numSolns > 0)
                {
                    stopLooking = true;
                    foundSoln = false;
                }
                else
                {
                    numSolns++;
                    solutionGrid = puzzle;
                    foundSoln = true;
                }
            }
            else
            {
                int[] location = FindFewestChoices(puzzle);
                int r = location[0]; //set row of fewest choices
                int c = location[1]; //set column of fewest choices
                if (r == -1 || c == -1) //If location is an invalid entry
                {
                    foundSoln = false;
                }
                else
                {
                    int numChoices = possValues[r, c].Count;
                    int i = 1;
                    bool done = false;
                    
                    while (!done && i <= numChoices)
                    {
                        int choice = PickOneTrue(r, c);
                        int value = possValues[r, c].ElementAt(choice);
                        possValues[r, c].RemoveAt(choice); //remove from  poss
                        puzzle.UserSetCell(r, c, value);
                        isSolved = IsSolved(puzzle);

                        if (stopLooking == true)
                        {
                            done = true;
                            foundSoln = false;
                        }
                        else
                        {
                            foundSoln = IsSolved(puzzle);
                            i++;
                        }
                    }
                }
            }

            return foundSoln;
        }


    }
}
