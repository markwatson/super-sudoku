﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSudoku
{
    class FileHandler
    {
        /// <summary>
        /// This function saves a full game.
        /// </summary>
        public bool SaveFile(PuzzleGrid gameBoard, String fileName)
        {
            return true;
        }

        /// <summary>
        /// This function saves just the part of the game file without any use edits applied.
        /// </summary>
        public bool SaveFileUnsolved(PuzzleGrid gameBoard, String fileName)
        {
            return true;
        }

        /// <summary>
        /// This function opens a game.
        /// </summary>
        public PuzzleGrid OpenFile(String fileName)
        {
            PuzzleGrid grid = null;

            return grid;
        }
    }
}
