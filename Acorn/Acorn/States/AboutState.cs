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
    public class AboutState : GameState
    {
        protected override IEnumerable<IGameView> LoadGameViews()
        {
            yield return new AboutHumanView();
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
            string text = "Programming,\nGame Design,\nSound Effects\nBy Jason Olson (www.owlxgames.com)\n\n\nMusic\nby Matthew Pablo (www.matthewpablo.com)\n\nThanks to @clingermangw,@the_zman,@chrisgwilliams for the push.";
            title.AddComponent(new TransformationComponent(new Vector2(800, 880), 0, 0, HorizontalAnchor.Center, VerticalAnchor.Top));
            title.AddComponent(new LabelComponent(text, ContentService.Instance.GetAsset<SpriteFont>(AcornAssets.DetailsText), Color.Black));
            yield return title;

            var menuButtonSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.MenuButton);
            var menuButtonPressedSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.MenuButtonPressed);
            var menuButton = new GameObject("MenuButton");
            menuButton.AddComponent(new TransformationComponent(new Vector2(800, 150), menuButtonSprite.Width, menuButtonPressedSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Bottom));
            menuButton.AddComponent(new ButtonComponent(menuButtonSprite, menuButtonPressedSprite));
            yield return menuButton;
        }
    }
}
