using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Acorn;
using Microsoft.Xna.Framework;

namespace Acorn.Components
{
    public class PlayerAvatarComponent : GameObjectComponent
    {
        public int PlayerIndex { get; set; }
        public Vector2 DestinationOffset { get; set; }

        public PlayerAvatarComponent(int playerIndex, Vector2 destinationOffset)
        {
            this.PlayerIndex = playerIndex;
            this.DestinationOffset = destinationOffset;
        }
    }
}
