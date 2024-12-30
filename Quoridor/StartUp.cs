using Microsoft.Extensions.DependencyInjection;
using Quoridor.Engine;
using Quoridor.Observabels;
using Quoridor.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor
{
    internal class StartUp
    {
        
        public void initilize()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            
            // register services
            serviceCollection.AddSingleton<Cursor>();
            serviceCollection.AddSingleton<UIManager>();
            serviceCollection.AddSingleton<Renderer>();
            serviceCollection.AddSingleton<CursorPositions>();
            serviceCollection.AddSingleton<Engine.Engine>();

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            SD.ServiceProvider = serviceProvider; // acces servie provider globally
        }
    }
}
