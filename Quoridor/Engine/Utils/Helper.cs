using Quoridor.Engine.PathFindings;
using Quoridor.Observabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.Engine.Utils
{
    internal class Helper
    {
        private readonly Validator validator;
        private readonly Bfs bfs;
        public Helper()
        {
            this.validator = new Validator();
            
            this.bfs = new Bfs();
        }

        public Player getCurrentPlayer() => SD.info.turn == Enums.Color.Blue ? SD.PlayerBlue : SD.PlayerRed;
        public Player getOpponentPlayer() => SD.info.turn == Enums.Color.Blue ? SD.PlayerRed : SD.PlayerBlue;



        public bool CheckWinner()
        {
            return SD.PlayerBlue.row == 1 || SD.PlayerRed.row == 17;
        }

        public List<int[,]> getAvailableWallPlacements(HashSet<string> walls , int[] bluePlayerPositon, int[] redPlayerPosition)
        {
            List<int[,]> wallPlacements = new();
            // get horizontal walls
            for (int i = 1; i < 17; i+=2) // row
            {
                for (int j = 2; j <= 16; j++) // cols
                {
                    int[,] newWall = new int[3, 2]
                    {
                        {i +0 , j},
                        {i +1 , j},
                        {i +2 , j},
                    };
                    var flag = this.validator.validateWallPlacement(newWall, walls, bluePlayerPositon, redPlayerPosition);
                    if (flag)
                    {
                        wallPlacements.Add(newWall);
                    }
                }
            }

            // get vertical valls
            for (int i = 2; i <= 16; i += 2) // row
            {
                for (int j = 1; j <= 15; j++) // cols
                {
                    int[,] newWall = new int[3, 2]
                    {
                        {i  , j + 0},
                        {i  , j + 1},
                        {i  , j + 2},
                    };
                    var flag = this.validator.validateWallPlacement(newWall, walls, bluePlayerPositon, redPlayerPosition);
                    if (flag)
                    {
                        wallPlacements.Add(newWall);
                    }
                }
            }


            return wallPlacements;
        }

        // for red
        public List<int[,]> findBestWallPlacements(List<int[,]> availableWallPlacements , HashSet<string> walls, int[] bluePlayerPosition
            , int[] redPlayerPosition, int initialDiff)
        {
            List<int[,]> bestWallPlacements = new();


            int minLength = int.MinValue;

            foreach(var wallPlacement in availableWallPlacements)
            {
                // add to walls
                for (int i = 0; i < wallPlacement.GetLength(0); i++)
                {
                    string hash = $"{wallPlacement[i, 0]}-{wallPlacement[i, 1]}";
                    walls.Add(hash);
                }

                int redPlayersPathLenght = this.bfs.GetShortestPath(redPlayerPosition, 17, walls).Count;
                int bluePlayersPathLenght = this.bfs.GetShortestPath(bluePlayerPosition, 1, walls).Count;
                

                int diff =   bluePlayersPathLenght - redPlayersPathLenght;
                if(diff > minLength)
                {
                    bestWallPlacements.Clear();
                    bestWallPlacements.Add(wallPlacement);
                    minLength = diff;
                }
                else if(diff == minLength)
                {
                    bestWallPlacements.Add(wallPlacement);
                }

                // remove from walls
                for (int i = 0; i < wallPlacement.GetLength(0); i++)
                {
                    string hash = $"{wallPlacement[i, 0]}-{wallPlacement[i, 1]}";
                    walls.Remove(hash);
                }

            }



            return bestWallPlacements;
        }


        // for blue
        public List<int[,]> findBestWallPlacementsBlue(List<int[,]> availableWallPlacements, HashSet<string> walls, int[] bluePlayerPosition
     , int[] redPlayerPosition, int initialDiff)
        {
            List<int[,]> bestWallPlacements = new();


            int minLength = int.MinValue;

            foreach (var wallPlacement in availableWallPlacements)
            {
                // add to walls
                for (int i = 0; i < wallPlacement.GetLength(0); i++)
                {
                    string hash = $"{wallPlacement[i, 0]}-{wallPlacement[i, 1]}";
                    walls.Add(hash);
                }

                int redPlayersPathLenght = this.bfs.GetShortestPath(redPlayerPosition, 17, walls).Count;
                int bluePlayersPathLenght = this.bfs.GetShortestPath(bluePlayerPosition, 1, walls).Count;
                

                int diff = redPlayersPathLenght - bluePlayersPathLenght;
                if (diff > minLength)
                {
                    bestWallPlacements.Clear();
                    bestWallPlacements.Add(wallPlacement);
                    minLength = diff;
                }
                else if (diff == minLength)
                {
                    bestWallPlacements.Add(wallPlacement);
                }

                // remove from walls
                for (int i = 0; i < wallPlacement.GetLength(0); i++)
                {
                    string hash = $"{wallPlacement[i, 0]}-{wallPlacement[i, 1]}";
                    walls.Remove(hash);
                }

            }



            return bestWallPlacements;
        }
    }
}
