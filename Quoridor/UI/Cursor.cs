using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.UI
{
    internal class Cursor
    {

        private static readonly object _consoleLock = new object(); // Lock object , ensure thread save write methods
        public Cursor()
        {
            
        }
        public void Write(int x, int y, string content)
        {
            lock (_consoleLock)
            {
                (int left, int top) = Console.GetCursorPosition();
                Console.SetCursorPosition(x, y);
                Console.Write(content);
                // reset old position
                Console.SetCursorPosition(left, top);
            }
        }
        public void WriteLine(int x, int y, string content)
        {
            lock (_consoleLock)
            {
                (int left, int top) = Console.GetCursorPosition();
                Console.SetCursorPosition(x, y);
                Console.Write(content);
                // reset old position
                Console.SetCursorPosition(left, top);
            }
        }
        public void setCursorPosition(int x, int y)
        {
            lock (_consoleLock)
            {
                Console.SetCursorPosition(x, y);
            }
        }
        public (int, int) GetCursorPosition()
        {
            lock (_consoleLock)
            {
                return Console.GetCursorPosition();
            }
        }
    
        public void Clear()
        {
            lock (_consoleLock)
            {
                Console.Clear();
            }
        }
    }
}
