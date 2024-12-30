using Quoridor.Engine.PathFindings;
using Quoridor.Engine.Utils;
using Quoridor.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.Engine.Ai
{
    internal class Ai
    {
        public void Move()
        {
            if (SD.PlayerBlue.row == 1 || SD.PlayerRed.row == 17) return;

            MCTS mcts = new MCTS(1000);

            MCTSNode bestMove = mcts.Search();
            SD.PlayerRed.row = bestMove.redPlayerPosition[0];
            SD.PlayerRed.col = bestMove.redPlayerPosition[1];

            HashSet<string> fromAi = bestMove.walls;
            foreach (var value in fromAi)
            {
                if (!SD.walls.Contains(value))
                {
                    SD.walls.Add(value);
                }
            }

        }
    }

    internal class MCTS
    {
        public MCTSNode root;
        public int maxSimulationCount = 0;
        private Bfs bfs;
        private Validator validator;
        private Helper helper;
        private int simulationCount = 0;
        public MCTS(int maxSimulationCount)
        {
            this.root = new MCTSNode();
            this.maxSimulationCount = maxSimulationCount;
            this.bfs = new Bfs();
            this.validator = new Validator();
            this.helper = new Helper();

            // states
            int[] redPlayerPosition = [SD.PlayerRed.row, SD.PlayerRed.col];
            int[] bluePlayerPosition = [SD.PlayerBlue.row, SD.PlayerBlue.col];
            HashSet<string> walls = new HashSet<string>(SD.walls.getHashSet()); // deep copy
            Color turn = Color.Red;
            int blueWallsLeft = SD.info.bluewallsLeft;
            int redWallsLeft = SD.info.redwallsLeft;

            // set states for root node
            this.root.walls = walls;
            this.root.turn = turn;
            this.root.bluePlayerPosition = bluePlayerPosition;
            this.root.redPlayerPosition = redPlayerPosition;
            this.root.blueWallsLeft = blueWallsLeft;
            this.root.redWallsLeft = redWallsLeft;

        }

        public MCTSNode Search()
        {
            MCTSNode curNode = this.root;
            while (this.simulationCount < maxSimulationCount)
            {
                if (curNode.isTerminalNode)
                {
                    curNode = this.root;
                    this.simulationCount++;
                }
                else if (curNode.isLeafNode)
                {
                    if (curNode.isNew)
                    {

                        this.Rollout(curNode);

                        curNode = this.root;
                    }
                    else
                    {
                        if (!curNode.isTerminalNode)
                        {
                            // expansion
                            this.Expansion(curNode);
                            curNode = this.root;
                        }
                        else
                        {
                            curNode = this.root;
                            this.simulationCount++;
                        }
                    }
                }
                else
                {
                    curNode = curNode.getChildWithMaxUTC();
                }
            }

            MCTSNode bestMove = this.root.getChildWithMaxUTC();
            return bestMove;
        }

        public void Rollout(MCTSNode startNode)
        {
            Color? turn = startNode.turn;
            // deep copy
            int[] redPlayerPosition = (int[])startNode.redPlayerPosition.Clone();
            int[] bluePlayerPosition = (int[])startNode.bluePlayerPosition.Clone();
            HashSet<string> walls = new HashSet<string>(startNode.walls);
            int blueWallsLeft = startNode.blueWallsLeft;
            int redWallsLeft = startNode.redWallsLeft;

            bool isEnd = false;
            while (!isEnd)
            {
                List<Node> bluePlayerPath = this.bfs.GetShortestPath(bluePlayerPosition, 1, walls);
                List<Node> redPlayerPath = this.bfs.GetShortestPath(redPlayerPosition, 17, walls);

                int pathDiff = bluePlayerPath.Count - redPlayerPath.Count;

                if (turn == Color.Red)
                {
                    if (pathDiff > 0)
                    {
                        Node next = redPlayerPath.Skip(1).FirstOrDefault();
                        redPlayerPosition[0] = next.row;
                        redPlayerPosition[1] = next.col;
                    }
                    else
                    {
                        if(redWallsLeft == 0)
                        {
                            // move instead
                            Node next = redPlayerPath.Skip(1).FirstOrDefault();
                            redPlayerPosition[0] = next.row;
                            redPlayerPosition[1] = next.col;
                        }
                        else
                        {
                            List<int[,]> availableWallPlacement = this.helper.getAvailableWallPlacements(walls, bluePlayerPosition, redPlayerPosition);
                            List<int[,]> bestWallPlacements = this.helper.findBestWallPlacements(availableWallPlacement,
                                walls, bluePlayerPosition, redPlayerPosition, pathDiff);

                            if (bestWallPlacements == null)
                            {
                                // move instead
                                Node next = redPlayerPath.Skip(1).FirstOrDefault();
                                redPlayerPosition[0] = next.row;
                                redPlayerPosition[1] = next.col;
                            }
                            else
                            {
                                // place wall
                                int[,] randomWallPlacement = bestWallPlacements[new Random().Next(bestWallPlacements.Count)];
                                for (int i = 0; i < randomWallPlacement.GetLength(0); i++)
                                {
                                    int row = randomWallPlacement[i, 0];
                                    int col = randomWallPlacement[i, 1];
                                    string hash = $"{row}-{col}";
                                    walls.Add(hash);
                                }
                                redWallsLeft--;
                            }
                        }
    

                    }

                }

                if (turn == Color.Blue)
                {
                    if (pathDiff < 0)
                    {
                        Node next = bluePlayerPath.Skip(1).FirstOrDefault();
                        bluePlayerPosition[0] = next.row;
                        bluePlayerPosition[1] = next.col;
                    }
                    else
                    {
                        if(blueWallsLeft == 0)
                        {
                            // move instead
                            Node next = bluePlayerPath.Skip(1).FirstOrDefault();
                            bluePlayerPosition[0] = next.row;
                            bluePlayerPosition[1] = next.col;
                        }
                        else
                        {
                            List<int[,]> availableWallPlacement = this.helper.getAvailableWallPlacements(walls, bluePlayerPosition, redPlayerPosition);
                            List<int[,]> bestWallPlacements = this.helper.findBestWallPlacementsBlue(availableWallPlacement,
                                walls, bluePlayerPosition, redPlayerPosition, pathDiff);

                            if (bestWallPlacements == null)
                            {
                                // move instead
                                Node next = bluePlayerPath.Skip(1).FirstOrDefault();
                                bluePlayerPosition[0] = next.row;
                                bluePlayerPosition[1] = next.col;
                            }
                            else
                            {
                                // place wall
                                int[,] randomWallPlacement = bestWallPlacements[new Random().Next(bestWallPlacements.Count)];
                                for (int i = 0; i < randomWallPlacement.GetLength(0); i++)
                                {
                                    int row = randomWallPlacement[i, 0];
                                    int col = randomWallPlacement[i, 1];
                                    string hash = $"{row}-{col}";
                                    walls.Add(hash);
                                }
                                blueWallsLeft--;
                            }
                        }

   
                    }

                }
                // swith turn
                turn = turn == Color.Red ? Color.Blue : Color.Red;

                bool flag = bluePlayerPosition[0] == 1 || redPlayerPosition[0] == 17;
                if (flag)
                {
                    isEnd = true;
                }

            }
            int point = 0; // tie  
            if (redPlayerPosition[0] == 1)
            {
                point = 1; // ai wins
            }
            if (bluePlayerPosition[0] == 17)
            {
                point = -1; // ai loses
            }

            MCTSNode cur = startNode;
            while (cur != null)
            {
                cur.visitCount++;
                cur.totalWins += point;
                cur = cur.parent;
            }
            this.simulationCount++;
        }

        public void Expansion(MCTSNode parentNode)
        {
            Color? turn = parentNode.turn;
            // deep copy
            int[] redPlayerPosition = (int[])parentNode.redPlayerPosition.Clone();
            int[] bluePlayerPosition = (int[])parentNode.bluePlayerPosition.Clone();
            HashSet<string> walls = new HashSet<string>(parentNode.walls);

            int blueWallsLeft = parentNode.blueWallsLeft;
            int redWallsLeft = parentNode.redWallsLeft;

            List<Node> bluePlayerPath = this.bfs.GetShortestPath(bluePlayerPosition, 1, walls);
            List<Node> redPlayerPath = this.bfs.GetShortestPath(redPlayerPosition, 17, walls);

            int pathDiff = bluePlayerPath.Count - redPlayerPath.Count;

            // expansion for red
            if (turn == Color.Red)
            {
                // player move    
                List<List<Node>> allShortestPath = this.bfs.GetAllShortestPath(redPlayerPosition, 17, walls);
                if (allShortestPath != null)
                {
                    foreach (List<Node> path in allShortestPath)
                    {
                        if (path.Count > 1)
                        {
                            Node next = path.Skip(1).First();
                            int[] newRedPlayerPosition = [next.row, next.col];

                            MCTSNode newMCTSNode = new MCTSNode();
                            newMCTSNode.parent = parentNode;
                            newMCTSNode.turn = Color.Blue;
                            newMCTSNode.bluePlayerPosition = (int[])bluePlayerPosition.Clone();
                            newMCTSNode.redPlayerPosition = newRedPlayerPosition;
                            newMCTSNode.walls = new HashSet<string>(walls);
                            newMCTSNode.redWallsLeft = redWallsLeft;
                            newMCTSNode.blueWallsLeft = blueWallsLeft;

                            if (newRedPlayerPosition[0] == 17)
                            {
                                newMCTSNode.isTerminalNode = true;
                            }

                            parentNode.children.Add(newMCTSNode);
                        }
                    }
                }

                // wall placement
                List<int[,]> availableWallPlacements = this.helper.getAvailableWallPlacements(walls, bluePlayerPosition, redPlayerPosition);

                List<int[,]> bestWallPlacements = this.helper.findBestWallPlacements(availableWallPlacements,
                    walls, bluePlayerPosition, redPlayerPosition, pathDiff);

                foreach (int[,] wallPlacement in bestWallPlacements)
                {
                    MCTSNode newMCTSNode = new MCTSNode();
                    newMCTSNode.parent = parentNode;
                    newMCTSNode.turn = Color.Blue;
                    newMCTSNode.bluePlayerPosition = (int[])bluePlayerPosition.Clone();
                    newMCTSNode.redPlayerPosition = (int[])redPlayerPosition.Clone();
                    newMCTSNode.redWallsLeft = redWallsLeft - 1;
                    newMCTSNode.blueWallsLeft = blueWallsLeft;  

                    HashSet<string> newWallFormation = new HashSet<string>(walls);
                    for (int i = 0; i < wallPlacement.GetLength(0); i++)
                    {
                        int row = wallPlacement[i, 0];
                        int col = wallPlacement[i, 1];
                        string hash = $"{row}-{col}";
                        newWallFormation.Add(hash);
                    }


                    newMCTSNode.walls = newWallFormation;

                    parentNode.children.Add(newMCTSNode);
                }
            }



            // expansion for blue
            if (turn == Color.Blue)
            {
                // player move    
                List<List<Node>> allShortestPath = this.bfs.GetAllShortestPath(bluePlayerPosition, 1, walls);
                if (allShortestPath != null)
                {
                    foreach (List<Node> path in allShortestPath)
                    {
                        if (path.Count > 1)
                        {
                            Node next = path.Skip(1).First();
                            int[] newBluePlayerPositon = [next.row, next.col];

                            MCTSNode newMCTSNode = new MCTSNode();
                            newMCTSNode.parent = parentNode;
                            newMCTSNode.turn = Color.Blue;
                            newMCTSNode.bluePlayerPosition = newBluePlayerPositon;
                            newMCTSNode.redPlayerPosition = (int[])redPlayerPosition.Clone();
                            newMCTSNode.walls = new HashSet<string>(walls);
                            newMCTSNode.redWallsLeft = redWallsLeft;
                            newMCTSNode.blueWallsLeft = blueWallsLeft;

                            if (newBluePlayerPositon[0] == 1)
                            {
                                newMCTSNode.isTerminalNode = true;
                            }

                            parentNode.children.Add(newMCTSNode);
                        }
                    }
                }

                // wall placement
                List<int[,]> availableWallPlacements = this.helper.getAvailableWallPlacements(walls, bluePlayerPosition, redPlayerPosition);

                List<int[,]> bestWallPlacements = this.helper.findBestWallPlacementsBlue(availableWallPlacements,
                    walls, bluePlayerPosition, redPlayerPosition, pathDiff);

                foreach (int[,] wallPlacement in bestWallPlacements)
                {
                    MCTSNode newMCTSNode = new MCTSNode();
                    newMCTSNode.parent = parentNode;
                    newMCTSNode.turn = Color.Blue;
                    newMCTSNode.bluePlayerPosition = (int[])bluePlayerPosition.Clone();
                    newMCTSNode.redPlayerPosition = (int[])redPlayerPosition.Clone();
                    newMCTSNode.redWallsLeft = redWallsLeft;
                    newMCTSNode.blueWallsLeft = blueWallsLeft -1;

                    HashSet<string> newWallFormation = new HashSet<string>(walls);
                    for (int i = 0; i < wallPlacement.GetLength(0); i++)
                    {
                        int row = wallPlacement[i, 0];
                        int col = wallPlacement[i, 1];
                        string hash = $"{row}-{col}";
                        newWallFormation.Add(hash);
                    }


                    newMCTSNode.walls = newWallFormation;

                    parentNode.children.Add(newMCTSNode);
                }
            }

        }
    }
}
