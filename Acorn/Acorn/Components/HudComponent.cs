using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Messaging;
using Microsoft.Xna.Framework.Graphics;

namespace Acorn.Components
{
    public class HudComponent : IComponent
    {
        public Texture2D Texture { get; set; }

        public HudComponent(Texture2D texture)
        {
            this.Texture = texture;
        }
    }
}
