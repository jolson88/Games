using System;
using System.Collections.Generic;
using System.Linq;
using Hiromi;
using Hiromi.Entities;
using Hiromi.Entities.Components;
using Hiromi.Entities.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mulgrew.Screens
{
    class PlayScreen : Screen
    {
        private EntityWorld _world;
        private Camera _camera;
        
        protected override void OnInitialize()
        {
            _camera = new Camera(this.MessageManager, new Vector2(1600, 900));

            _world = new EntityWorld();
            _world.SetSystem(new SpriteRenderingSystem(_camera));
            _world.Initialize();

            _world.CreateEntity()
                    .AddComponent(new TransformComponent(new Vector2(100, 100)))
                    .AddComponent(new SpriteComponent(ContentService.Instance.GetAsset<Texture2D>("Sprites\\Grid")))
                    .AddToWorld();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            _world.Update(gameTime.ElapsedGameTime.TotalSeconds);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            _world.Draw(gameTime.ElapsedGameTime.TotalSeconds);
        }
    }
}
