using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hiromi;
using Hiromi.Components;
using Acorn.Components;
using Acorn.Views;

namespace Acorn.States
{
    public class PlayState : GameState
    {
        private static int CARD_NUMBER = 4;
        private static int WINNING_TOTAL = 10;

        private GameLogicSystem _logic;

        protected override void OnInitialize()
        {
            _logic = new GameLogicSystem(this.MessageManager, this.ProcessManager, CARD_NUMBER, WINNING_TOTAL);
        }

        protected override IEnumerable<IGameView> LoadGameViews()
        {
            yield return new PlayingHumanView(this.MessageManager, this.GameObjectManager);
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

            // HUD - Scoring Acorns
            // Generate player one's score acorns
            var emptyAcornSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.EmptyAcorn);
            for (int pointNumber = 0; pointNumber < WINNING_TOTAL; pointNumber++)
            {
                var obj = new GameObject(depth: -100);
                obj.AddComponent(new PositionComponent(new Vector2(0.05f, (0.07f * pointNumber) + 0.07f), emptyAcornSprite.Width, emptyAcornSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
                obj.AddComponent(new SpriteComponent(emptyAcornSprite));
                obj.AddComponent(new ScoreComponent(0, pointNumber));
                yield return obj;
            }

            // Generate player two's score acorns
            for (int pointNumber = 0; pointNumber < WINNING_TOTAL; pointNumber++)
            {
                var obj = new GameObject(depth: -100);
                obj.AddComponent(new PositionComponent(new Vector2(0.95f, (0.07f * pointNumber) + 0.07f), emptyAcornSprite.Width, emptyAcornSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
                obj.AddComponent(new SpriteComponent(emptyAcornSprite));
                obj.AddComponent(new ScoreComponent(1, pointNumber));
                yield return obj;
            }
            
            // HUD - Game Status
            var status = new GameObject(depth: -100);
            status.AddComponent(new PositionComponent(new Vector2(0.5f, 0.1f), 0, 0, HorizontalAnchor.Center, VerticalAnchor.Center));
            status.AddComponent(new LabelComponent(string.Empty, ContentService.Instance.GetAsset<SpriteFont>(AcornAssets.TitleText), new Color(30, 30, 30)));
            status.AddComponent(new GameStatusComponent());
            yield return status;

            // TODO: Turn into controllers on the PlayingHumanView
            var playerOneController = new GameObject();
            playerOneController.AddComponent(new PlayerControllerComponent(0));
            yield return playerOneController;

            var playerTwoController = new GameObject();
            playerTwoController.AddComponent(new PlayerControllerComponent(1));
            yield return playerTwoController;
        }

        protected override void RegisterMessageListeners()
        {
            this.MessageManager.AddListener<StateChangedMessage>(msg => OnStateChanged((StateChangedMessage)msg));
        }

        private void OnStateChanged(StateChangedMessage msg)
        {
            if (msg.State == this)
            {
                this.MessageManager.QueueMessage(new GameStartedMessage());
            }
        }
    }
}
