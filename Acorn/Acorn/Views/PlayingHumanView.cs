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
        private static int AVATAR_BOUNCE_HEIGHT = 60;

        private int _currentPlayer;
        private int[] _playerIndices;
        private List<PlayerController> _playerControllers;
        private DebugCameraController _cameraController;
        private List<GameObject> _cards;
        private int _selectedCardCount = 0;
        private Dictionary<int, GameObject> _playerAvatars;
        private Dictionary<int, List<ScoreComponent>> _scoreAcorns;
        private Random _random;

        // Multiple indices allow this one view to have multiple players play with it (local multiplayer)
        public PlayingHumanView(params int[] playerIndices)
        {
            _playerAvatars = new Dictionary<int, GameObject>();
            _scoreAcorns = new Dictionary<int, List<ScoreComponent>>();
            _playerControllers = new List<PlayerController>();
            _playerIndices = playerIndices;
            _cards = new List<GameObject>();
            _random = new Random();

            _scoreAcorns.Add(0, new List<ScoreComponent>());
            _scoreAcorns.Add(1, new List<ScoreComponent>());
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
            else if (msg.GameObject.HasComponent<ScoreComponent>())
            {
                var acorn = msg.GameObject.GetComponent<ScoreComponent>();
                _scoreAcorns[acorn.PlayerIndex].Add(acorn);
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
            AnimatePlayerOnscreen();
        }

        private void OnEndTurn(EndTurnMessage msg)
        {
            AnimatePlayerOffscreen();
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
            var screenHeight = GraphicsService.Instance.DesignedScreenSize.Y;
            this.MessageManager.TriggerMessage(new NudgeCameraMessage(new Vector2(0, screenHeight)));
            this.ProcessManager.AttachProcess(Process.BuildProcessChain(
                new TweenProcess(Easing.ConvertTo(EasingKind.EaseOut, Easing.GetElasticFunction(oscillations: 12, springiness: 20)), TimeSpan.FromSeconds(4.2), interp =>
                {
                    this.MessageManager.QueueMessage(new NudgeCameraMessage(new Vector2(0, screenHeight - (screenHeight * interp.Value))));
                }),
                new ActionProcess(() => {
                    // Now that we are animated in (crazy camera movements are done), we can enable screen wrapping again
                    var cloud = this.GameObjectManager.GetAllGameObjectsWithTag("Cloud").First();
                    var wrapping = cloud.GetComponent<ScreenWrappingComponent>();
                    wrapping.IsEnabled = true;
                })));
        }

        private void AnimatePlayerOnscreen()
        {
            var player = _playerAvatars[_currentPlayer];
            var transformation = player.GetComponent<TransformationComponent>();
            var avatar = player.GetComponent<PlayerAvatarComponent>();

            var bounceFunction = Easing.GetBounceFunction(4, 1.4);
            player.AddComponent(new MoveToComponent(avatar.OnscreenDestination,
                TimeSpan.FromSeconds(1.25),
                Easing.ConvertTo(EasingKind.EaseOut, Easing.GetSineFunction()),
                Easing.Reverse(bounceFunction),
                0,
                AVATAR_BOUNCE_HEIGHT));
        }

        private void AnimatePlayerOffscreen()
        {
            var player = _playerAvatars[_currentPlayer];
            var transformation = player.GetComponent<TransformationComponent>();
            var avatar = player.GetComponent<PlayerAvatarComponent>();

            var bounceFunction = Easing.GetBounceFunction(4, 1.2);
            var moveComponent = new MoveToComponent(avatar.OffscreenDestination, TimeSpan.FromSeconds(1.25), Easing.GetLinearFunction(), bounceFunction, 0, AVATAR_BOUNCE_HEIGHT);
            moveComponent.Removed += (sender, args) =>
            {
                transformation.Position = avatar.OffscreenDestination; // Set so bouncing offset doesn't leave us higher (top of bounce)
                this.MessageManager.QueueMessage(new EndTurnConfirmationMessage(_currentPlayer));
            };
            player.AddComponent(moveComponent);
        }

        private void CreateFallingAcorns(int acornCount)
        {
            var acornSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.Acorn);
            for (int i = 0; i < acornCount; i++)
            {
                // Random value between 0.2 and 0.8
                var randX = ((float)_random.NextDouble() * 960) + 320;

                var obj = new GameObject("FallenAcorn");
                obj.AddComponent(new TransformationComponent(new Vector2(randX, GraphicsService.Instance.DesignedScreenSize.Y + acornSprite.Height), acornSprite.Width, acornSprite.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
                obj.AddComponent(new SpriteComponent(acornSprite));
                obj.AddComponent(new MoveToComponent(new Vector2(randX, 40), TimeSpan.FromSeconds(1.0), Easing.GetLinearFunction(), Easing.ConvertTo(EasingKind.EaseOut, Easing.GetSineFunction())));
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
            var animationDuration = TimeSpan.FromSeconds(0.7);

            var fallenAcorns = this.GameObjectManager.GetAllGameObjectsWithTag("FallenAcorn").ToList();
            var acornsToScore = _scoreAcorns[_currentPlayer].Where(sc => !sc.IsOn).OrderBy(sc => sc.PointNumber).ToList();
            for (int i = 0; i < Math.Min(fallenAcorns.Count(), acornsToScore.Count()); i++)
            {
                var fallenAcorn = fallenAcorns[i];
                var acornToScore = acornsToScore[i];

                var moveComponent = new MoveToComponent(acornToScore.GameObject.Transform.Position, animationDuration, Easing.GetLinearFunction(), Easing.GetSineFunction());
                moveComponent.Removed += (sender, args) => {
                    acornToScore.IsOn = true;
                    this.GameObjectManager.RemoveGameObject(fallenAcorn);
                };
                fallenAcorn.AddComponent(moveComponent);
            }
        }
    }
}
