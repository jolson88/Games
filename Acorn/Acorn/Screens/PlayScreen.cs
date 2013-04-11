using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Jarrett;

namespace Acorn.Screens
{
    public class PlayScreen : Screen
    {
        protected override Background InitializeBackground()
        {
            return new Background(Content.Load<Texture2D>("Sprites\\Background"));
        }
    }
}
