using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hiromi;
using Hiromi.Components;
using Acorn;
using Acorn.Components;

namespace Acorn.Views
{
    public class PlayingHumanView : HumanGameView
    {
        private int _currentPlayer;
        private int[] _playerIndices;
        private List<PlayerController> _playerControllers;
        private DebugCameraController _cameraController;
        private List<GameObject> _cards;
        private int _selectedCardCount = 0;
        private Dictionary<int, GameObject> _playerAvatars;
        private Random _random;

        // Multiple indices allow this one view to have multiple players play with it (local multiplayer)
        public PlayingHumanView(params int[] playerIndices)
        {
            _playerAvatars = new Dictionary<int, GameObject>();
            _playerControllers = new List<PlayerController>();
            _playerIndices = playerIndices;
            _cards = new List<GameObject>();
            _random = new Random();
        }

        protected override void OnInitialize()
        {
            for (int i = 0; i < _playerIndices.Length; i++)
            {
                _playerControllers.Add(new PlayerController(_playerIndices[i], this.MessageManager));
            }

            _cameraController = new DebugCameraController(this.MessageManager);

            this.MessageManager.AddListener<GameObjectLoadedMessage>(OnNewGameObject);
            this.MessageManager.AddListener<CardSelectedMessage>(OnCardSelected);
            this.MessageManager.AddListener<KeyDownMessage>(OnKeyDown);
            this.MessageManager.AddListener<StartTurnMessage>(OnStartTurn);
            this.MessageManager.AddListener<EndTurnMessage>(OnEndTurn);

            this.AnimateScreenIn();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (var controller in _playerControllers)
            {
                controller.Update(gameTime);
            }
            _cameraController.Update(gameTime);
        }

        private void OnNewGameObject(GameObjectLoadedMessage msg)
        {
            if (msg.GameObject.HasComponent<CardComponent>())
            {
                _cards.Add(msg.GameObject);
            }
            else if (msg.GameObject.HasComponent<PlayerAvatarComponent>())
            {
                var avatar = msg.GameObject.GetComponent<PlayerAvatarComponent>();
                _playerAvatars.Add(avatar.PlayerIndex, msg.GameObject);
            }
        }

        private void OnCardSelected(CardSelectedMessage msg)
        {
            if (msg.CardValue == 0)
            {
                var card = _cards.Where(go => go.GetComponent<CardComponent>().CardIndex == msg.CardIndex).First();
                card.AddComponent(new ShakeComponent(15, TimeSpan.FromSeconds(1.25)));
                _selectedCardCount = 0;
            }
            else
            {
                _selectedCardCount++;
                var card = _cards.Where(go => go.GetComponent<CardComponent>().CardIndex == msg.CardIndex).First();
                card.AddComponent(new SwellComponent(15, TimeSpan.FromSeconds(0.25)));

                CreateFallingAcorns(msg.CardValue);
                if (_selectedCardCount == _cards.Count())
                {
                    // All cards selected, delay and send shuffle request
                    this.ProcessManager.AttachProcess(new DelayProcess(TimeSpan.FromSeconds(1.2), new ActionProcess(() =>
                    {
                        _selectedCardCount = 0;
                        this.MessageManager.QueueMessage(new CardShuffleRequestMessage(_currentPlayer));
                    })));
                }
            }
        }

        private void OnKeyDown(KeyDownMessage msg)
        {
            if (msg.Key == Microsoft.Xna.Framework.Input.Keys.Escape)
            {
                this.MessageManager.QueueMessage(new RequestChangeStateMessage(new States.MenuState()));
            }
        }

        private void OnStartTurn(StartTurnMessage msg)
        {
            _currentPlayer = msg.PlayerIndex;
            var player = _playerAvatars[msg.PlayerIndex];
            var transformation = player.GetComponent<TransformationComponent>();
            var avatar = player.GetComponent<PlayerAvatarComponent>();

            var bounceFunction = Easing.GetBounceFunction(4, 1.4);
            this.ProcessManager.AttachProcess(new TweenProcess(Easing.GetSineFunction(), EasingKind.EaseOut, TimeSpan.FromSeconds(1.25), interp =>
            {
                transformation.PositionOffset = (avatar.DestinationOffset * (interp.Value));
                transformation.PositionOffset = new Vector2(transformation.PositionOffset.X, -0.1f * (float)bounceFunction(1.0 - interp.Percentage));
            }));
        }

        private void OnEndTurn(EndTurnMessage msg)
        {
            var player = _playerAvatars[msg.PlayerIndex];
            var transformation = player.GetComponent<TransformationComponent>();
            var avatar = player.GetComponent<PlayerAvatarComponent>();

            var bounceFunction = Easing.GetBounceFunction(4, 1.2);
            this.ProcessManager.AttachProcess(Process.BuildProcessChain(
                new TweenProcess(Easing.GetLinearFunction(), EasingKind.EaseIn, TimeSpan.FromSeconds(1.25), interp =>
                {
                    transformation.PositionOffset = avatar.DestinationOffset * (1f - interp.Value);
                    transformation.PositionOffset = new Vector2(transformation.PositionOffset.X, -0.1f * (float)bounceFunction(interp.Percentage));
                }),
                new ActionProcess(() => {
                    this.MessageManager.QueueMessage(new EndTurnConfirmationMessage(_currentPlayer));   
                })));

            if (msg.Reason == EndTurnReason.LostPoints)
            {
                AnimateFallenAcornsDisappearing();
            }
            else
            {
                AnimateFallenAcornsScoring();
            }
        }

        private void AnimateScreenIn()
        {
            var screenHeight = GraphicsService.Instance.GraphicsDevice.Viewport.Height;
            this.MessageManager.TriggerMessage(new MoveCameraMessage(new Vector2(0, -screenHeight)));
            this.ProcessManager.AttachProcess(new TweenProcess(Easing.GetElasticFunction(oscillations: 12, springiness: 20), EasingKind.EaseOut, TimeSpan.FromSeconds(3.8), interp =>
            {
                this.MessageManager.QueueMessage(new MoveCameraMessage(new Vector2(0, -screenHeight + (screenHeight * interp.Value))));
            }));
        }

        private void CreateFallingAcorns(int acornCount)
        {
            var acornSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.Acorn);
            for (int i = 0; i < acornCount; i++)
            {
                // Random value between 0.2 and 0.8
                var randX = ((float)_random.NextDouble() * 0.6f) + 0.2f;

                var obj = new GameObject("FallenAcorn");
                obj.AddComponent(new TransformationComponent(new Vector2(randX, 0.98f), acornSprite.Width, acornSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Bottom));
                obj.AddComponent(new SpriteComponent(acornSprite));
                this.GameObjectManager.AddGameObject(obj);
                
            }
        }

        private void AnimateFallenAcornsDisappearing()
        {
            var acorns = this.GameObjectManager.GetAllGameObjectsWithTag("FallenAcorn");
            foreach (var acorn in acorns)
            {
                this.GameObjectManager.RemoveGameObject(acorn);
            }
        }

        private void AnimateFallenAcornsScoring()
        {
            // Temporary until we create special animation
            AnimateFallenAcornsDisappearing();
        }
    }
}
