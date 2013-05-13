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
        public Vector2 OnscreenDestination { get; set; }
        public Vector2 OffscreenDestination { get; set; }

        public PlayerAvatarComponent(int playerIndex, Vector2 onscreenDestination, Vector2 offscreenDestination)
        {
            this.PlayerIndex = playerIndex;
            this.OnscreenDestination = onscreenDestination;
            this.OffscreenDestination = offscreenDestination;
        }

        protected override void OnLoaded()
        {
            this.GameObject.Transform.Position = this.OffscreenDestination;
        }
    }
}
