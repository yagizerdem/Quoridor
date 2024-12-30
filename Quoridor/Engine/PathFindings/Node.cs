using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.Engine.PathFindings
{
    internal class Node
    {
        public int row;
        public int col;
        public Node parent;
        public int g = 0;
        public int h = 0;
        public int f = 0;
        public string hash => $"${row}-{col}";
    }
}
