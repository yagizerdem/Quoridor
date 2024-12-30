using Quoridor.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.Engine.Ai
{
    internal class MCTSNode
    {
        public List<MCTSNode> children = new List<MCTSNode>();
        public int visitCount = 0;
        public int totalWins = 0;
        public bool isTerminalNode = false;
        public int blueWallsLeft;
        public int redWallsLeft;
        public bool isLeafNode => children.Count == 0;
        public bool isNew => this.visitCount == 0;
        public static double utcConstant = Math.Sqrt(2);
        public MCTSNode parent = null;

        // states
        public HashSet<string> walls = new HashSet<string>();
        public Color? turn = null;
        public int[] bluePlayerPosition = new int[2];
        public int[] redPlayerPosition = new int[2];

        public MCTSNode()
        {
                
        }

        public double calculateUtc()
        {
            if(this.parent == null)
            {
                return 0;
            }
            if(this.visitCount == 0)
            {
                return int.MaxValue;
            }

            double utcScore = 0;
            utcScore += this.totalWins / this.visitCount;
            utcScore += MCTSNode.utcConstant * Math.Sqrt(Math.Log(this.parent.visitCount) / this.visitCount);
            return utcScore;
        }

        public MCTSNode getChildWithMaxUTC()
        {
            List<MCTSNode> nodes = new List<MCTSNode>();
            double maxUTC = int.MinValue;

            foreach (MCTSNode node in this.children)
            {
                double curUTC = node.calculateUtc();
                if (curUTC > maxUTC)
                {
                    maxUTC = curUTC;
                    nodes.Clear();
                    nodes.Add(node);
                }
                else if (curUTC == maxUTC)
                {
                    nodes.Add(node);
                }
            }
            MCTSNode randomNode = nodes[new Random().Next(nodes.Count)];
            return randomNode;
        }
    }
}
