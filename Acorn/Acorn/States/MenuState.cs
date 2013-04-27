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
            yield return new MenuHumanView(this.MessageManager, this.GameObjectManager);
        }
        
        protected override IEnumerable<GameObject> LoadGameObjects()
        {
            var bg = new GameObject(depth: 1000);
            bg.AddComponent(new BackgroundComponent(ContentService.Instance.GetAsset<Texture2D>(AcornAssets.Background)));
            yield return bg;

            var cloudSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.Cloud);
            var cloud = new GameObject();
            cloud.AddComponent(new PositionComponent(new Vector2(0.2f, 0.02f), cloudSprite.Width, cloudSprite.Height));
            cloud.AddComponent(new SpriteComponent(cloudSprite));
            cloud.AddComponent(new SimpleMovementComponent(new Vector2(-0.03f, 0f)));
            cloud.AddComponent(new ScreenWrappingComponent());
            yield return cloud;

            var titleSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.GameTitle);
            var title = new GameObject();
            title.AddComponent(new PositionComponent(new Vector2(0.5f, 0.1f), titleSprite.Width, titleSprite.Height, HorizontalAnchor.Center));
            title.AddComponent(new SpriteComponent(titleSprite));
            yield return title;

            var playButtonSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.PlayButton);
            var playButtonPressedSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.PlayButtonPressed);
            var playButton = new GameObject("PlayButton");
            playButton.AddComponent(new PositionComponent(new Vector2(0.5f, 0.5f), playButtonSprite.Width, playButtonSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
            playButton.AddComponent(new ButtonComponent(playButtonSprite, playButtonPressedSprite));
            yield return playButton;

            var detailsFont = ContentService.Instance.GetAsset<SpriteFont>(AcornAssets.DetailsText);
            var company = new GameObject();
            company.AddComponent(new PositionComponent(new Vector2(0.98f, 0.98f), 0, 0, HorizontalAnchor.Right, VerticalAnchor.Bottom));
            company.AddComponent(new LabelComponent("Coding Coda Games", detailsFont));
            yield return company;
        }
    }
}
