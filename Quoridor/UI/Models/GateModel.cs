using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.UI.Models
{
    internal class GateModel
    {
        public string Gate = null;
        public GateModel()
        {
            this.Gate = """
                ---------------------
                |     Play solo     |
                |  Play against ai  |
                |  Play against bot |
                ---------------------
                """.Trim();
        }

        public string GetGate()
        {
            return this.Gate;
        }
    }
}
