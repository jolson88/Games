using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Messaging;

namespace Acorn.Behaviors
{
    public class ScoreBehavior : GameObjectBehavior
    {
        public int PlayerIndex { get; set; }

        public ScoreBehavior(int playerIndex)
        {
            this.PlayerIndex = playerIndex;
        }
    }
}
