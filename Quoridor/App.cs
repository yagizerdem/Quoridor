using Microsoft.Extensions.DependencyInjection;
using Quoridor.Enums;
using Quoridor.Observabels;
using Quoridor.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor
{
    internal class App
    {
        UIManager UIManager { get; set; }
        public App() {
            this.UIManager = SD.ServiceProvider.GetService<UIManager>();
            if (this.UIManager == null)
            {
                throw new Exception(SD.serviceConfigurationError);
            }
        }

        public void Run()
        {
            this.initilize();
            //while (true)
            //{
      
            //    Thread.Sleep(1000);
            //}
        }
        public void initilize()
        {
            SD.PlayerRed = new Player(Color.Red);
            SD.PlayerBlue = new Player(Color.Blue);

            SD.PlayerBlue.PropertyChanged += (sender, args) =>
            {
                UIManager.RefreshFrame();
            };

            SD.PlayerRed.PropertyChanged += (sender, args) =>
            {
                UIManager.RefreshFrame();
            };

            SD.PlayerBlue.row = 17;
            SD.PlayerBlue.col = 9;
            SD.PlayerRed.row = 1;
            SD.PlayerRed.col = 9;

            SD.info = new Info();

            SD.info.PropertyChanged += (sender, args) =>
            {
                UIManager.RefreshFrame();
            };
            SD.info.bluewallsLeft = 10;
            SD.info.redwallsLeft = 10;
            SD.info.message = "";
            SD.info.turn = Color.Blue;

            SD.walls = new Walls();

            SD.walls.PropertyChanged += (sender, args) =>
            {
                UIManager.RefreshFrame();
            };

            // row-col

            //SD.walls.Add("4-4");
            //SD.walls.Add("5-4");
            //SD.walls.Add("6-4");
            //SD.walls.Add("7-6");
        }
    }
}
