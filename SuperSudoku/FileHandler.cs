using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SuperSudoku
{
    class FileHandler
    {

        /// <summary>
        /// This function saves a game.
        /// </summary>
        public bool SaveFile(PuzzleGrid board, String fileName, bool unsolved=false)
        {
            int i = 0; //mnemonic: row
            int j = 0; //mnemonic: column
            TextWriter writer = new StreamWriter(fileName);
            int cellVal;
            for(i = 0; i < 9; i++)
            {
                string currentLine = "";
	            for(j = 0; j < 9; j++)
	            {	
		            cellVal = board.GetCell(i,j);
		            if(cellVal < 0)
		            {
                        currentLine += Math.Abs(cellVal).ToString();
			            currentLine += "-";
		            }
		            else
		            {
                        if (unsolved)
                        {
                            currentLine += "0";
                        }
                        else
                        {
                            currentLine += cellVal.ToString();
                        }
                        
			            currentLine += "+";
		            }
	            }
	            writer.WriteLine(currentLine);
            }
            writer.Close();
            return(true);
        }

        /// <summary>
        /// This function opens a game.
        /// </summary>
        public PuzzleGrid OpenFile(String fileName)
        {
            PuzzleGrid openedPuzzle = new PuzzleGrid();
		    int i = 0; //mnemonic: row
		    int j = 0; //mnemonic: column
		    int cellVal = 0;
		    bool eOF = false;
            using (StreamReader reader = new StreamReader(fileName))
		    {
			    for(i = 0; i < 9; i++)
			    {
			        string currentLine;
			        if((currentLine = reader.ReadLine()) != null)
				    {
					    for(j = 0; j < 9; j++)
					    {	
						    cellVal = (int) currentLine[2*j]-'0';
						    if(currentLine[(2*j)+1] == '+')
						    {
							    openedPuzzle.InitSetCell(i,j,cellVal);
						    }
						    else if(currentLine[(2*j)+1] == '-')
						    {
							    openedPuzzle.InitSetCell(i,j,-(cellVal));
						    }
					    }
				    }
				    else
				    {
					    eOF = true;
				    }
			    }
		    }			
		    if(eOF)
			    return null;
		    else
                return openedPuzzle;
        }
    }
}
