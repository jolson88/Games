using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Acorn.Hiromi;
using Acorn.Hiromi.Behaviors;
using Acorn.Behaviors;

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
            var cloud = new GameObject()
            {
                Sprite = AcornResourceManager.GetCloudSprite(),
                Position = new Vector2(0.2f, 0.1f)
            };
            cloud.AddBehavior(new MovementBehavior(new Vector2(-0.03f, 0)));
            cloud.AddBehavior(new WrapAroundScreenBehavior());

            var stopButton = new GameObject()
            {
                Sprite = AcornResourceManager.GetStopButtonSprite(),
                Position = new Vector2(0.5f, 0.65f)
            };
            stopButton.AddBehavior(new CommonButtonBehavior(AcornResourceManager.GetStopButtonSprite(), AcornResourceManager.GetStopButtonPressedSprite()));
            stopButton.AddBehavior(new StopButtonBehavior());

            GameObjectService objectService = GameObjectService.Instance;
            objectService.AddGameObject(cloud);
            objectService.AddGameObject(stopButton);
        }
    }
}
