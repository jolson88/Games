using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi.Messaging;

namespace Acorn
{
    /// <summary>
    /// Message from the system to start a game.
    /// </summary>
    public class StartGameMessage : Message
    {
        public override string ToString()
        {
            return "[System] Starting game";
        }
    }
}
