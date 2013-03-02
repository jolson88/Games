using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Jarrett.Core
{
    interface IGameView
    {
        void Initialize(IGame game);
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
