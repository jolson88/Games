using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hiromi;
using Hiromi.Components;
using Hiromi.Systems;
using Acorn.Components;
using Acorn.Systems;

namespace Acorn.Screens
{
    public class PlayScreen : Screen
    {
        private static int CARD_NUMBER = 4;
        private static int WINNING_TOTAL = 10;

        protected override IEnumerable<GameSystem> LoadGameSystems()
        {
            yield return new GeneralInputSystem();
            yield return new BackgroundRenderingSystem();
            yield return new SpriteRendererSystem();
            yield return new UISystem();
            yield return new GameLogicSystem(CARD_NUMBER, WINNING_TOTAL);
            yield return new ScreenWrappingSystem();
            yield return new PlayerControlSystem(0);
            yield return new PlayerControlSystem(1); // While it may look weird for two, this could easily be a ComputerControlSystem for 2-player game
            yield return new VisualizationSystem();
            yield return new HudSystem(WINNING_TOTAL);
        }

        protected override IEnumerable<GameObject> LoadGameObjects()
        {
            var bg = new GameObject();
            bg.AddComponent(new BackgroundComponent(ContentService.Instance.GetAsset<Texture2D>(AcornAssets.Background)));
            yield return bg;

            var cloudSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.Cloud);
            var cloud = new GameObject();
            cloud.AddComponent(new PositionComponent(new Vector2(0.2f, 0.02f), cloudSprite.Width, cloudSprite.Height));
            cloud.AddComponent(new SpriteComponent(cloudSprite));
            cloud.AddComponent(new SimpleMovementComponent(new Vector2(-0.03f, 0f)));
            cloud.AddComponent(new ScreenWrappingComponent());
            yield return cloud;

            var cardBackSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.CardBack);
            for (int i = 0; i < CARD_NUMBER; i++)
            {
                var card = new GameObject();
                card.AddComponent(new PositionComponent(new Vector2(0.20f + (i * 0.165f), 0.30f), cardBackSprite.Width, cardBackSprite.Height));
                card.AddComponent(new SpriteComponent(cardBackSprite));
                card.AddComponent(new CardComponent(i));
                yield return card;
            }

            var stopButtonSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.StopButton);
            var stopButtonPressedSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.StopButtonPressed);
            var stopButton = new GameObject() { Tag = "StopButton" };
            stopButton.AddComponent(new PositionComponent(new Vector2(0.5f, 0.7f), stopButtonSprite.Width, stopButtonSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
            stopButton.AddComponent(new ButtonComponent(stopButtonSprite, stopButtonPressedSprite));
            yield return stopButton;
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
