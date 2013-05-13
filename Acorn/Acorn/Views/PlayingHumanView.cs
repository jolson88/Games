using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Hiromi;
using Hiromi.Components;
using Acorn;
using Acorn.Components;
using Acorn.States;

namespace Acorn.Views
{
    public class PlayingHumanView : HumanGameView
    {
        private static int AVATAR_BOUNCE_HEIGHT = 60;

        private PlaySettings _playSettings;
        private int _currentPlayer;
        private List<IPlayerController> _playerControllers;
        private List<GameObject> _cards;
        private int _selectedCardCount = 0;
        private Dictionary<int, GameObject> _playerAvatars;
        private int[] _playerScores;
        private Dictionary<int, List<ScoreComponent>> _scoreAcorns;
        private Random _random;
        private float[] _acornRotations = new float[] { -1.5f, -1.2f, -0.9f, 0.9f, 1.2f, 1.5f };
        private SoundEffect[] _scoringSounds;
#if DEBUG
        private DebugController _cameraController;
#endif

        // Multiple indices allow this one view to have multiple players play with it (local multiplayer)
        public PlayingHumanView(PlaySettings playSettings)
        {
            _playSettings = playSettings;
            _playerAvatars = new Dictionary<int, GameObject>();
            _scoreAcorns = new Dictionary<int, List<ScoreComponent>>();
            _playerControllers = new List<IPlayerController>();
            _cards = new List<GameObject>();
            _random = new Random();

            _scoreAcorns.Add(0, new List<ScoreComponent>());
            _scoreAcorns.Add(1, new List<ScoreComponent>());

            _playerScores = new int[] { 0, 0 };
        }

        protected override void OnInitialize()
        {
            if (_playSettings.PlayerOneKind == PlayerKind.Human)
            {
                _playerControllers.Add(new HumanPlayerController(0, this.MessageManager));
            }
            else
            {
                _playerControllers.Add(new ComputerPlayerController(0, this.MessageManager));
            }

            if (_playSettings.PlayerTwoKind == PlayerKind.Human)
            {
                _playerControllers.Add(new HumanPlayerController(1, this.MessageManager));
            }
            else
            {
                _playerControllers.Add(new ComputerPlayerController(1, this.MessageManager));
            }

#if DEBUG
            _cameraController = new DebugController(this);
#endif

            this.MessageManager.AddListener<GameObjectLoadedMessage>(OnNewGameObject);
            this.MessageManager.AddListener<CardSelectedMessage>(OnCardSelected);
            this.MessageManager.AddListener<KeyDownMessage>(OnKeyDown);
            this.MessageManager.AddListener<StartTurnMessage>(OnStartTurn);
            this.MessageManager.AddListener<EndTurnMessage>(OnEndTurn);
            this.MessageManager.AddListener<ScoreChangedMessage>(OnScoreChanged);
            this.MessageManager.AddListener<StopRequestMessage>(OnStopRequest);
            this.MessageManager.AddListener<CardsShuffledMessage>(OnCardsShuffled);

            _scoringSounds = new SoundEffect[] {
                ContentService.Instance.GetAsset<SoundEffect>(AcornAssets.DingAcorn1),
                ContentService.Instance.GetAsset<SoundEffect>(AcornAssets.DingAcorn2),
                ContentService.Instance.GetAsset<SoundEffect>(AcornAssets.DingAcorn3),
                ContentService.Instance.GetAsset<SoundEffect>(AcornAssets.DingAcorn4),
                ContentService.Instance.GetAsset<SoundEffect>(AcornAssets.DingAcorn5),
                ContentService.Instance.GetAsset<SoundEffect>(AcornAssets.DingAcorn6),
                ContentService.Instance.GetAsset<SoundEffect>(AcornAssets.DingAcorn7),
                ContentService.Instance.GetAsset<SoundEffect>(AcornAssets.DingAcorn8)
            };
        }

