using Quoridor.Engine.Utils;
using Quoridor.Observabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.Engine
{
    internal class Engine
    {
        private readonly Helper helper;
        private readonly Validator validator;
        private readonly Bot bot;
        private readonly Quoridor.Engine.Ai.Ai ai; 
        public Engine()
        {
            this.helper = new Helper();
            this.validator = new Validator();   
            this.bot = new Bot();
            this.ai = new Quoridor.Engine.Ai.Ai();
        }

        public void MovePlayer(int[] to)
        {
            if (SD.gameEnd) return;
            Player currentPlayer = helper.getCurrentPlayer();
            int[] from = new int[2] {currentPlayer.row, currentPlayer.col};
            if (validator.ValidatePlayerMovement(from , to , SD.walls.getHashSet()))
            {
                currentPlayer.row = to[0];
                currentPlayer.col = to[1];
                SD.info.message = "";

                if(SD.gameMode == Enums.GameModes.Solo)
                {
                    SD.info.SwitchTurn();
                }
                else if(SD.gameMode == Enums.GameModes.Bot)
                {
                    bot.Move();
                }
                else if(SD.gameMode == Enums.GameModes.Ai)
                {
                    ai.Move();
                }


            }
            else
            {
                SD.info.message = "invalid movement";
            }


        }
    
        public void PlaceWall(int[,] row_col)
        {
            if (SD.gameEnd) return;
            try
            {
                // check wall count 
                if (SD.info.turn == Enums.Color.Blue && SD.info.bluewallsLeft == 0)
                {
                    SD.info.message = "no walls left";
                    throw new Exception();
                }

                if (SD.info.turn == Enums.Color.Red && SD.info.redwallsLeft== 0)
                {
                    SD.info.message = "no walls left";
                    throw new Exception();
                }
                //
                int[] bluePlayerPosition = [SD.PlayerBlue.row , SD.PlayerBlue.col];
                int[] redPlayerPositon = [SD.PlayerRed.row, SD.PlayerRed.col];
                bool flag = this.validator.validateWallPlacement(row_col, SD.walls.getHashSet() , bluePlayerPosition , redPlayerPositon);
               
                // place wall if flag is true
                if (flag)
                {
                    // place walls
                    for (int i = 0; i < row_col.GetLength(0); i++)
                    {
                        int row = row_col[i, 0];
                        int col = row_col[i, 1];
                        string hash = $"{row}-{col}";
                        SD.walls.Add(hash);
                    }
                    if (SD.info.turn == Enums.Color.Blue)
                    {
                        SD.info.bluewallsLeft--;
                    }
                    else
                    {
                        SD.info.redwallsLeft--;
                    }
                   
                    SD.info.message = "";

                    if (SD.gameMode == Enums.GameModes.Solo)
                    {
                        SD.info.SwitchTurn();
                    }
                    else if (SD.gameMode == Enums.GameModes.Bot)
                    {
                        bot.Move();
                    }
                    else if (SD.gameMode == Enums.GameModes.Ai)
                    {
                        ai.Move();
                    }

                }
                else
                {
                    SD.info.message = "invalid wall placement";
                }
            }catch(Exception ex)
            {
                
            }
        }
        
        public void CheckWinner()
        {
            if(SD.PlayerBlue.row == 1)
            {
                SD.info.message = "player blue win";
                SD.gameEnd = true;
            }
            if(SD.PlayerRed.row == 17)
            {
                SD.info.message = "player red winds";
                SD.gameEnd = true;
            }
        }
    }
}
