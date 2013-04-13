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
        protected override Background InitializeBackground()
        {
            return AcornResourceManager.GetBackground();
        }

        protected override void OnLoad()
        {
            var cloud = new Cloud()
            {
                Sprite = AcornResourceManager.GetCloudSprite(),
                Position = new Vector2(0.1f, 0.001f)
            };

            var stopButton = new StopButton()
            {
                Sprite = AcornResourceManager.GetStopButtonSprite(),
                Position = new Vector2(0.5f, 0.65f),
                NormalSprite = AcornResourceManager.GetStopButtonSprite(),
                HoverSprite = AcornResourceManager.GetStopButtonPressedSprite()
            };

            GameObjectService objectService = GameObjectService.Instance;
            objectService.AddGameObject(cloud);
            objectService.AddGameObject(stopButton);
        }
    }
}
