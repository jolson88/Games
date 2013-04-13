using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Acorn.Hiromi
{
    public class GameObjectBehavior
    {
        public GameObject GameObject { get; set; }

        public virtual void Update(GameTime gameTime) { }
    }
}
