using Microsoft.Extensions.DependencyInjection;
using Quoridor.Observabels;
using Quoridor.UI.Frames.Interface;
using Quoridor.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.UI.Frames.Class
{
    internal class MainFrame : IFrame
    {
        Renderer? renderer;
        GateModel? gateModel;
        CursorPositions? cursorPositions;
        Cursor? cursor;
        public MainFrame()
        {
            this.cursor = SD.ServiceProvider?.GetRequiredService<Cursor>();
            this.cursorPositions = SD.ServiceProvider?.GetRequiredService<CursorPositions>();
            this.renderer = SD.ServiceProvider?.GetRequiredService<Renderer>();
            if (this.renderer == null || this.cursorPositions == null || this.cursor == null)
            {
                throw new Exception(SD.serviceConfigurationError);
            }
            this.gateModel = new GateModel();


        }

        public event IFrame.SwitchFrame OnSwitchFrame;

        public void Clear()
        {
            cursor?.Clear();
        }

        public void Loop()
        {
            this.cursorPositions.setCx(1);
            this.cursorPositions.setCy(1);
            bool isRunning = true;
            while (isRunning)
            {
                ConsoleKeyInfo ckey = Console.ReadKey();
                if (ckey.Key == ConsoleKey.DownArrow || ckey.Key == ConsoleKey.UpArrow)
                {
                    int newCy = this.cursorPositions.getCy() + (ckey.Key == ConsoleKey.DownArrow ? 1 : -1);
                    int newCx = this.cursorPositions.getCx();
                    if (this.checkBounds(newCx, newCy))
                    {
                        this.cursorPositions.setCy(newCy);
                        this.cursorPositions.setCx(newCx);
                    }
                }
                else if (ckey.Key == ConsoleKey.Enter)
                {
                    int cy = this.cursorPositions.getCy();
                    switch (cy)
                    {
                        case 1:
                            SD.gameMode = Enums.GameModes.Solo;
                            break;
                        case 2:
                            SD.gameMode = Enums.GameModes.Ai;
                            break;
                        case 3:
                            SD.gameMode = Enums.GameModes.Bot;
                            break;
                    }
                    isRunning = false;
                    this.OnSwitchFrame.Invoke(1); // gt to game frame
                }
                Thread.Sleep(50);
            }
        }

        public void Refresh()
        {
            this.Clear();
            this.Render();
        }

        public void Render()
        {
            string board = this.gateModel.Gate;
            this.renderer?.Render(0, 0, board);
            // sync cursor position
            this.cursor.setCursorPosition(this.cursorPositions.getCx() , this.cursorPositions.getCy());
        }
        
        private bool checkBounds(int cx , int cy)
        {
            return cx == 1 && (cy >= 1 && cy <= 3);
        }
    }
}
