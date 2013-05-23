using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hiromi;
using Hiromi.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Acorn.Views;

namespace Acorn.States
{
    public class GameOverState : GameState
    {
        private PlaySettings _playSettings;
        private int _winningPlayer;

        public GameOverState(PlaySettings playSettings, int winningPlayer)
        {
            _playSettings = playSettings;
            _winningPlayer = winningPlayer;
        }

        public override GameState GetPreviousGameState()
        {
            return new PlayerSelectState();
        }

        protected override IEnumerable<IGameView> LoadGameViews()
        {
            yield return new GameOverHumanView(_playSettings);
        }

        protected override IEnumerable<GameObject> LoadGameObjects()
        {
            var bgSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.Background);
            var bg = new GameObject();
            bg.AddComponent(new TransformationComponent(new Vector2(0f, 0f), bgSprite.Width, bgSprite.Height, HorizontalAnchor.Left, VerticalAnchor.Bottom)
            {
                Z = -10
            });
            bg.AddComponent(new SpriteComponent(bgSprite));
            yield return bg;

            var cloudSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.Cloud);
            var cloud = new GameObject("Cloud");
            cloud.AddComponent(new TransformationComponent(new Vector2(100, 890), cloudSprite.Width, cloudSprite.Height));
            cloud.AddComponent(new SpriteComponent(cloudSprite));
            cloud.AddComponent(new SimpleMovementComponent(new Vector2(-65, 0)));
            cloud.AddComponent(new ScreenWrappingComponent());
            yield return cloud;

            var title = new GameObject();
            string text = (_winningPlayer == 0) ? "Red Player Wins!!!" : "Blue Player Wins!!!";
            title.AddComponent(new TransformationComponent(new Vector2(800, 765), 0, 0, HorizontalAnchor.Center, VerticalAnchor.Top));
            title.AddComponent(new LabelComponent(text, ContentService.Instance.GetAsset<SpriteFont>(AcornAssets.TitleText), new Color(30, 30, 30)));
            yield return title;

            var playButtonSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.PlayButton);
            var playButtonPressedSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.PlayButtonPressed);
            var playButton = new GameObject("PlayButton");
            playButton.AddComponent(new TransformationComponent(new Vector2(800, 540), playButtonSprite.Width, playButtonSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
            playButton.AddComponent(new ButtonComponent(playButtonSprite, playButtonPressedSprite));
            yield return playButton;

            var menuButtonSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.MenuButton);
            var menuButtonPressedSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.MenuButtonPressed);
            var menuButton = new GameObject("MenuButton");
            menuButton.AddComponent(new TransformationComponent(new Vector2(800, 400), menuButtonSprite.Width, menuButtonPressedSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
            menuButton.AddComponent(new ButtonComponent(menuButtonSprite, menuButtonPressedSprite));
            yield return menuButton;

            var squirrelAsset = (_winningPlayer == 0) ? AcornAssets.RedSquirrel : AcornAssets.BlueSquirrel;
            var squirrelPosition = (_winningPlayer == 0) ? new Vector2(125, 10) : new Vector2(1475, 10);
            var squirrelSprite = ContentService.Instance.GetAsset<Texture2D>(squirrelAsset);
            var squirrel = new GameObject("PlayerAvatar");
            squirrel.AddComponent(new TransformationComponent(squirrelPosition, squirrelSprite.Width, squirrelSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Bottom));
            squirrel.AddComponent(new SpriteComponent(squirrelSprite));
            yield return squirrel;
        }
    }
}