        public override void OnLoaded()
        {
            this.AnimateScreenIn();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (var controller in _playerControllers)
            {
                controller.Update(gameTime);
            }
#if DEBUG
            _cameraController.Update(gameTime);
#endif
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

        private void OnStopRequest(StopRequestMessage msg)
        {
            var sound = ContentService.Instance.GetAsset<SoundEffect>(AcornAssets.ButtonSelect);
            this.MessageManager.TriggerMessage(new PlaySoundEffectMessage(sound, 0.6f));
        }

        private void OnCardSelected(CardSelectedMessage msg)
        {
            System.Diagnostics.Debug.WriteLine("VIEW: Existing selected card count = " + _selectedCardCount);

            if (msg.CardValue == 0)
            {
                var card = _cards.Where(go => go.GetComponent<CardComponent>().CardIndex == msg.CardIndex).First();
                card.AddComponent(new ShakeComponent(20, TimeSpan.FromSeconds(1.5)));
                this.MessageManager.QueueMessage(new PlaySoundEffectMessage(ContentService.Instance.GetAsset<SoundEffect>(AcornAssets.BuzzZeroCard), 0.06f));
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
                    this.ProcessManager.AttachProcess(new DelayProcess("Shuffle Delay", TimeSpan.FromSeconds(1.2), new ActionProcess(() =>
                    {
                        this.MessageManager.QueueMessage(new CardShuffleRequestMessage(_currentPlayer));
                    })));
                }
                this.MessageManager.QueueMessage(new PlaySoundEffectMessage(ContentService.Instance.GetAsset<SoundEffect>(AcornAssets.DingSelectCard), 0.2f));
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

        private void OnCardsShuffled(CardsShuffledMessage msg)
        {
            _selectedCardCount = 0;
        }

        private void OnEndTurn(EndTurnMessage msg)
        {
            _selectedCardCount = 0;
            if (msg.Reason != EndTurnReason.WonGame)
            {
                AnimatePlayerOffscreen();
            }

            if (msg.Reason == EndTurnReason.LostPoints)
            {
                AnimateFallenAcornsDisappearing();
            }
            else
            {
                AnimateFallenAcornsScoring(msg.Reason);
            }
        }

        private void OnScoreChanged(ScoreChangedMessage msg)
        {
            _playerScores[msg.PlayerIndex] = msg.Score;
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
            };
            player.AddComponent(moveComponent);
        }

        private void CreateFallingAcorns(int acornCount)
        {
            var acornSprite = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.Acorn);
            var randX = 0.0;
            var randDelay = TimeSpan.Zero;
            for (int i = 0; i < acornCount; i++)
            {
                // Random value between 0.2 and 0.8
                randX = (GetNextDouble(randX, 0.05) * 960.0) + 320.0;
                randDelay = TimeSpan.FromSeconds(_random.NextDouble() * 0.25);

                var newX = randX;
                var obj = new GameObject("FallenAcorn");
                obj.AddComponent(new TransformationComponent(new Vector2((float)newX, GraphicsService.Instance.DesignedScreenSize.Y + acornSprite.Height),
                                        acornSprite.Width,
                                        acornSprite.Height,
                                        HorizontalAnchor.Center,
                                        VerticalAnchor.Center)
                {
                    Rotation = _acornRotations[_random.Next(_acornRotations.Length)],
                    Z = 10
                });
                obj.AddComponent(new SpriteComponent(acornSprite));
                this.GameObjectManager.AddGameObject(obj);

                // Start falling after a random amount of time
                this.ProcessManager.AttachProcess(new DelayProcess("Random acorn falling delay", randDelay, new ActionProcess(() =>
                {                    
                    var moveTo = new MoveToComponent(new Vector2((float)newX, 40), TimeSpan.FromSeconds(0.8), Easing.GetLinearFunction(), Easing.ConvertTo(EasingKind.EaseOut, Easing.GetSineFunction()));
                    moveTo.Removed += (sender, args) =>
                    {
                        var sound = ContentService.Instance.GetAsset<SoundEffect>(AcornAssets.SoundOfAcornOnGround);
                        this.MessageManager.TriggerMessage(new PlaySoundEffectMessage(sound, 0.5f));
                    };
                    obj.AddComponent(moveTo);
                })));
                
            }
        }

