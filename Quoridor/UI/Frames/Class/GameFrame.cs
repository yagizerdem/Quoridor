using Microsoft.Extensions.DependencyInjection;
using Quoridor.Observabels;
using Quoridor.UI.Frames.Interface;
using Quoridor.UI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.UI.Frames.Class
{
    internal class GameFrame : IFrame
    {
        Cursor? cursor;
        BoardModel BoardModel;
        Renderer? renderer;
        CursorPositions? cursorPositions;
        Engine.Engine engine;

        public GameFrame()
        {
            this.cursor = SD.ServiceProvider?.GetRequiredService<Cursor>();
            this.renderer = SD.ServiceProvider?.GetRequiredService<Renderer>();
            this.cursorPositions = SD.ServiceProvider?.GetRequiredService<CursorPositions>();
            this.engine = SD.ServiceProvider.GetRequiredService<Engine.Engine>();
            if (this.engine == null || this.renderer == null || this.cursor == null || this.cursorPositions == null)
            {
                throw new Exception(SD.serviceConfigurationError);
            }
            this.BoardModel = new BoardModel();
        }

        public event IFrame.SwitchFrame OnSwitchFrame;

        public void Clear()
        {
            cursor?.Clear();
        }

        public void Loop()
        {
            // initilization
            this.cursorPositions.setCx(1);
            this.cursorPositions.setCy(1);
            bool isRunning = true;



            while (isRunning)
            {

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo ckey = Console.ReadKey(intercept: true);
                    // Clear the input buffer
                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey(intercept: true); // Discard remaining keys in the buffer
                    }


                    // keyboard readers
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
                    if (ckey.Key == ConsoleKey.LeftArrow || ckey.Key == ConsoleKey.RightArrow)
                    {
                        int newCy = this.cursorPositions.getCy();
                        int newCx = this.cursorPositions.getCx() + (ckey.Key == ConsoleKey.RightArrow ? 1 : -1);
                        if (this.checkBounds(newCx, newCy))
                        {
                            this.cursorPositions.setCy(newCy);
                            this.cursorPositions.setCx(newCx);
                        }
                    }
                    if(ckey.Key == ConsoleKey.Enter)
                    {
                        (int col , int row) = Console.GetCursorPosition();
                        this.engine.MovePlayer(new int[] {row , col});

                        // check if there i winner
                        this.engine.CheckWinner();
                    }
                    if(ckey.Key == ConsoleKey.NumPad1)
                    {
                        (int col, int row) = Console.GetCursorPosition();

                        int[,] row_cols = new int[3, 2];
                        for (int i = 0; i < 3; i++)
                        {
                            row_cols[i, 0] = row + i;
                            row_cols[i, 1] = col;
                        }
                        this.engine.PlaceWall(row_cols);
                    }
                    if (ckey.Key == ConsoleKey.NumPad2)
                    {
                        (int col, int row) = Console.GetCursorPosition();

                        int[,] row_cols = new int[3, 2];
                        for (int i = 0; i < 3; i++)
                        {
                            row_cols[i, 0] = row ;
                            row_cols[i, 1] = col + i;
                        }
                        this.engine.PlaceWall(row_cols);
                    }
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
            string board = this.BoardModel.GetBoard();
            this.renderer?.Render(0, 0, board);

            this.renderer?.Render(20, 1, $"turn : {(SD.info.turn == Enums.Color.Blue ? "blue turn" : "red turn")}");
            this.renderer?.Render(20, 2, $"blue walls left : {SD.info.bluewallsLeft}");
            this.renderer?.Render(20, 3, $"red walls left : {SD.info.redwallsLeft}");
            this.renderer?.Render(20, 4, $"message : {SD.info.message}");
            this.renderer?.Render(20, 5, $"press 1 to place horizontal wall,  press 2 to place vertical wall");
            // sync cursor position
            this.cursor.setCursorPosition(this.cursorPositions.getCx(), this.cursorPositions.getCy());
        }

        public bool checkBounds(int cx ,  int cy)
        {
            return cx >= 1 && cx <= 18 && cy >= 1 && cy <= 17;
        }
    }
}
