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

        private AcornGameLogic _logic;

        protected override void OnInitialize()
        {
            _logic = new AcornGameLogic(this.MessageManager, this.ProcessManager, CARD_NUMBER, WINNING_TOTAL);
        }

        protected override IEnumerable<IGameView> LoadGameViews()
        {
            // Two human players
            yield return new PlayingHumanView(0, 1);
        }

        protected override IEnumerable<GameObject> LoadGameObjects()
        {
            var bgSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.Background);
            var bg = new GameObject();
            bg.AddComponent(new TransformationComponent(new Vector2(0f, 0f), GraphicsService.Instance.GraphicsDevice.Viewport.Width, GraphicsService.Instance.GraphicsDevice.Viewport.Height, transformedByCamera: false));
            bg.AddComponent(new SpriteComponent(bgSprite, SpriteKind.Background));
            yield return bg;

            var cloudSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.Cloud);
            var cloud = new GameObject();
            cloud.AddComponent(new TransformationComponent(new Vector2(1.0f, 0.02f), cloudSprite.Width, cloudSprite.Height, transformedByCamera: false));
            cloud.AddComponent(new SpriteComponent(cloudSprite, SpriteKind.Background));
            cloud.AddComponent(new SimpleMovementComponent(new Vector2(-0.03f, 0f)));
            cloud.AddComponent(new ScreenWrappingComponent());
            yield return cloud;

            var cardBackSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.CardBack);
            for (int i = 0; i < CARD_NUMBER; i++)
            {
                var card = new GameObject();
                card.AddComponent(new TransformationComponent(new Vector2(0.25f + (i * 0.165f), 0.40f), cardBackSprite.Width, cardBackSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
                card.AddComponent(new SpriteComponent(cardBackSprite));
                card.AddComponent(new CardComponent(i));
                yield return card;
            }

            var stopButtonSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.StopButton);
            var stopButtonPressedSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.StopButtonPressed);
            var stopButton = new GameObject() { Tag = "StopButton" };
            stopButton.AddComponent(new TransformationComponent(new Vector2(0.5f, 0.7f), stopButtonSprite.Width, stopButtonSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
            stopButton.AddComponent(new ButtonComponent(stopButtonSprite, stopButtonPressedSprite));
            yield return stopButton;

            // Generate Player Avatars
            var redSquirrelSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.RedSquirrel);
            var redSquirrel = new GameObject();
            redSquirrel.AddComponent(new TransformationComponent(new Vector2(-0.08f, 0.97f), redSquirrelSprite.Width, redSquirrelSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Bottom));
            redSquirrel.AddComponent(new SpriteComponent(redSquirrelSprite));
            redSquirrel.AddComponent(new PlayerAvatarComponent(0, new Vector2(0.18f, 0f)));
            yield return redSquirrel;

            var blueSquirrelSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.BlueSquirrel);
            var blueSquirrel = new GameObject();
            blueSquirrel.AddComponent(new TransformationComponent(new Vector2(1.08f, 0.97f), blueSquirrelSprite.Width, blueSquirrelSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Bottom));
            blueSquirrel.AddComponent(new SpriteComponent(blueSquirrelSprite));
            blueSquirrel.AddComponent(new PlayerAvatarComponent(1, new Vector2(-0.18f, 0f)));
            yield return blueSquirrel;

            // HUD - Scoring Acorns
            // Generate player one's score acorns
            var emptyAcornSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.EmptyAcorn);
            for (int pointNumber = 0; pointNumber < WINNING_TOTAL; pointNumber++)
            {
                var obj = new GameObject(depth: -100);
                obj.AddComponent(new TransformationComponent(new Vector2(0.05f, (0.07f * pointNumber) + 0.07f), emptyAcornSprite.Width, emptyAcornSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
                obj.AddComponent(new SpriteComponent(emptyAcornSprite));
                obj.AddComponent(new ScoreComponent(0, pointNumber + 1));
                yield return obj;
            }

            // Generate player two's score acorns
            for (int pointNumber = 0; pointNumber < WINNING_TOTAL; pointNumber++)
            {
                var obj = new GameObject(depth: -100);
                obj.AddComponent(new TransformationComponent(new Vector2(0.95f, (0.07f * pointNumber) + 0.07f), emptyAcornSprite.Width, emptyAcornSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
                obj.AddComponent(new SpriteComponent(emptyAcornSprite));
                obj.AddComponent(new ScoreComponent(1, pointNumber + 1));
                yield return obj;
            }
        }

        protected override void RegisterMessageListeners()
        {
            this.MessageManager.AddListener<StateChangedMessage>(OnStateChanged);
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
