using Quoridor.UI.Frames.Class;
using Quoridor.UI.Frames.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.UI
{
    internal class UIManager
    {
        private List<IFrame> allFrames  = new List<IFrame>();   
        private int ActiveFrameIndex {  get; set; }


        public UIManager()
        {

            IFrame mainFrame = new MainFrame();
            IFrame gameFrame = new GameFrame();
            allFrames.Add(mainFrame);
            allFrames.Add(gameFrame);

            foreach(var frame in allFrames)
            {
                frame.OnSwitchFrame += (int index) =>
                {
                    this.switchFrame(index);
                };
            }

            this.ActiveFrameIndex = 0;
        }
        public void Run()
        {
            this.initilizeFrame();
        }
        
        public void switchFrame(int index)
        {
            this.ActiveFrameIndex = index;
            this.initilizeFrame();
        }
        private void initilizeFrame()
        {
            IFrame activeFrame = this.allFrames[ActiveFrameIndex];
            activeFrame.Clear(); // render after clearing console screen
            activeFrame.Render();
            activeFrame.Loop();
        }
    
        public void RefreshFrame()
        {
            this.allFrames[this.ActiveFrameIndex].Refresh();
        }
    }
}
