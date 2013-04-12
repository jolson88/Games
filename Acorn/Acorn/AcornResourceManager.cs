using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Acorn.Hiromi;

namespace Acorn
{
    public class AcornResourceManager
    {
        private ContentManager _content;

        public AcornResourceManager(ContentManager content)
        {
            _content = content;
        }

        public Background GetBackground()
        {
            return new Background(_content.Load<Texture2D>("Sprites\\Background"));
        }

        public Sprite GetCloud()
        {
            return new Sprite(_content.Load<Texture2D>("Sprites\\Cloud"));
        }
    }
}
