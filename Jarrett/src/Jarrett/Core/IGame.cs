using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jarrett.Core
{
    interface IGame
    {
        GraphicsDevice Device { get; }
        IResourceManager Resources { get; }
    }
}
