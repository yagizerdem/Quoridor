using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.UI.Models
{
    internal class BoardModel
    {
        public string Board = null;
        public BoardModel()
        {
            this.Board = """
                -------------------
                |. . . . . . . . .|
                |                 |
                |. . . . . . . . .|
                |                 |
                |. . . . . . . . .|
                |                 |
                |. . . . . . . . .|
                |                 |
                |. . . . . . . . .|
                |                 |
                |. . . . . . . . .|
                |                 |
                |. . . . . . . . .|
                |                 |
                |. . . . . . . . .|
                |                 |
                |. . . . . . . . .|
                -------------------
                """.Trim();
        }
        
        public string GetBoard()
        {
            string template = this.Board;
            // modify row to fit ui
            int blueRow = SD.PlayerBlue.row;
            int blueCol = SD.PlayerBlue.col;
            int redRow = SD.PlayerRed.row;
            int redCol = SD.PlayerRed.col;

            // Split the template into lines
            string[] lines = template.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            // Update the row for PlayerBlue
            char[] blueRowChars = lines[blueRow].ToCharArray();
            blueRowChars[blueCol] = SD.PlayerBlue.symbol;
            lines[blueRow] = new string(blueRowChars); // Update the line in the array

            // Update the row for PlayerRed
            char[] redRowChars = lines[redRow].ToCharArray();
            redRowChars[redCol] = SD.PlayerRed.symbol;
            lines[redRow] = new string(redRowChars); // Update the line in the array


            // add walls
            IEnumerable<string> wallPositions = SD.walls.Iterator();
            foreach(string wall in wallPositions)
            {
                int[] row_col = wall.Split("-").Select(int.Parse).ToArray();
                char[] rowchars = lines[row_col[0]].ToCharArray();
                rowchars[row_col[1]] = '#';
                lines[row_col[0]] = new string(rowchars); // Update the line in the array
            }


            // Join the lines back into a single string
            template = string.Join("\n", lines);

            // Return or use the updated template
            return template;
        }
    }
}
