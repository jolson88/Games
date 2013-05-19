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
    public class PlayerSelectState : GameState
    {
        protected override IEnumerable<IGameView> LoadGameViews()
        {
            yield return new PlayerSelectHumanView();
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

            var title = new GameObject();
            string text = "VS.";
            title.AddComponent(new TransformationComponent(new Vector2(800, 650), 0, 0, HorizontalAnchor.Center, VerticalAnchor.Center));
            title.AddComponent(new LabelComponent(text, ContentService.Instance.GetAsset<SpriteFont>(AcornAssets.TitleText), new Color(30, 30, 30)));
            yield return title;

            var redPlayerButtonSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.RedPlayerButton);
            var redPlayerButton = new GameObject("RedPlayer");
            redPlayerButton.AddComponent(new TransformationComponent(new Vector2(300, 650), redPlayerButtonSprite.Width, redPlayerButtonSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
            redPlayerButton.AddComponent(new SpriteComponent(redPlayerButtonSprite));
            yield return redPlayerButton;

            var bluePlayerButtonSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.BlueComputerButton);
            var bluePlayerButton = new GameObject("BluePlayer");
            bluePlayerButton.AddComponent(new TransformationComponent(new Vector2(1300, 650), bluePlayerButtonSprite.Width, bluePlayerButtonSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
            bluePlayerButton.AddComponent(new SpriteComponent(bluePlayerButtonSprite));
            yield return bluePlayerButton;

            var playButtonSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.PlayButton);
            var playButtonPressedSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.PlayButtonPressed);
            var playButton = new GameObject("PlayButton");
            playButton.AddComponent(new TransformationComponent(new Vector2(800, 250), playButtonSprite.Width, playButtonSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
            playButton.AddComponent(new ButtonComponent(playButtonSprite, playButtonPressedSprite));
            yield return playButton;
        }
    }
}
