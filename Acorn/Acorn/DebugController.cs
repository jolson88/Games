using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Acorn
{
    public class DebugController
    {
        private Random _random;
        private HumanGameView _view;
        private ProcessManager _processManager;
        private MessageManager _messageManager;
        private bool _rotateCamera = false;
        private float _rotationSpeedPerSecond;
        private float _currentRotation = 0f;
        private bool _zoomCamera = false;
        private float _zoomSpeedPerSecond;
        private float _currentZoom = 1f;
        private Vector2 _currentTranslation = Vector2.Zero;
        private int _maxShakeDistance;

        public DebugController(HumanGameView view)
        {
            _view = view;
            _rotationSpeedPerSecond = 0.7f * (2f * (float)Math.PI);
            _zoomSpeedPerSecond = 0.4f;
            _maxShakeDistance = 30;

            _random = new Random();

            _processManager = new ProcessManager();
            _messageManager = view.MessageManager;
            _messageManager.AddListener<KeyDownMessage>(OnKeyDown);
            _messageManager.AddListener<KeyUpMessage>(OnKeyUp);
        }

        public void Update(GameTime gameTime)
        {
            _processManager.Update(gameTime);
            if (_zoomCamera)
            {
                _currentZoom += (_zoomSpeedPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds);
                _messageManager.QueueMessage(new ZoomCameraMessage(_currentZoom));
            }
            if (_rotateCamera)
            {
                _currentRotation += (_rotationSpeedPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds);
                _messageManager.QueueMessage(new RotateCameraMessage(_currentRotation));
            }
        }

        private void OnKeyDown(KeyDownMessage msg)
        {
            if (msg.Key == Keys.Space)
            {
                var process = new TweenProcess(TimeSpan.FromSeconds(1), interp =>
                {
                    var translation = new Vector2(_maxShakeDistance - (_maxShakeDistance * interp.Value), 0);
                    translation = Vector2.Transform(translation, Matrix.CreateRotationZ((float)(_random.NextDouble() * (2f * Math.PI))));
                    _messageManager.QueueMessage(new NudgeCameraMessage(translation));
                });
                process.AttachChild(new ActionProcess(() =>
                {
                    _messageManager.QueueMessage(new NudgeCameraMessage(Vector2.Zero));
                }));

                _processManager.AttachProcess(Process.BuildProcessChain(
                    new TweenProcess(TimeSpan.FromSeconds(1), interp =>
                    {
                        var translation = new Vector2(_maxShakeDistance - (_maxShakeDistance * interp.Value), 0);
                        translation = Vector2.Transform(translation, Matrix.CreateRotationZ((float)(_random.NextDouble() * (2f * Math.PI))));
                        _messageManager.QueueMessage(new NudgeCameraMessage(translation));
                    }),
                    new ActionProcess(() =>
                    {
                        _messageManager.QueueMessage(new NudgeCameraMessage(Vector2.Zero));
                    })));
            }

            if (msg.Key == Keys.F1)
            {
                _currentTranslation += new Vector2(-_maxShakeDistance, 0);
                _messageManager.QueueMessage(new NudgeCameraMessage(_currentTranslation));
            }
            else if (msg.Key == Keys.F2)
            {
                _currentTranslation += new Vector2(0, -_maxShakeDistance);
                _messageManager.QueueMessage(new NudgeCameraMessage(_currentTranslation));
            }
            else if (msg.Key == Keys.F3)
            {
                _currentTranslation += new Vector2(0, _maxShakeDistance);
                _messageManager.QueueMessage(new NudgeCameraMessage(_currentTranslation));
            }
            else if (msg.Key == Keys.F4)
            {
                _currentTranslation += new Vector2(_maxShakeDistance, 0);
                _messageManager.QueueMessage(new NudgeCameraMessage(_currentTranslation));
            }
            if (msg.Key == Keys.F5)
            {
                _zoomCamera = true;
                _zoomSpeedPerSecond = -(Math.Abs(_zoomSpeedPerSecond));
            }
            if (msg.Key == Keys.F6)
            {
                _zoomCamera = true;
                _zoomSpeedPerSecond = Math.Abs(_zoomSpeedPerSecond);
            }
            if (msg.Key == Keys.F9)
            {
                _rotateCamera = true;
            }
            if (msg.Key == Keys.F12)
            {
                _view.SceneGraph.DebugVisuals = !_view.SceneGraph.DebugVisuals;
            }
        }

        private void OnKeyUp(KeyUpMessage msg)
        {
            if (msg.Key == Keys.F5)
            {
                _zoomCamera = false;
                _currentZoom = 1f;
                _messageManager.QueueMessage(new ZoomCameraMessage(1f));
            }
            if (msg.Key == Keys.F6)
            {
                _zoomCamera = false;
                _currentZoom = 1f;
                _messageManager.QueueMessage(new ZoomCameraMessage(1f));
            }
            if (msg.Key == Keys.F9)
            {
                _rotateCamera = false;
                _currentRotation = 0f;
                _messageManager.QueueMessage(new RotateCameraMessage(0f));
            }
        }
    }
}
