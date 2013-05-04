using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Hiromi;
using Hiromi.Components;
using Acorn.Components;
using Acorn.States;

namespace Acorn.Views
{
    public class MenuHumanView : HumanGameView
    {
        private GameObject _playButton;
        private GameObject _cloud;

        protected override void OnInitialize()
        {
            this.MessageManager.AddListener<GameObjectLoadedMessage>(OnNewGameObject);
            this.MessageManager.AddListener<ButtonPressMessage>(OnButtonPress);
        }

        private void OnNewGameObject(GameObjectLoadedMessage msg)
        {
            if (msg.GameObject.Tag.Equals("PlayButton"))
            {
                _playButton = msg.GameObject;
                _playButton.AddComponent(new SwellComponent(16, TimeSpan.FromSeconds(1), isRepeating: true));
            }
            else if (msg.GameObject.Tag.Equals("Cloud"))
            {
                _cloud = msg.GameObject;
            }
        }

        private void OnButtonPress(ButtonPressMessage msg)
        {
            if (msg.GameObjectId == _playButton.Id)
            {
                BuildTransitionAnimation();
            }
        }

        private void BuildTransitionAnimation()
        {
            _cloud.RemoveComponent<ScreenWrappingComponent>();
            _cloud.RemoveComponent<SimpleMovementComponent>();

            var _cloudTransformation = _cloud.GetComponent<TransformationComponent>();

            // Fade other stuff off the screen
            this.ProcessManager.AttachProcess(Process.BuildProcessChain(
                new TweenProcess(Easing.GetBackFunction(0.3), EasingKind.EaseIn, TimeSpan.FromSeconds(1), interp =>
                {
                    _cloudTransformation.PositionOffset = new Vector2(interp.Value, 0);
                    this.MessageManager.QueueMessage(new NudgeCameraMessage(new Vector2(interp.Value * GraphicsService.Instance.GraphicsDevice.Viewport.Width, 0)));
                }),
                new ActionProcess(() =>
                {
                    this.MessageManager.QueueMessage(new RequestChangeStateMessage(new PlayState()));
                })));
        }
    }
}
