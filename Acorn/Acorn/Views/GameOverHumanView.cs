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
    public class GameOverHumanView : HumanGameView
    {
        private PlaySettings _playSettings;
        private GameObject _playButton;
        private GameObject _menuButton;
        private GameObject _label;
        private GameObject _playerAvatar;
        private bool _jumping = true;

        public GameOverHumanView(PlaySettings playSettings)
        {
            _playSettings = playSettings;
        }

        protected override void OnInitialize()
        {
            this.MessageManager.AddListener<GameObjectLoadedMessage>(OnNewGameObject);
            this.MessageManager.AddListener<ButtonPressMessage>(OnButtonPress);
        }

        public override void OnLoaded()
        {
            this.CreateJumpingCycle();
            this.AnimateScreenOn();

            this.MessageManager.QueueMessage(new PlaySoundEffectMessage(ContentService.Instance.GetAsset<SoundEffect>(AcornAssets.Applause)));
        }

        private void CreateJumpingCycle()
        {
            this.ProcessManager.AttachProcess(Process.BuildProcessChain(
                new TweenProcess(Easing.GetSineFunction(), TimeSpan.FromSeconds(0.5), interp =>
                {
                    _playerAvatar.Transform.PositionOffset = new Vector2(0, 40f * interp.Value);
                }),
                new TweenProcess(Easing.ConvertTo(EasingKind.EaseOut, Easing.GetSineFunction()), TimeSpan.FromSeconds(0.5), interp =>
                {
                    _playerAvatar.Transform.PositionOffset = new Vector2(0, 40f * (1f - interp.Value));
                }),
                new ActionProcess(() =>
                {
                    if (_jumping)
                    {
                        CreateJumpingCycle();
                    }
                })));
        }

        private void OnNewGameObject(GameObjectLoadedMessage msg)
        {
            if (msg.GameObject.Tag.Equals("PlayButton"))
            {
                _playButton = msg.GameObject;
            }
            else if (msg.GameObject.Tag.Equals("MenuButton"))
            {
                _menuButton = msg.GameObject;
            }
            else if (msg.GameObject.HasComponent<LabelComponent>())
            {
                _label = msg.GameObject;
            }
            else if (msg.GameObject.Tag.Equals("PlayerAvatar"))
            {
                _playerAvatar = msg.GameObject;
            }
        }

        private void OnButtonPress(ButtonPressMessage msg)
        {
            if (msg.GameObjectId == _playButton.Id)
            {
                var sound = ContentService.Instance.GetAsset<SoundEffect>(AcornAssets.ButtonSelect);
                this.MessageManager.TriggerMessage(new PlaySoundEffectMessage(sound, 0.6f));
                this.AnimateScreenOff(new PlayState(_playSettings));
            }
            else if (msg.GameObjectId == _menuButton.Id)
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

            _label.Transform.PositionOffset = new Vector2(0, this.SceneGraph.Camera.Bounds.Height);
            _playButton.Transform.PositionOffset = new Vector2(0, this.SceneGraph.Camera.Bounds.Height);
            _menuButton.Transform.PositionOffset = new Vector2(0, this.SceneGraph.Camera.Bounds.Height);

            this.ProcessManager.AttachProcess(
                new TweenProcess(Easing.ConvertTo(EasingKind.EaseOut, Easing.GetPowerFunction(4)), TimeSpan.FromSeconds(1.1), interp =>
                {
                    _label.Transform.PositionOffset = new Vector2(0, (1f - interp.Value) * this.SceneGraph.Camera.Bounds.Height);
                    _playButton.Transform.PositionOffset = new Vector2(0, (1f - interp.Value) * this.SceneGraph.Camera.Bounds.Height);
                    _menuButton.Transform.PositionOffset = new Vector2(0, (1f - interp.Value) * this.SceneGraph.Camera.Bounds.Height);
                }));
        }

        private void AnimateScreenOff(GameState newState)
        {
            var cloud = this.GameObjectManager.GetAllGameObjectsWithTag("Cloud").First();
            cloud.RemoveComponent<ScreenWrappingComponent>();
            cloud.RemoveComponent<SimpleMovementComponent>();

            _jumping = false;
            this.ProcessManager.RemoveAllProcesses();

            var avatarDirection = (_playerAvatar.Transform.Position.X - (this.SceneGraph.Camera.Bounds.Width / 2) < 0) ? -1f : 1f;

            this.ProcessManager.AttachProcess(Process.BuildProcessChain(
                new TweenProcess(Easing.GetBackFunction(0.3), TimeSpan.FromSeconds(1), interp =>
                {
                    cloud.Transform.PositionOffset = new Vector2(-interp.Value * this.SceneGraph.Camera.Bounds.Width, 0);
                    _label.Transform.PositionOffset = new Vector2(-interp.Value * this.SceneGraph.Camera.Bounds.Width, 0);
                    _playButton.Transform.PositionOffset = new Vector2(interp.Value * this.SceneGraph.Camera.Bounds.Width, 0);
                    _menuButton.Transform.PositionOffset = new Vector2(interp.Value * this.SceneGraph.Camera.Bounds.Width, 0);
                    _playerAvatar.Transform.PositionOffset = new Vector2(interp.Value * avatarDirection * this.SceneGraph.Camera.Bounds.Width, 0);
                }),
                new ActionProcess(() =>
                {
                    this.MessageManager.QueueMessage(new RequestChangeStateMessage(newState));
                })));
        }
    }
}
