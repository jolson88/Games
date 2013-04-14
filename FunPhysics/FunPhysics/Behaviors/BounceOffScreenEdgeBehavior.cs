using Hiromi;
using Hiromi.Behaviors;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunPhysics.Behaviors
{
    class BounceOffScreenEdgeBehavior : GameObjectBehavior
    {
        public override void Update(GameTime gameTime)
        {
            var view_bounds = GraphicsService.Instance.GraphicsDevice.Viewport.Bounds;

            //TODO: Take center into account
            //var c = GameObject.Sprite.Center;
            var sprite_bounds = GameObject.Sprite.Texture.Bounds;
            sprite_bounds.Offset(
                (int)(GameObject.Position.X * view_bounds.Width),
                (int)(GameObject.Position.Y * view_bounds.Height));

            var move_behavior = this.GameObject.GetBehavior<MovementBehavior>();

            if (sprite_bounds.Left < view_bounds.Left || sprite_bounds.Right > view_bounds.Right)
                move_behavior.Velocity = new Vector2(move_behavior.Velocity.X * -1, move_behavior.Velocity.Y);

            if (sprite_bounds.Top < view_bounds.Top || sprite_bounds.Bottom > view_bounds.Bottom)
                move_behavior.Velocity = new Vector2(move_behavior.Velocity.X, move_behavior.Velocity.Y * -1);
        }
    }


}
