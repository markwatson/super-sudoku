using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSudoku
{
    public class PossibleValues
    {
        int[] possValues; //Holds all the possible values for a cell
        int currentIndex; //Holds currently open index in array
        bool[] hasBeenTried; //Whether or not a given value has been tried.
                            //Index of bool correlates with index of int in possValues
        Random rng = new Random();

        public PossibleValues()
        {
            currentIndex = 0;
            possValues = new int[9];
            hasBeenTried = new bool[9];
            int warmup; //for warming up RNG
            for(int i = 0; i < 50000; i++)//warm up RNG
            {
                warmup = rng.Next();
            }
            for (int i = 0; i < 9; i++)
            {
                possValues[i] = 0;
                hasBeenTried[i] = false;
            }
        }
        /// <summary>
        /// Add takes in a value that is a possiblity for a grid location,
        /// and adds that value (if it does not exist in the array already)
        /// to the array.
        /// </summary>
        /// <param name="value"></param>
        public void Add(int value)
        {
            bool exists = false; //tests if value already entered
            for (int i = 0; i < 9; i++)
            {
                if (value == possValues[i])
                {
                    exists = true;
                }
            }
            if (exists == false) //if not already in there
            {
                possValues[currentIndex] = value;
                currentIndex++;      
            }
        }
        public int TryNumber()
        {
            int ret = 0;
            int i = 1;
            while (possValues[i] == 0 || hasBeenTried[i] == true) //if invalid value or has been tried
            {
                i = rng.Next(0, currentIndex - 1); //keep trying to generate valid value
            }
            ret = possValues[i];
            return ret;
        }
        /// <summary>
        /// Removes a value when it's been definitively set
        /// </summary>
 /*       public void RemovePossible(int value)
        {
            
            int[] temp = new int[9];    //create temp array
            int tempIndex = 0;
            for (int i = 0; i < 9; i++)
            {
                if (possValues[i] != value)
                {
                    temp[tempIndex] = possValues[i];
                    tempIndex++;
                }
            }
            possValues = temp;
        }*/
        public int Count //returns how many elements in array
        {
            get
            {
                return currentIndex;  //changes from 0-indexing to natural counting
            }
        }
    }
}
