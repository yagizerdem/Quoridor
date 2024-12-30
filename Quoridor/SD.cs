using Quoridor.Enums;
using Quoridor.Observabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor
{
    internal class SD
    {
        
        public static IServiceProvider? ServiceProvider = null;

        public static string serviceConfigurationError = "service is not configured";

        
        // non reactive states
        public static GameModes gameMode;

        public static Player PlayerBlue;

        public static bool gameEnd = false;

        // reactive states
        public static Player PlayerRed;

        public static Info info;

        public static Walls walls;
    }
}