        private void AnimateFallenAcornsDisappearing()
        {
            var acorns = this.GameObjectManager.GetAllGameObjectsWithTag("FallenAcorn");
            if (acorns.Count() > 0)
            {
                var spinRotationCount = 8;
                var zeroCard = _cards.Where(go => go.GetComponent<CardComponent>().CardValue == 0).First();
                this.ProcessManager.AttachProcess(Process.BuildProcessChain(
                    new TweenProcess("Acorn spinning", Easing.ConvertTo(EasingKind.EaseOut, Easing.GetSineFunction()), TimeSpan.FromSeconds(2.0), interp =>
                    {
                        zeroCard.Transform.Rotation = (float)(2 * Math.PI * spinRotationCount) * interp.Value;
                    }),
                    new ActionProcess("End turn confirmation", () =>
                    {
                        this.MessageManager.QueueMessage(new EndTurnConfirmationMessage(_currentPlayer));
                    })));

                foreach (var acorn in acorns)
                {
                    // Fade out as we get closer to zero card
                    this.ProcessManager.AttachProcess(new TweenProcess("Acorn fade out", Easing.GetPowerFunction(2), TimeSpan.FromSeconds(2.2), interp =>
                    {
                        acorn.GetComponent<SpriteComponent>().Alpha = (1.0f) - interp.Value;
                    }));

                    // Shake and fly acorn towards zero card
                    acorn.AddComponent(new ShakeComponent(15, TimeSpan.FromSeconds(1.4), shakeHarderAtEnd: true));
                    this.ProcessManager.AttachProcess(new DelayProcess("Delay while acorn shakes", TimeSpan.FromSeconds(0.7), new ActionProcess(() =>
                    {
                        this.ProcessManager.AttachProcess(new DelayProcess("Random acorn delay before flying", TimeSpan.FromSeconds(0.5 * _random.NextDouble()), new ActionProcess(() =>
                        {
                            var moveComponent = new MoveToComponent(zeroCard.Transform.Position, TimeSpan.FromSeconds(0.65), Easing.GetLinearFunction(), Easing.GetSineFunction());
                            moveComponent.Removed += (moveRemoveSender, moveRemoveArgs) =>
                            {
                                this.GameObjectManager.RemoveGameObject(acorn);
                            };
                            acorn.AddComponent(moveComponent);

                            var originalRotation = acorn.Transform.Rotation;
                            this.ProcessManager.AttachProcess(new TweenProcess("Acorn rotation", TimeSpan.FromSeconds(0.5), interp =>
                            {
                                acorn.Transform.Rotation = originalRotation - (originalRotation * interp.Value);
                            }));
                        })));
                    })));
                }
            }
            else
            {
                this.ProcessManager.AttachProcess(new DelayProcess("End turn confirmationd delay", TimeSpan.FromSeconds(2.0), new ActionProcess(() =>
                {
                    this.MessageManager.QueueMessage(new EndTurnConfirmationMessage(_currentPlayer));
                })));
            }
        }

        private void AnimateFallenAcornsScoring(EndTurnReason endTurnReason)
        {
            var animationDuration = TimeSpan.FromSeconds(0.6);

            var fallenAcorns = this.GameObjectManager.GetAllGameObjectsWithTag("FallenAcorn").ToList();
            var acornsToScore = _scoreAcorns[_currentPlayer].Where(sc => !sc.IsOn).OrderBy(sc => sc.PointNumber).ToList();
            var loopCount = Math.Min(fallenAcorns.Count(), acornsToScore.Count());

            // Did the player just say "stop" without flipping over cards?
            if (loopCount == 0)
            {
                this.MessageManager.QueueMessage(new EndTurnConfirmationMessage(_currentPlayer));
                return;
            }

            for (int i = 0; i < loopCount; i++)
            {
                var fallenAcorn = fallenAcorns[i];
                var acornToScore = acornsToScore[i];

                var currentScore = _playerScores[_currentPlayer];
                var scoreIndexOffset = i;
                var lastAcorn = (i == loopCount - 1);
                this.ProcessManager.AttachProcess(new DelayProcess("Acorn scoring delay per acorn", TimeSpan.FromSeconds(0.24 * i), new ActionProcess(() =>
                {
                    var moveComponent = new MoveToComponent(acornToScore.GameObject.Transform.Position, animationDuration, Easing.GetLinearFunction(), Easing.GetSineFunction());
                    moveComponent.Removed += (sender, args) =>
                    {
                        acornToScore.IsOn = true;
                        this.GameObjectManager.RemoveGameObject(fallenAcorn);
                        this.MessageManager.QueueMessage(new PlaySoundEffectMessage(_scoringSounds[currentScore + scoreIndexOffset], 0.32f));

                        if (lastAcorn)
                        {
                            if (endTurnReason == EndTurnReason.WonGame)
                            {
                                AnimateScreenOff();
                            }
                            else
                            {
                                this.MessageManager.QueueMessage(new EndTurnConfirmationMessage(_currentPlayer));
                            }
                        }
                    };
                    fallenAcorn.AddComponent(moveComponent);

                    var originalRotation = fallenAcorn.Transform.Rotation;
                    this.ProcessManager.AttachProcess(new TweenProcess("Acorn rotation", animationDuration, interp =>
                    {
                        fallenAcorn.Transform.Rotation = originalRotation - (originalRotation * interp.Value);
                    }));
                })));
            }
        }

        private double GetNextDouble(double previousValue, double minimumSpread)
        {
            double newValue = 0;
            do
            {
                newValue = _random.NextDouble();
            } while (Math.Abs(previousValue - newValue) <= minimumSpread);
            return newValue;
        }

