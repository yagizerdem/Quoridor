using Quoridor.Engine.PathFindings;
using Quoridor.Observabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.Engine.Utils
{
    internal class Validator
    {
        public bool ValidatePlayerMovement(int[] from ,  int[] to , HashSet<string> walls )
        {
            int dx = Math.Abs( from[0]  - to[0]);
            int dy = Math.Abs( from[1] - to[1]);


            // check map bounds
            if (to[0]<1 || to[0]>17 || to[1] <1 || to[1] > 17) return false;

            // check step size  and direction 
            if (dy != 0 && dx != 0) return false;
            if (dy == 0 && dx == 0) return false;
            if ((dy != 2 && dx == 0) || (dx != 2 && dy == 0)) return false;

            // check is wall blocking
            string hash = $"{(from[0] + to[0]) / 2}-{(from[1] + to[1]) / 2}";
            if (walls.Contains(hash)) return false;

            return true;

        }
    
        public bool validateWallPlacement(int[,]  row_col, HashSet<string> walls, int[] bluePlayerPositon , int[] redPlayerPosition)
        {
            bool isHorizontal = row_col[0,0] - row_col[1,0] != 0 ? true : false; 
            for (int i = 0; i < row_col.GetLength(0); i++)
            {
                int row = row_col[i, 0];
                int col = row_col[i, 1];
                string hash = $"{row}-{col}";
                // check walls are colliding
                if (walls.Contains(hash))
                {
                    return false;
                }

                // check walls are overlapping with player squares
                if(row % 2 == 1 && col % 2 == 1)
                {
                    return false;
                }

                // check map bounds
                if (row < 1 || row > 17 || col < 1 || col > 17) return false;

                if (isHorizontal && i == 0  && row % 2 == 0) return false;
                if (!isHorizontal && i == 0 && col % 2 == 0) return false;


            }

            // check wall placement block player paths at least 1 path must be open to end point
            int[] from_blue = [SD.PlayerBlue.row, SD.PlayerBlue.col];
            bool hasBluePlayerPath = simulateWallPlacement(row_col, walls, from_blue, 1);
            if (!hasBluePlayerPath) return false;

            int[] from_red = [SD.PlayerRed.row, SD.PlayerRed.col];
            bool hasRedPlayerPath = simulateWallPlacement(row_col, walls, from_red, 17);
            if (!hasRedPlayerPath) return false;

            return true;
        }
    
        public bool simulateWallPlacement(int[,] row_col, HashSet<string> walls , int[] from , int targetRow)
        {

            // add new wall to wall hash set
            for (var i = 0; i < row_col.GetLength(0); i++)
            {
                for (var j = 0; j < row_col.GetLength(1); j++)
                {
                    int row = row_col[i, 0];
                    int col = row_col[i, 1];
                    string hash = $"{row}-{col}";
                    walls.Add(hash);
                }
            }

            Bfs bfs = new Bfs();
            List<Node> path = bfs.GetShortestPath(from , targetRow , walls);
        

            // remove added walls from hash set
            // add new wall to wall hash set
            for (var i = 0; i < row_col.GetLength(0); i++)
            {
                for (var j = 0; j < row_col.GetLength(1); j++)
                {
                    int row = row_col[i, 0];
                    int col = row_col[i, 1];
                    string hash = $"{row}-{col}";
                    walls.Remove(hash);
                }
            }
            
            if (path == null) return false;
            
            return true;
        }
    }
}
