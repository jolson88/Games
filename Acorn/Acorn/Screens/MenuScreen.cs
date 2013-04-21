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

        protected override List<GameSystem> LoadGameSystems()
        {
            var systems = new List<GameSystem>();
            systems.Add(new ScreenWrappingSystem());
            return systems;
        }

        protected override List<GameObject> LoadGameObjects()
        {
            var objects = new List<GameObject>();

            var bg = new GameObject();
            bg.AddComponent(new BackgroundComponent(ContentService.Instance.GetAsset<Texture2D>(AcornAssets.Background)));
            objects.Add(bg);

            var cloudSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.Cloud);
            var cloud = new GameObject();
            cloud.AddComponent(new PositionComponent(new Vector2(0.2f, 0.02f), cloudSprite.Width, cloudSprite.Height));
            cloud.AddComponent(new SpriteComponent(cloudSprite));
            cloud.AddComponent(new SimplePhysicsComponent(new Vector2(-0.03f, 0f)));
            cloud.AddComponent(new ScreenWrappingComponent());
            objects.Add(cloud);

            var title = new GameObject();
            title.AddComponent(new PositionComponent(new Microsoft.Xna.Framework.Vector2(0.5f, 0.15f), 0, 0, HorizontalAnchor.Center));
            title.AddComponent(new LabelComponent("Project Acorn", ContentService.Instance.GetAsset<SpriteFont>(AcornAssets.TitleText), Color.Black));
            objects.Add(title);

            var playButtonSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.PlayButton);
            var playButtonPressedSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.PlayButtonPressed);
            _playButton = new GameObject();
            _playButton.AddComponent(new PositionComponent(new Vector2(0.5f, 0.5f), playButtonSprite.Width, playButtonSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
            _playButton.AddComponent(new ButtonComponent(playButtonSprite, playButtonPressedSprite));
            objects.Add(_playButton);

            return objects;
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
