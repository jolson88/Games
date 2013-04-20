using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hiromi;
using Hiromi.Components;
using Hiromi.Messaging;
using Hiromi.Systems;
using Acorn.Components;

namespace Acorn.Screens
{
    public class PlayScreen : Screen
    {
        public PlayScreen()
        {
            MessageService.Instance.AddListener<ScreenLoadedMessage>(msg => OnScreenLoaded((ScreenLoadedMessage)msg));
        }

        protected override List<GameSystem> LoadGameSystems()
        {
            var systems = new List<GameSystem>();
            systems.Add(new GeneralInputSystem());
            systems.Add(new UISystem());
            systems.Add(new BackgroundRenderingSystem());
            systems.Add(new SimplePhysicsSystem());
            systems.Add(new SpriteRendererSystem());
            systems.Add(new ScreenWrappingSystem());

            return systems;
        }

        protected override List<GameObject> LoadGameObjects()
        {
            var objects = new List<GameObject>();

            var bg = new GameObject();
            bg.AddComponent(new BackgroundComponent(ContentService.Instance.GetAsset<Texture2D>(AcornAssets.Background)));
            objects.Add(bg);

            var cloudSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.CloudSprite);
            var cloud = new GameObject();
            cloud.AddComponent(new PositionComponent(new Vector2(0.2f, 0.02f), cloudSprite.Width, cloudSprite.Height));
            cloud.AddComponent(new SpriteComponent(cloudSprite));
            cloud.AddComponent(new SimplePhysicsComponent(new Vector2(-0.03f, 0f)));
            cloud.AddComponent(new ScreenWrappingComponent());
            objects.Add(cloud);

            var cardBackSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.CardBack);
            for (int i = 0; i < 4; i++)
            {
                var card = new GameObject();
                card.AddComponent(new PositionComponent(new Vector2(0.20f + (i * 0.165f), 0.25f), cardBackSprite.Width, cardBackSprite.Height));
                card.AddComponent(new SpriteComponent(cardBackSprite));
                card.AddComponent(new ButtonComponent());
                card.AddComponent(new CardComponent(i));
                objects.Add(card);
            }

            var stopButtonSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.StopButtonSprite);
            var stopButtonPressedSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.StopButtonPressedSprite);
            var stopButton = new GameObject();
            stopButton.AddComponent(new PositionComponent(new Vector2(0.5f, 0.65f), stopButtonSprite.Width, stopButtonSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
            stopButton.AddComponent(new SpriteComponent(stopButtonSprite));
            stopButton.AddComponent(new ButtonComponent(stopButtonPressedSprite, stopButtonSprite));
            objects.Add(stopButton);

            return objects;
        }

        /*
        protected override void OnLoad()
        {
            GameObjectService objectService = GameObjectService.Instance;

            var logic = new GameObject();
            logic.AddBehavior(new GameLogicBehavior(cardCount: 4, winningPoints: 20));
            objectService.AddGameObject(logic);

            var playerOneController = new GameObject();
            playerOneController.AddBehavior(new PlayerControllerBehavior(0));
            objectService.AddGameObject(playerOneController);

            var playerTwoController = new GameObject();
            playerTwoController.AddBehavior(new PlayerControllerBehavior(1));
            objectService.AddGameObject(playerTwoController);

        }
        */

        private void OnScreenLoaded(ScreenLoadedMessage msg)
        {
            if (msg.LoadedScreen == this)
            {
                MessageService.Instance.QueueMessage(new GameStartedMessage());
            }
        }
    }
}
