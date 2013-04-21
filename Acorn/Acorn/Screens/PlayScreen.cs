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
using Acorn.Systems;

namespace Acorn.Screens
{
    public class PlayScreen : Screen
    {
        protected override List<GameSystem> LoadGameSystems()
        {
            var systems = new List<GameSystem>();
            systems.Add(new GameLogicSystem(4, 20));
            systems.Add(new ScreenWrappingSystem());
            systems.Add(new PlayerControlSystem(0));
            systems.Add(new PlayerControlSystem(1)); // While it may look weird for two, this could easily be a ComputerControlSystem for 2-player game
            systems.Add(new HudSystem(20));
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
                card.AddComponent(new PositionComponent(new Vector2(0.20f + (i * 0.165f), 0.20f), cardBackSprite.Width, cardBackSprite.Height));
                card.AddComponent(new SpriteComponent(cardBackSprite));
                card.AddComponent(new ButtonComponent());
                card.AddComponent(new CardComponent(i));
                objects.Add(card);
            }

            var stopButtonSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.StopButtonSprite);
            var stopButtonPressedSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.StopButtonPressedSprite);
            var stopButton = new GameObject() { Tag = "StopButton" };
            stopButton.AddComponent(new PositionComponent(new Vector2(0.5f, 0.7f), stopButtonSprite.Width, stopButtonSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
            stopButton.AddComponent(new SpriteComponent(stopButtonSprite));
            stopButton.AddComponent(new ButtonComponent(stopButtonPressedSprite, stopButtonSprite));
            objects.Add(stopButton);

            var scoreSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.ScoreMeter);
            var playerOneScoreMeter = new GameObject();
            playerOneScoreMeter.AddComponent(new PositionComponent(new Vector2(0.02f, 0.03f), scoreSprite.Width, scoreSprite.Height, HorizontalAnchor.Left, VerticalAnchor.Top));
            playerOneScoreMeter.AddComponent(new HudComponent(scoreSprite));
            playerOneScoreMeter.AddComponent(new ScoreComponent(0, new Color(225, 10, 10, 255)));
            objects.Add(playerOneScoreMeter);

            var playerTwoScoreMeter = new GameObject();
            playerTwoScoreMeter.AddComponent(new PositionComponent(new Vector2(0.98f, 0.03f), scoreSprite.Width, scoreSprite.Height, HorizontalAnchor.Right, VerticalAnchor.Top));
            playerTwoScoreMeter.AddComponent(new HudComponent(scoreSprite));
            playerTwoScoreMeter.AddComponent(new ScoreComponent(1, new Color(70, 140, 255, 255)));
            objects.Add(playerTwoScoreMeter);

            return objects;
        }

        protected override void RegisterMessageListeners()
        {
            this.MessageManager.AddListener<ScreenLoadedMessage>(msg => OnScreenLoaded((ScreenLoadedMessage)msg));
        }

        private void OnScreenLoaded(ScreenLoadedMessage msg)
        {
            if (msg.Screen == this)
            {
                this.MessageManager.QueueMessage(new GameStartedMessage());
            }
        }
    }
}
