using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Hiromi;
using Hiromi.Components;
using Acorn.Components;
using Acorn.States;

namespace Acorn.Views
{
    public class MenuHumanView : HumanGameView
    {
        private GameObject _playButton;
        private GameObject _aboutButton;
        private GameObject _cloud;
        private GameObject _title;

        protected override void OnInitialize()
        {
            this.MessageManager.AddListener<GameObjectLoadedMessage>(OnNewGameObject);
            this.MessageManager.AddListener<ButtonPressMessage>(OnButtonPress);

            var bgMusic = ContentService.Instance.GetAsset<Song>(AcornAssets.BackgroundMusic);
            this.MessageManager.TriggerMessage(new PlaySongMessage(bgMusic));
        }

        public override void OnLoaded()
        {
            this.AnimateScreenOn();
        }

        private void OnNewGameObject(GameObjectLoadedMessage msg)
        {
            if (msg.GameObject.Tag.Equals("PlayButton"))
            {
                _playButton = msg.GameObject;
                _playButton.AddComponent(new SwellComponent(16, TimeSpan.FromSeconds(1), isRepeating: true));
            }
            else if (msg.GameObject.Tag.Equals("AboutButton"))
            {
                _aboutButton = msg.GameObject;
            }
            else if (msg.GameObject.Tag.Equals("Cloud"))
            {
                _cloud = msg.GameObject;
            }
            else if (msg.GameObject.Tag.Equals("Title"))
            {
                _title = msg.GameObject;
            }
        }

        private void OnButtonPress(ButtonPressMessage msg)
        {
            if (msg.GameObjectId == _playButton.Id)
            {
                var buttonSound = ContentService.Instance.GetAsset<SoundEffect>(AcornAssets.ButtonSelect);
                this.MessageManager.TriggerMessage(new PlaySoundEffectMessage(buttonSound, 0.6f));
                AnimateScreenOff(new PlayState());
            }
            else if (msg.GameObjectId == _aboutButton.Id)
            {
                var buttonSound = ContentService.Instance.GetAsset<SoundEffect>(AcornAssets.ButtonSelect);
                this.MessageManager.TriggerMessage(new PlaySoundEffectMessage(buttonSound, 0.6f));
                AnimateScreenOff(new AboutState());
            }
        }

        private void AnimateScreenOn()
        {
            var labels = this.GameObjectManager.GetAllGameObjectsWithComponent<LabelComponent>();

            _cloud.Transform.Position = new Vector2(this.SceneGraph.Camera.Bounds.Width, _cloud.Transform.Position.Y);
            _title.Transform.PositionOffset = new Vector2(-this.SceneGraph.Camera.Bounds.Width, 0);
            _playButton.Transform.PositionOffset = new Vector2(this.SceneGraph.Camera.Bounds.Width, 0);
            _aboutButton.Transform.PositionOffset = new Vector2(this.SceneGraph.Camera.Bounds.Width, 0);

            var companyLabel = this.GameObjectManager.GetAllGameObjectsWithTag("Company").First();
            companyLabel.Transform.PositionOffset = new Vector2(this.SceneGraph.Camera.Bounds.Width, 0);

            this.ProcessManager.AttachProcess(new DelayProcess(TimeSpan.FromSeconds(0.4), 
                new TweenProcess(Easing.ConvertTo(EasingKind.EaseOut, Easing.GetPowerFunction(4)), TimeSpan.FromSeconds(1.75), interp =>
                {
                    _title.Transform.PositionOffset = new Vector2(-(1f - interp.Value) * this.SceneGraph.Camera.Bounds.Width, 0);
                    _playButton.Transform.PositionOffset = new Vector2((1f - interp.Value) * this.SceneGraph.Camera.Bounds.Width, 0);
                    _aboutButton.Transform.PositionOffset = new Vector2((1f - interp.Value) * this.SceneGraph.Camera.Bounds.Width, 0);
                    companyLabel.Transform.PositionOffset = new Vector2((1f - interp.Value) * this.SceneGraph.Camera.Bounds.Width, 0);
                })));
        }

        private void AnimateScreenOff(GameState newState)
        {
            _cloud.RemoveComponent<ScreenWrappingComponent>();
            _cloud.RemoveComponent<SimpleMovementComponent>();

            var labels = this.GameObjectManager.GetAllGameObjectsWithComponent<LabelComponent>();

            // Fade other stuff off the screen
            this.ProcessManager.AttachProcess(Process.BuildProcessChain(
                new TweenProcess(Easing.GetBackFunction(0.3), TimeSpan.FromSeconds(1), interp =>
                {
                    _cloud.Transform.PositionOffset = new Vector2(-interp.Value * this.SceneGraph.Camera.Bounds.Width, 0);
                    _title.Transform.PositionOffset = new Vector2(-interp.Value * this.SceneGraph.Camera.Bounds.Width, 0);
                    _playButton.Transform.PositionOffset = new Vector2(interp.Value * this.SceneGraph.Camera.Bounds.Width, 0);
                    _aboutButton.Transform.PositionOffset = new Vector2(interp.Value * this.SceneGraph.Camera.Bounds.Width, 0);
                    
                    foreach (var label in labels)
                    {
                        label.Transform.PositionOffset = new Vector2(interp.Value * this.SceneGraph.Camera.Bounds.Width, 0);
                    }
                }),
                new ActionProcess(() =>
                {
                    this.MessageManager.QueueMessage(new RequestChangeStateMessage(newState));
                })));
        }
    }
}
