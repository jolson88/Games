using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Hiromi;
using Hiromi.Components;
using Acorn;
using Acorn.Components;

namespace Acorn.Views
{
    public class PlayingHumanView : HumanGameView
    {
        private int[] _playerIndices;
        private List<PlayerController> _playerControllers;
        private DebugCameraController _cameraController;
        private List<GameObject> _cards;

        // Multiple indices allow this one view to have multiple players play with it (local multiplayer)
        public PlayingHumanView(params int[] playerIndices)
        {
            _playerControllers = new List<PlayerController>();
            _playerIndices = playerIndices;
            _cards = new List<GameObject>();
        }

        protected override void OnInitialize()
        {
            for (int i = 0; i < _playerIndices.Length; i++)
            {
                _playerControllers.Add(new PlayerController(_playerIndices[i], this.MessageManager));
            }

            _cameraController = new DebugCameraController(this.MessageManager);

            this.MessageManager.AddListener<NewGameObjectMessage>(OnNewGameObject);
            this.MessageManager.AddListener<CardSelectedMessage>(OnCardSelected);

            var screenHeight = GraphicsService.Instance.GraphicsDevice.Viewport.Height;
            this.MessageManager.TriggerMessage(new MoveCameraMessage(new Vector2(0, -screenHeight)));
            var fadeInProcess = new TweenProcess(TimeSpan.FromSeconds(1), percentage =>
            {
                this.MessageManager.QueueMessage(new MoveCameraMessage(new Vector2(0, -screenHeight + (screenHeight * percentage))));
            });
            this.ProcessManager.AttachProcess(fadeInProcess);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (var controller in _playerControllers)
            {
                controller.Update(gameTime);
            }
            _cameraController.Update(gameTime);
        }

        private void OnNewGameObject(NewGameObjectMessage msg)
        {
            if (msg.GameObject.HasComponent<CardComponent>())
            {
                _cards.Add(msg.GameObject);
            }
        }

        private void OnCardSelected(CardSelectedMessage msg)
        {
            if (msg.CardValue == 0)
            {
                var card = _cards.Where(go => go.GetComponent<CardComponent>().CardIndex == msg.CardIndex).First();
                card.AddComponent(new ShakeComponent(15, TimeSpan.FromSeconds(0.75)));
            }
            else
            {
                var card = _cards.Where(go => go.GetComponent<CardComponent>().CardIndex == msg.CardIndex).First();
                card.AddComponent(new SwellComponent(15, TimeSpan.FromSeconds(0.25)));
            }
        }
    }
}
