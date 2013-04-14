using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hiromi;
using Hiromi.Behaviors;
using Hiromi.Messaging;
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
            GameObjectService objectService = GameObjectService.Instance;

            var cloud = new GameObject()
            {
                Sprite = AcornResourceManager.GetCloudSprite(),
                Position = new Vector2(0.2f, 0.1f)
            };
            cloud.AddBehavior(new MovementBehavior(new Vector2(-0.03f, 0)));
            cloud.AddBehavior(new WrapAroundScreenBehavior());
            objectService.AddGameObject(cloud);
            
            var stopButton = new GameObject()
            {
                Sprite = AcornResourceManager.GetStopButtonSprite(),
                Position = new Vector2(0.5f, 0.65f),
                Tag = "StopButton"
            };
            stopButton.AddBehavior(new CommonButtonBehavior(AcornResourceManager.GetStopButtonSprite(), AcornResourceManager.GetStopButtonPressedSprite()));
            objectService.AddGameObject(stopButton);

            for (int i = 0; i < 4; i++)
            {
                var card = new GameObject()
                {
                    Sprite = AcornResourceManager.GetCardBackSprite(),
                    Position = new Vector2(0.20f + (i * 0.165f), 0.25f),
                    Tag = "Card"
                };
                card.AddBehavior(new CommonButtonBehavior());
                card.AddBehavior(new CardBehavior(i));
                objectService.AddGameObject(card);
            }

            // TODO: Add initialization sequence to GameBehaviors. Spinning them up via a constructor is very fragile and can break if GOs are created in a different order.
            var logic = new GameObject();
            logic.AddBehavior(new GameLogicBehavior(cardCount: 4, winningPoints: 20));
            objectService.AddGameObject(logic);

            var playerOneController = new GameObject();
            playerOneController.AddBehavior(new PlayerControllerBehavior(0));
            objectService.AddGameObject(playerOneController);

            var playerTwoController = new GameObject();
            playerTwoController.AddBehavior(new PlayerControllerBehavior(1));
            objectService.AddGameObject(playerTwoController);

            MessageService.Instance.QueueMessage(new StartGameMessage());
        }
    }
}
