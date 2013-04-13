using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Acorn.Hiromi;
using Acorn.Objects;

namespace Acorn.Screens
{
    public class PlayScreen : Screen
    {
        private AcornResourceManager _resourceManager;

        public PlayScreen(AcornResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        protected override Background InitializeBackground()
        {
            return _resourceManager.GetBackground();
        }

        protected override void OnLoad()
        {
            var cloud = new Cloud()
            {
                Sprite = _resourceManager.GetCloud(),
                Position = new Vector2(0.1f, 0.001f)
            };

            GameObjectService objectService = GameObjectService.Instance;
            objectService.AddGameObject(cloud);
        }
    }
}
