using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.Engine.PathFindings
{
    internal class Bfs
    {
        public List<Node> GetShortestPath(int[] from, int targetRow, HashSet<string> walls)
        {
            HashSet<string> openSet = new();
            Queue<Node> queue = new Queue<Node>();
            int curRow = from[0] , curCol = from[1];
            Node start = new Node();
            start.row = curRow;
            start.col = curCol; 
            queue.Enqueue(start);

            while(queue.Count > 0)
            {
                Node curNode = queue.Dequeue();
                if(curNode.row == targetRow)
                {
                    return reconstructPath(curNode);
                }
                
                openSet.Add(curNode.hash);
                List<Node> neighbors = getNeighbors(curNode , walls );
                               

                foreach(Node neighbor in neighbors)
                {
                    if (!openSet.Contains(neighbor.hash))
                    {
                        queue.Enqueue(neighbor);
                        openSet.Add(neighbor.hash);
                        neighbor.parent = curNode;
                    }
                }

            }

            return null;

        }
        private List<Node> getNeighbors(Node node, HashSet<string> walls)
        {
            List<Node> neighbors = new();
            int row = node.row;
            int col = node.col;
            List<Direction> directions = new List<Direction>() { new Direction(2,0) , new Direction(-2, 0) , new Direction(0, 2) , new Direction(0, -2) };
            foreach(Direction direction in directions)
            {
                int newRow = row + direction.y;
                int newCol = col + direction.x;

                // ceheck map bounds
                if (newRow < 1 || newRow > 17 || newCol < 1 || newCol > 17) continue;

                // check is blocked
                if (walls.Contains($"{(row + newRow) / 2}-{(col + newCol) / 2}")) continue;


                Node neighbor = new Node();
                neighbor.row = newRow;
                neighbor.col = newCol;
                neighbors.Add(neighbor);
            }

            return neighbors;
        }

        private class Direction
        {
            public Direction(int x , int y)
            {
                this.x = x;
                this.y = y;
            }
            public int x { get; set; } // col
            public int y { get; set; } // row
        }
        
        private List<Node> reconstructPath(Node finalNode)
        {
            List<Node> path = new List<Node>();
            Node temp = finalNode;
            while (temp != null)
            {
                path.Add(temp);
                temp = temp.parent;
            }
            path.Reverse();
            return path;
        }
    
        
        public List<List<Node>> GetAllShortestPath(int[] from, int targetRow, HashSet<string> walls)
        {
            List<List<Node>> allShortestPath = new List<List<Node>>();  
 
            HashSet<string> openSet = new();
            Queue<Node> queue = new Queue<Node>();
            int curRow = from[0], curCol = from[1];
            Node start = new Node();
            start.row = curRow;
            start.col = curCol;
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                Node curNode = queue.Dequeue();
                if (curNode.row == targetRow)
                {
                    allShortestPath.Add(reconstructPath(curNode));
                }

                openSet.Add(curNode.hash);
                List<Node> neighbors = getNeighbors(curNode, walls);


                foreach (Node neighbor in neighbors)
                {
                    if (!openSet.Contains(neighbor.hash))
                    {
                        queue.Enqueue(neighbor);
                        openSet.Add(neighbor.hash);
                        neighbor.parent = curNode;
                    }
                }

            }


            if (allShortestPath.Count == 0) return null;
            return allShortestPath;
        }
    }
}
