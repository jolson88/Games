using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Acorn.States
{
    public class GameOverState : GameState
    {
        private GeneralInputSystem _inputSystem;
        private int _winningPlayer;
        private GameObject _playButton;
        private GameObject _menuButton;

        public GameOverState(int winningPlayer)
        {
            _winningPlayer = winningPlayer;
        }

        protected override void OnInitialize()
        {
            _inputSystem = new GeneralInputSystem(this.MessageManager);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            _inputSystem.Update(gameTime);
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

            var title = new GameObject();
            string text = (_winningPlayer == 0) ? "Red Player Wins!!!" : "Blue Player Wins!!!";
            title.AddComponent(new PositionComponent(new Vector2(0.5f, 0.15f), 0, 0, HorizontalAnchor.Center));
            title.AddComponent(new LabelComponent(text, ContentService.Instance.GetAsset<SpriteFont>(AcornAssets.TitleText), new Color(30, 30, 30)));
            yield return title;

            var playButtonSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.PlayButton);
            var playButtonPressedSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.PlayButtonPressed);
            _playButton = new GameObject();
            _playButton.AddComponent(new PositionComponent(new Vector2(0.5f, 0.4f), playButtonSprite.Width, playButtonSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
            _playButton.AddComponent(new ButtonComponent(playButtonSprite, playButtonPressedSprite));
            yield return _playButton;

            var menuButtonSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.MenuButton);
            var menuButtonPressedSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.MenuButtonPressed);
            _menuButton = new GameObject();
            _menuButton.AddComponent(new PositionComponent(new Vector2(0.5f, 0.55f), menuButtonSprite.Width, menuButtonPressedSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
            _menuButton.AddComponent(new ButtonComponent(menuButtonSprite, menuButtonPressedSprite));
            yield return _menuButton;

            var squirrelAsset = (_winningPlayer == 0) ? AcornAssets.RedSquirrel : AcornAssets.BlueSquirrel;
            var squirrelPosition = (_winningPlayer == 0) ? new Vector2(0.25f, 0.96f) : new Vector2(0.75f, 0.96f);
            var squirrelSprite = ContentService.Instance.GetAsset<Texture2D>(squirrelAsset);
            var squirrel = new GameObject();
            squirrel.AddComponent(new PositionComponent(squirrelPosition, squirrelSprite.Width, squirrelSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Bottom));
            squirrel.AddComponent(new SpriteComponent(squirrelSprite));
            yield return squirrel;
        }

        protected override void RegisterMessageListeners()
        {
            this.MessageManager.AddListener<ButtonPressMessage>(msg => OnButtonPress((ButtonPressMessage)msg));
        }

        private void OnButtonPress(ButtonPressMessage msg)
        {
            if (msg.GameObjectId == _playButton.Id)
            {
                this.MessageManager.QueueMessage(new RequestChangeStateMessage(new PlayState()));
            }
            else if (msg.GameObjectId == _menuButton.Id)
            {
                this.MessageManager.QueueMessage(new RequestChangeStateMessage(new MenuState()));
            }
        }
    }
}
