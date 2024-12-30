using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.UI
{
    internal class Renderer
    {
        Cursor? cursor;
        public Renderer()
        {
            this.cursor = SD.ServiceProvider?.GetRequiredService<Cursor>();
            if(this.cursor == null)
            {
                throw new Exception(SD.serviceConfigurationError);
            }
        }
        public void Render(int x , int y , string contenet)
        {
            string[] lines = contenet.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            for(int i = 0; i < lines.Length; i++)
            {
                cursor?.Write(x , y + i , lines[i]);
            }
        }
    }
}
