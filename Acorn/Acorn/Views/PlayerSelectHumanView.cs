using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Hiromi;
using Hiromi.Components;
using Acorn.Components;
using Acorn.States;

namespace Acorn.Views
{
    public class PlayerSelectHumanView : HumanGameView
    {
        private GameObject _playButton;
        private GameObject _redPlayerButton;
        private GameObject _bluePlayerButton;

        protected override void OnInitialize()
        {
            this.MessageManager.AddListener<GameObjectLoadedMessage>(OnNewGameObject);
            this.MessageManager.AddListener<ButtonPressMessage>(OnButtonPress);
            this.MessageManager.AddListener<PointerPressMessage>(OnPointerPress);

            var bgMusic = ContentService.Instance.GetAsset<Song>(AcornAssets.BackgroundMusic);
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
                _playButton.AddComponent(new SwellComponent(16, TimeSpan.FromSeconds(1), true));
            }
            else if (msg.GameObject.Tag.Equals("RedPlayer"))
            {
                _redPlayerButton = msg.GameObject;
                _redPlayerButton.AddComponent(new SwellComponent(10, TimeSpan.FromSeconds(1), true));
            }
            else if (msg.GameObject.Tag.Equals("BluePlayer"))
            {
                _bluePlayerButton = msg.GameObject;
                _bluePlayerButton.AddComponent(new SwellComponent(10, TimeSpan.FromSeconds(1), true));
            }
        }

        private void OnButtonPress(ButtonPressMessage msg)
        {
            if (msg.GameObjectId == _playButton.Id)
            {
                var buttonSound = ContentService.Instance.GetAsset<SoundEffect>(AcornAssets.ButtonSelect);
                this.MessageManager.TriggerMessage(new PlaySoundEffectMessage(buttonSound, PlatformConfiguration.SoundLevels.ButtonSelect));

                var playerOneKind = PlayerKind.Human;
                var playerTwoKind = PlayerKind.Human;
                if (_redPlayerButton.GetComponent<SpriteComponent>().Texture == ContentService.Instance.GetAsset<Texture2D>(AcornAssets.RedComputerButton))
                {
                    playerOneKind = PlayerKind.Computer;
                }
                if (_bluePlayerButton.GetComponent<SpriteComponent>().Texture == ContentService.Instance.GetAsset<Texture2D>(AcornAssets.BlueComputerButton))
                {
                    playerTwoKind = PlayerKind.Computer;
                }
                AnimateScreenOff(new PlayState(new PlaySettings(playerOneKind, playerTwoKind)));
            }
        }

        private void OnPointerPress(PointerPressMessage msg)
        {
            if (msg.GameObjectId == _redPlayerButton.Id)
            {
                this.MessageManager.TriggerMessage(new PlaySoundEffectMessage(ContentService.Instance.GetAsset<SoundEffect>(AcornAssets.ButtonSelect), PlatformConfiguration.SoundLevels.ButtonSelect));
                var sprite = _redPlayerButton.GetComponent<SpriteComponent>();
                if (sprite.Texture == ContentService.Instance.GetAsset<Texture2D>(AcornAssets.RedPlayerButton))
                {
                    sprite.Texture = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.RedComputerButton);
                }
                else
                {
                    sprite.Texture = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.RedPlayerButton);
                }
            }
            else if (msg.GameObjectId == _bluePlayerButton.Id)
            {
                this.MessageManager.TriggerMessage(new PlaySoundEffectMessage(ContentService.Instance.GetAsset<SoundEffect>(AcornAssets.ButtonSelect), PlatformConfiguration.SoundLevels.ButtonSelect));
                var sprite = _bluePlayerButton.GetComponent<SpriteComponent>();
                if (sprite.Texture == ContentService.Instance.GetAsset<Texture2D>(AcornAssets.BluePlayerButton))
                {
                    sprite.Texture = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.BlueComputerButton);
                }
                else
                {
                    sprite.Texture = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.BluePlayerButton);
                }
            }
        }

        private void AnimateScreenOn()
        {
            var labels = this.GameObjectManager.GetAllGameObjectsWithComponent<LabelComponent>();
            foreach (var label in labels)
            {
                label.Transform.PositionOffset = new Vector2(0, this.SceneGraph.Camera.Bounds.Height);
            }

            _playButton.Transform.PositionOffset = new Vector2(0, this.SceneGraph.Camera.Bounds.Height);
            _redPlayerButton.Transform.PositionOffset = new Vector2(-this.SceneGraph.Camera.Bounds.Width, 0);
            _bluePlayerButton.Transform.PositionOffset = new Vector2(this.SceneGraph.Camera.Bounds.Width, 0);

            this.ProcessManager.AttachProcess(new DelayProcess(TimeSpan.FromSeconds(0.4),
                new TweenProcess(Easing.ConvertTo(EasingKind.EaseOut, Easing.GetPowerFunction(4)), TimeSpan.FromSeconds(1.75), interp =>
                {
                    _playButton.Transform.PositionOffset = new Vector2(0, (1f - interp.Value) * this.SceneGraph.Camera.Bounds.Height);
                    _redPlayerButton.Transform.PositionOffset = new Vector2(-(1f - interp.Value) * this.SceneGraph.Camera.Bounds.Width, 0);
                    _bluePlayerButton.Transform.PositionOffset = new Vector2((1f - interp.Value) * this.SceneGraph.Camera.Bounds.Width, 0);

                    foreach (var label in labels)
                    {
                        label.Transform.PositionOffset = new Vector2(0, (1f - interp.Value) * this.SceneGraph.Camera.Bounds.Height);
                    }
                })));
        }

        private void AnimateScreenOff(GameState newState)
        {
            var labels = this.GameObjectManager.GetAllGameObjectsWithComponent<LabelComponent>();

            // Fade other stuff off the screen
            this.ProcessManager.AttachProcess(Process.BuildProcessChain(
                new TweenProcess(Easing.GetBackFunction(0.3), TimeSpan.FromSeconds(1), interp =>
                {
                    _playButton.Transform.PositionOffset = new Vector2(0, interp.Value * this.SceneGraph.Camera.Bounds.Height);
                    _redPlayerButton.Transform.PositionOffset = new Vector2(-interp.Value * this.SceneGraph.Camera.Bounds.Width, 0);
                    _bluePlayerButton.Transform.PositionOffset = new Vector2(interp.Value * this.SceneGraph.Camera.Bounds.Width, 0);

                    foreach (var label in labels)
                    {
                        label.Transform.PositionOffset = new Vector2(0, interp.Value * this.SceneGraph.Camera.Bounds.Height);
                    }
                }),
                new ActionProcess(() =>
                {
                    this.MessageManager.QueueMessage(new RequestChangeStateMessage(newState));
                })));
        }
    }
}