        private void AnimateScreenIn()
        {
            var stopButton = this.GameObjectManager.GetAllGameObjectsWithTag("StopButton").First();
            var cloud = this.GameObjectManager.GetAllGameObjectsWithTag("Cloud").First();

            var screenWidth = GraphicsService.Instance.DesignedScreenSize.X;
            var screenHeight = GraphicsService.Instance.DesignedScreenSize.Y;

            stopButton.Transform.PositionOffset = new Vector2(0, screenHeight);
            foreach (var card in _cards)
            {
                card.Transform.PositionOffset = new Vector2(0, screenHeight);
            }
            foreach (var key in _scoreAcorns.Keys)
            {
                foreach (var acorn in _scoreAcorns[key])
                {
                    if (acorn.PlayerIndex == 0)
                    {
                        acorn.GameObject.Transform.PositionOffset = new Vector2(-screenWidth / 2, 0);
                    }
                    else
                    {
                        acorn.GameObject.Transform.PositionOffset = new Vector2(screenWidth / 2, 0);
                    }
                }
            }

            cloud.GetComponent<ScreenWrappingComponent>().IsEnabled = false;
            cloud.Transform.Position = new Vector2(GraphicsService.Instance.DesignedScreenSize.X, cloud.Transform.Position.Y);

            this.ProcessManager.AttachProcess(Process.BuildProcessChain(
                new TweenProcess(Easing.ConvertTo(EasingKind.EaseOut, Easing.GetPowerFunction(4)), TimeSpan.FromSeconds(1.25), interp =>
                {
                    stopButton.Transform.PositionOffset = new Vector2(0, screenHeight * (1f - interp.Value));
                    foreach (var card in _cards)
                    {
                        card.Transform.PositionOffset = new Vector2(0, screenHeight * (1f - interp.Value));
                    }

                    foreach (var key in _scoreAcorns.Keys)
                    {
                        foreach (var acorn in _scoreAcorns[key])
                        {
                            if (acorn.PlayerIndex == 0)
                            {
                                acorn.GameObject.Transform.PositionOffset = new Vector2((-screenWidth / 2) * (1f - interp.Value), 0);
                            }
                            else
                            {
                                acorn.GameObject.Transform.PositionOffset = new Vector2((screenWidth / 2) * (1f - interp.Value), 0);
                            }
                        }
                    }
                }),
                new ActionProcess(() =>
                {
                    cloud.GetComponent<ScreenWrappingComponent>().IsEnabled = true;
                    stopButton.Transform.PositionOffset = Vector2.Zero;
                    foreach (var card in _cards)
                    {
                        card.Transform.PositionOffset = Vector2.Zero;
                    }
                })));
        }

        private void AnimateScreenOff()
        {
            var screenWidth = GraphicsService.Instance.DesignedScreenSize.X;
            var screenHeight = GraphicsService.Instance.DesignedScreenSize.Y;

            var cloud = this.GameObjectManager.GetAllGameObjectsWithTag("Cloud").First();
            var stopButton = this.GameObjectManager.GetAllGameObjectsWithTag("StopButton").First();

            cloud.GetComponent<ScreenWrappingComponent>().IsEnabled = false;
            this.ProcessManager.AttachProcess(Process.BuildProcessChain(
                new TweenProcess(Easing.GetPowerFunction(2), TimeSpan.FromSeconds(1.4), interp =>
                {
                    foreach (var key in _scoreAcorns.Keys)
                    {
                        foreach (var acorn in _scoreAcorns[key])
                        {
                            acorn.GameObject.Transform.PositionOffset = new Vector2(0, screenHeight * interp.Value);
                        }
                    }

                    // Make acorns disappear twice as fast
                    var leftoverAcorns = this.GameObjectManager.GetAllGameObjectsWithTag("FallenAcorn");
                    foreach (var acorn in leftoverAcorns)
                    {
                        var interpVal = interp.Value * 2f;
                        if (interpVal < 1f)
                        {
                            acorn.GetComponent<SpriteComponent>().Alpha = (1.0f - interpVal);
                        }
                        else
                        {
                            this.GameObjectManager.RemoveGameObject(acorn);
                        }
                    }
                }),
                new ActionProcess(() =>
                {
                    this.MessageManager.QueueMessage(new EndTurnConfirmationMessage(_currentPlayer));
                })));

            this.ProcessManager.AttachProcess(new DelayProcess(TimeSpan.FromSeconds(0.25), new TweenProcess(Easing.GetBackFunction(0.3), TimeSpan.FromSeconds(1), interp =>
                {
                    stopButton.Transform.PositionOffset = new Vector2(interp.Value * screenWidth, 0);

                    cloud.Transform.PositionOffset = new Vector2(-interp.Value * screenWidth, 0);
                    foreach (var card in _cards)
                    {
                        card.Transform.PositionOffset = new Vector2(-interp.Value * screenWidth, 0);
                    }
                })));
        }
    }
}
