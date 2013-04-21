using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Microsoft.Xna.Framework.Graphics;

namespace Acorn.Components
{
    public class HudComponent : GameObjectComponent
    {
        public Texture2D Texture { get; set; }

        public HudComponent(Texture2D texture)
        {
            this.Texture = texture;
        }
    }
}
