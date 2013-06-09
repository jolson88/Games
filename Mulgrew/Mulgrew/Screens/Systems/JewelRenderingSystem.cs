using System;
using System.Collections.Generic;
using System.Linq;
using Hiromi;
using Hiromi.Entities;
using Hiromi.Entities.Components;
using Hiromi.Entities.Systems;
using Mulgrew.Screens.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mulgrew.Screens.Systems
{
    class JewelRenderingSystem : EntityProcessingSystem
    {
        private Camera _camera;
        private SpriteBatch _batch;
        private Dictionary<JewelKind, Texture2D> _jewelTextureLookup;

        public JewelRenderingSystem(Camera camera)
        {
            _camera = camera;
        }
        
        public override void Initialize()
        {
            _batch = new SpriteBatch(GraphicsService.Instance.GraphicsDevice);

            _jewelTextureLookup = new Dictionary<JewelKind, Texture2D>();
            _jewelTextureLookup.Add(JewelKind.Red, ContentService.Instance.GetAsset<Texture2D>("Sprites\\Jewel-Red"));
        }

        protected override void BeginDraw()
        {
            _batch.Begin(_camera);
        }

        protected override void DrawEntity(Entity entity)
        {
            var transform = entity.GetComponent<TransformComponent>();
            var jewel = entity.GetComponent<JewelComponent>();

            var jewelTexture = _jewelTextureLookup[jewel.Kind];
            _batch.Draw(jewelTexture, 
                new Rectangle((int)transform.Position.X, (int)transform.Position.Y, jewelTexture.Width, jewelTexture.Height), 
                Color.White);
        }

        protected override void EndDraw()
        {
            _batch.End();
        }

        protected override bool InterestedInEntity(Entity entity)
        {
            return entity.HasComponent<TransformComponent>() && entity.HasComponent<JewelComponent>();
        }
    }
}
