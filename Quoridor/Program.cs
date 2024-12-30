using Microsoft.Extensions.DependencyInjection;
using Quoridor.UI;
using System.Collections.ObjectModel;

namespace Quoridor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StartUp start = new StartUp();
            start.initilize();

            UIManager? uiManager = SD.ServiceProvider?.GetRequiredService<UIManager>();
            if(uiManager == null)
            {
                throw new Exception("initial configuration failed");
            }

            // new thread for  ui
            Thread thread = new Thread(new ThreadStart(() => uiManager.Run()));
            thread.Start();

            App app = new App();
            app.Run();
        }
    }
}
