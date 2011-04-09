using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSudoku
{
    class PuzzleGenerator
    {
        private readonly PuzzleSolver puzzleSolver;

        /// <summary>
        /// This constructs a puzzle generator class.
        /// </summary>
        /// <param name="puzzleSolverIn">The instance of the puzzle solver from UserInterface.</param>
        public PuzzleGenerator(PuzzleSolver puzzleSolverIn)
        {
            puzzleSolver = puzzleSolverIn;
        }
    }
}
