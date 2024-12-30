using Quoridor.Engine.PathFindings;
using Quoridor.Engine.Utils;
using Quoridor.Observabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.Engine
{
    internal class Bot
    {
        private readonly Bfs bfs;
        private readonly Helper helper;
        public Bot()
        {
            this.bfs = new Bfs();
            this.helper = new Helper();
        }

        public void Move()
        {
            Player bot = SD.PlayerRed;
            Player humanPlayer = SD.PlayerBlue;
            HashSet<string> walls = SD.walls.getHashSet();

            int[] botFrom = [bot.row, bot.col];
            int botTargetRow = 17;

            List<Node> botPath = bfs.GetShortestPath(botFrom, botTargetRow, walls);

            int[] humanPlayerFrom = [humanPlayer.row, humanPlayer.col];
            int humanPlayerTargetRow = 1;

            List<Node> humanPlayerPath = bfs.GetShortestPath(humanPlayerFrom, humanPlayerTargetRow, walls);

            int lengthDiff = botPath.Count - humanPlayerPath.Count;
            
            if(lengthDiff < 0)
            {
                // move bot 1 step
                if(botPath != null)
                {
                    Node next = botPath.Skip(1).FirstOrDefault();
                    bot.row = next.row;
                    bot.col = next.col; 
                }
            }
            else
            {
                if(SD.info.redwallsLeft > 0)
                {
                    // find best wall and place it
                    List<int[,]> availableWallPlacements = this.helper.getAvailableWallPlacements(SD.walls.getHashSet(), humanPlayerFrom, botFrom);

                    List<int[,]> bestWallPlacements = this.helper.findBestWallPlacements(availableWallPlacements , walls , humanPlayerFrom, botFrom , lengthDiff);


                    // selct random best wall placement
                    int[,] randomBestWallPlacement = bestWallPlacements.OrderBy(x => Guid.NewGuid()).First();


                    // insrt wall to map 
                    for (int i = 0; i < randomBestWallPlacement.GetLength(0); i++)
                    {
                        int row = randomBestWallPlacement[i,0];
                        int col = randomBestWallPlacement[i,1];
                        string hash = $"{row}-{col}";
                        SD.walls.Add(hash);
                    }
                    SD.info.redwallsLeft--;
                }
                else
                {
                    // move bot 1 step
                    if (botPath != null)
                    {
                        Node next = botPath.Skip(1).FirstOrDefault();
                        bot.row = next.row;
                        bot.col = next.col;
                    }
                }
            }

        }
    }
}
