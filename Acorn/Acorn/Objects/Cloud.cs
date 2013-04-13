using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Acorn.Behaviors;
using Acorn.Hiromi;
using Acorn.Hiromi.Messaging;
using Acorn.Hiromi.Processing;

namespace Acorn.Objects
{
    public class Cloud : GameObject
    {
        protected override void OnInitialize()
        {
            this.AddComponent(new MovementBehavior(new Vector2(-0.03f, 0)));
            this.AddComponent(new WrapAroundScreenBehavior());
        }
    }
}
