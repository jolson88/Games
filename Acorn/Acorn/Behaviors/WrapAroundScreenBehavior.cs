﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acorn.Hiromi;
using Acorn.Hiromi.Messaging;
using Acorn.Hiromi.Processing;
using Microsoft.Xna.Framework;

namespace Acorn.Behaviors
{
    public class WrapAroundScreenBehavior : BehaviorComponent
    {
        private bool _canBeReflected = true;

        public WrapAroundScreenBehavior()
        {
            MessageService.Instance.AddListener<OffScreenMessage>(msg => { OnOffScreen((OffScreenMessage)msg); });
        }

        public void OnOffScreen(OffScreenMessage msg)
        {
            if (_canBeReflected && msg.GameObjectId == this.GameObject.Id)
            {
                // Remember to account for Sprite center offsets!
                var xOffset = this.GameObject.Sprite.Center.X / GraphicsService.Instance.GraphicsDevice.Viewport.Width;
                var yOffset = this.GameObject.Sprite.Center.Y / GraphicsService.Instance.GraphicsDevice.Viewport.Height;

                // Reflect X-axis
                if (this.GameObject.Bounds.Right < 0.0f)
                {
                    this.GameObject.Position = new Vector2(1.0f + xOffset, this.GameObject.Position.Y);
                }
                else if (this.GameObject.Bounds.Left > GraphicsService.Instance.GraphicsDevice.Viewport.Width)
                {
                    this.GameObject.Position = new Vector2(-((float)this.GameObject.Sprite.Texture.Width / (float)GraphicsService.Instance.GraphicsDevice.Viewport.Width) + xOffset, this.GameObject.Position.Y);
                }

                // Reflect Y-axis
                if (this.GameObject.Bounds.Bottom < 0.0f)
                {
                    this.GameObject.Position = new Vector2(this.GameObject.Position.X, 1.0f + yOffset);
                }
                else if (this.GameObject.Bounds.Top > GraphicsService.Instance.GraphicsDevice.Viewport.Height)
                {
                    this.GameObject.Position = new Vector2(this.GameObject.Position.X,
                        -((float)this.GameObject.Sprite.Texture.Height / (float)GraphicsService.Instance.GraphicsDevice.Viewport.Height) + yOffset);
                }

                _canBeReflected = false;
            }

            // Make sure we don't keep resetting ourselves by introducing a delay
            this.GameObject.ProcessManager.AttachProcess(new DelayProcess(TimeSpan.FromSeconds(1),
                new ActionProcess(() => { _canBeReflected = true; })));
        }
    }
}