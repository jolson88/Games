using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Acorn.Views;

namespace Acorn.States
{
    public class GameOverState : GameState
    {
        private int _winningPlayer;

        public GameOverState(int winningPlayer)
        {
            _winningPlayer = winningPlayer;
        }

        protected override IEnumerable<IGameView> LoadGameViews()
        {
            yield return new GameOverHumanView();
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
            var playButton = new GameObject("PlayButton");
            playButton.AddComponent(new PositionComponent(new Vector2(0.5f, 0.4f), playButtonSprite.Width, playButtonSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
            playButton.AddComponent(new ButtonComponent(playButtonSprite, playButtonPressedSprite));
            yield return playButton;

            var menuButtonSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.MenuButton);
            var menuButtonPressedSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.MenuButtonPressed);
            var menuButton = new GameObject("MenuButton");
            menuButton.AddComponent(new PositionComponent(new Vector2(0.5f, 0.55f), menuButtonSprite.Width, menuButtonPressedSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
            menuButton.AddComponent(new ButtonComponent(menuButtonSprite, menuButtonPressedSprite));
            yield return menuButton;

            var squirrelAsset = (_winningPlayer == 0) ? AcornAssets.RedSquirrel : AcornAssets.BlueSquirrel;
            var squirrelPosition = (_winningPlayer == 0) ? new Vector2(0.25f, 0.96f) : new Vector2(0.75f, 0.96f);
            var squirrelSprite = ContentService.Instance.GetAsset<Texture2D>(squirrelAsset);
            var squirrel = new GameObject();
            squirrel.AddComponent(new PositionComponent(squirrelPosition, squirrelSprite.Width, squirrelSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Bottom));
            squirrel.AddComponent(new SpriteComponent(squirrelSprite));
            yield return squirrel;
        }
    }
}
