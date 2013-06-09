using System;
using System.Collections.Generic;
using System.Linq;
using Hiromi;
using Hiromi.Entities;
using Hiromi.Entities.Components;
using Hiromi.Entities.Systems;
using Hiromi.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mulgrew.Screens.Components;
using Mulgrew.Screens.Systems;

namespace Mulgrew.Screens
{
    class PlayScreen : Screen
    {
        private static int SQUARE_SIZE = 64;
        private static int SQUARE_PADDING = 3;
        private static int COLUMN_COUNT = 9;
        private static int ROW_COUNT = 11;

        private EntityWorld _world;
        private Camera _camera;
        
        protected override void OnInitialize()
        {
            _camera = new Camera(new Vector2(1600, 900));
            this.MessageBus.Register(_camera);

            _world = new EntityWorld();
            _world.SetSystem(new SpriteRenderingSystem(_camera));
            _world.SetSystem(new JewelRenderingSystem(_camera));
            _world.Initialize();

            var gridLocation = new Vector2(500, 150);
            _world.CreateEntity()
                    .AddComponent(new TransformComponent(gridLocation))
                    .AddComponent(new SpriteComponent(ContentService.Instance.GetAsset<Texture2D>("Sprites\\Grid")))
                    .AddToWorld();

            for (int column = 0; column < COLUMN_COUNT; column++)
            {
                for (int row = 0; row < ROW_COUNT; row++)
                {
                    _world.CreateEntity()
                        .AddComponent(new TransformComponent(new Vector2(gridLocation.X + SQUARE_SIZE * column + SQUARE_PADDING, gridLocation.Y + SQUARE_SIZE * row + SQUARE_PADDING)))
                        .AddComponent(new JewelComponent(column, row, JewelKind.Red))
                        .AddToWorld();
                }
            }
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
