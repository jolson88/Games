﻿using System;
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
            var bgSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.Background);
            var bg = new GameObject();
            bg.AddComponent(new TransformationComponent(new Vector2(0, 0), GraphicsService.Instance.GraphicsDevice.Viewport.Width, GraphicsService.Instance.GraphicsDevice.Viewport.Height, transformedByCamera: false));
            bg.AddComponent(new BackgroundComponent(bgSprite));
            yield return bg;

            var cloudSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.Cloud);
            var cloud = new GameObject("Cloud");
            cloud.AddComponent(new TransformationComponent(new Vector2(100, 890), cloudSprite.Width, cloudSprite.Height));
            cloud.AddComponent(new SpriteComponent(cloudSprite));
            cloud.AddComponent(new SimpleMovementComponent(new Vector2(-65, 0)));
            // Don't enable screen wrapping until after screen is animated in or else wrapping gets in way of "off camera->on camera" transition
            cloud.AddComponent(new ScreenWrappingComponent(isEnabled: true)); // TODO: Add in disabling once screen transition is added
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
            var squirrel = new GameObject();
            squirrel.AddComponent(new TransformationComponent(squirrelPosition, squirrelSprite.Width, squirrelSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Bottom));
            squirrel.AddComponent(new SpriteComponent(squirrelSprite));
            yield return squirrel;
        }
    }
}
