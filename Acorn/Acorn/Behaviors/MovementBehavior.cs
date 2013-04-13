﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Acorn.Hiromi;

namespace Acorn.Behaviors
{
    public class MovementBehavior : BehaviorComponent
    {
        // Velocity to change in seconds
        public Vector2 Velocity { get; set; }

        public MovementBehavior(Vector2 velocity)
        {
            this.Velocity = velocity;
        }

        public override void Update(GameTime gameTime)
        {
            this.GameObject.Position += this.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}