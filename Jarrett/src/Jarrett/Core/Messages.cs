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
    class NewGameRequestMessage : Message 
    {
        public int HumanPlayers { get; set; }
        public int TotalPlayers { get; set; }
        public string LevelName { get; set; }

        public NewGameRequestMessage(string levelName, int humanPlayers, int totalPlayers = 2)
        {
            this.LevelName = levelName;
            this.HumanPlayers = humanPlayers;
            this.TotalPlayers = totalPlayers;
        }
    }

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
