using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Components;
using Hiromi.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Acorn.Screens
{
    public class MenuScreen : Screen
    {
        private GameObject _playButton;

        protected override IEnumerable<GameSystem> LoadGameSystems()
        {
            yield return new GeneralInputSystem();
            yield return new BackgroundRenderingSystem();
            yield return new UISystem();
            yield return new ScreenWrappingSystem();
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

            var titleSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.GameTitle);
            var title = new GameObject();
            title.AddComponent(new PositionComponent(new Vector2(0.5f, 0.1f), titleSprite.Width, titleSprite.Height, HorizontalAnchor.Center));
            title.AddComponent(new SpriteComponent(titleSprite));
            yield return title;

            var playButtonSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.PlayButton);
            var playButtonPressedSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.PlayButtonPressed);
            _playButton = new GameObject();
            _playButton.AddComponent(new PositionComponent(new Vector2(0.5f, 0.5f), playButtonSprite.Width, playButtonSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
            _playButton.AddComponent(new ButtonComponent(playButtonSprite, playButtonPressedSprite));
            yield return _playButton;

            var detailsFont = ContentService.Instance.GetAsset<SpriteFont>(AcornAssets.DetailsText);
            var company = new GameObject();
            company.AddComponent(new PositionComponent(new Vector2(0.98f, 0.98f), 0, 0, HorizontalAnchor.Right, VerticalAnchor.Bottom));
            company.AddComponent(new LabelComponent("Coding Coda Games", detailsFont));
            yield return company;
        }

        protected override void RegisterMessageListeners()
        {
            this.MessageManager.AddListener<ButtonPressMessage>(msg => OnButtonPress((ButtonPressMessage)msg));
        }

        private void OnButtonPress(ButtonPressMessage msg)
        {
            if (msg.GameObjectId == _playButton.Id)
            {
                this.MessageManager.QueueMessage(new RequestLoadScreenMessage(new PlayScreen()));
            }
        }
    }
}
