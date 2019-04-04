using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyGame
{
    class GameBulletException : Exception
    {        
        public GameBulletException(string message) : base(message){}
    }
}
