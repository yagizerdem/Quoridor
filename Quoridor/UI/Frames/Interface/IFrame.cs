using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.UI.Frames.Interface
{
    internal interface IFrame
    {

        public void Render();

        public void Refresh();

        public void Clear();

        public void Loop();

        public delegate void SwitchFrame(int index);
        public event SwitchFrame OnSwitchFrame;
    }
}
