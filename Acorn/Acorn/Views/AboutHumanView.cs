using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Hiromi;
using Hiromi.Components;
using Acorn.States;
using Acorn.Components;

namespace Acorn.Views
{
    public class AboutHumanView : HumanGameView
    {
        private GameObject _menuButton;
        private GameObject _label;

        protected override void OnInitialize()
        {
            this.MessageManager.AddListener<GameObjectLoadedMessage>(OnNewGameObject);
            this.MessageManager.AddListener<ButtonPressMessage>(OnButtonPress);
        }

        public override void OnLoaded()
        {
            this.AnimateScreenOn();
        }

        private void OnNewGameObject(GameObjectLoadedMessage msg)
        {
            if (msg.GameObject.Tag.Equals("MenuButton"))
            {
                _menuButton = msg.GameObject;
            }
            else if (msg.GameObject.HasComponent<LabelComponent>())
            {
                _label = msg.GameObject;
            }
        }

        private void OnButtonPress(ButtonPressMessage msg)
        {
            if (msg.GameObjectId == _menuButton.Id)
            {
                var sound = ContentService.Instance.GetAsset<SoundEffect>(AcornAssets.ButtonSelect);
                this.MessageManager.TriggerMessage(new PlaySoundEffectMessage(sound, 0.6f));
                this.AnimateScreenOff(new MenuState());
            }
        }

        private void AnimateScreenOn()
        {
            var cloud = this.GameObjectManager.GetAllGameObjectsWithTag("Cloud").First();
            cloud.Transform.Position = new Vector2(this.SceneGraph.Camera.Bounds.Width, cloud.Transform.Position.Y);
        }

        private void AnimateScreenOff(GameState newState)
        {
            var cloud = this.GameObjectManager.GetAllGameObjectsWithTag("Cloud").First();
            cloud.RemoveComponent<ScreenWrappingComponent>();
            cloud.RemoveComponent<SimpleMovementComponent>();

            this.ProcessManager.AttachProcess(Process.BuildProcessChain(
                new TweenProcess(Easing.GetBackFunction(0.3), TimeSpan.FromSeconds(1), interp =>
                {
                    cloud.Transform.PositionOffset = new Vector2(-interp.Value * this.SceneGraph.Camera.Bounds.Width, 0);
                    _label.Transform.PositionOffset = new Vector2(-interp.Value * this.SceneGraph.Camera.Bounds.Width, 0);
                    _menuButton.Transform.PositionOffset = new Vector2(interp.Value * this.SceneGraph.Camera.Bounds.Width, 0);
                }),
                new ActionProcess(() =>
                {
                    this.MessageManager.QueueMessage(new RequestChangeStateMessage(newState));
                })));
        }
    }
}
