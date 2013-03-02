using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Jarrett.Core
{
    class Message { }

    // ***************************
    //
    //       Game Messages
    //
    // ***************************    
    class NewGameRequestMessage : Message { }
    class QuitGameRequestMessage : Message { }


    // ***************************
    // 
    //       Input Messages
    //
    // ***************************
    class KeyDownMessage : Message
    {
        public Keys Key { get; set; }
        public KeyDownMessage(Keys key) { this.Key = key; }
    }

    class KeyUpMessage : Message
    {
        public Keys Key { get; set; }
        public KeyUpMessage(Keys key) { this.Key = key; }
    }
}
