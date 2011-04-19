using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSudoku
{
    public class PossibleValues
    {
        bool[] possible; //Whether or not a given value has been tried.
                            //Index of bool correlates with index of int in possValues

        public PossibleValues()
        {
            possible = new bool[10];
        }
        public bool[] List
        {
            get
            {
                return possible;
            }
            set
            {
                for (int i = 1; i < 10; i++)
                {
                    possible[i] = value[i];
                }
            }
        }
    }
}
