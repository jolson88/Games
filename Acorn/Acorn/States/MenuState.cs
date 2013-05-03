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
    public class MenuState : GameState
    {
        protected override IEnumerable<IGameView> LoadGameViews()
        {
            yield return new MenuHumanView();
        }
        
        protected override IEnumerable<GameObject> LoadGameObjects()
        {
            var bgSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.Background);
            var bg = new GameObject();
            bg.AddComponent(new TransformationComponent(new Vector2(0f, 0f), GraphicsService.Instance.GraphicsDevice.Viewport.Width, GraphicsService.Instance.GraphicsDevice.Viewport.Height, transformedByCamera: false));
            bg.AddComponent(new SpriteComponent(bgSprite));
            yield return bg;

            var cloudSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.Cloud);
            var cloud = new GameObject("Cloud");
            cloud.AddComponent(new TransformationComponent(new Vector2(100, 890), cloudSprite.Width, cloudSprite.Height));
            cloud.AddComponent(new SpriteComponent(cloudSprite));
            cloud.AddComponent(new SimpleMovementComponent(new Vector2(-65f, 0f)));
            cloud.AddComponent(new ScreenWrappingComponent());
            yield return cloud;

            var titleSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.GameTitle);
            var title = new GameObject();
            title.AddComponent(new TransformationComponent(new Vector2(800, 810), titleSprite.Width, titleSprite.Height, HorizontalAnchor.Center));
            title.AddComponent(new SpriteComponent(titleSprite));
            yield return title;

            var playButtonSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.PlayButton);
            var playButtonPressedSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.PlayButtonPressed);
            var playButton = new GameObject("PlayButton");
            playButton.AddComponent(new TransformationComponent(new Vector2(800, 450), playButtonSprite.Width, playButtonSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
            playButton.AddComponent(new ButtonComponent(playButtonSprite, playButtonPressedSprite));
            yield return playButton;

            var detailsFont = ContentService.Instance.GetAsset<SpriteFont>(AcornAssets.DetailsText);
            var company = new GameObject();
            company.AddComponent(new TransformationComponent(new Vector2(1590, 10), 0, 0, HorizontalAnchor.Right, VerticalAnchor.Bottom));
            company.AddComponent(new LabelComponent("Owl X Games", detailsFont, Color.White));
            yield return company;
        }
    }
}
