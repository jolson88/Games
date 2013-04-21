using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Components;
using Hiromi.Systems;
using Hiromi.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Acorn.Components;

namespace Acorn.Systems
{
    public class HudSystem : GameSystem
    {
        private int _winningPoints;
        private SpriteBatch _batch;
        private Dictionary<int, int> _scores;
        private Dictionary<int, GameObject> _scoreMeters;

        public HudSystem(int winningPoints)
        {
            _winningPoints = winningPoints;
            _batch = new SpriteBatch(GraphicsService.Instance.GraphicsDevice);
            _scores = new Dictionary<int, int>();
            _scoreMeters = new Dictionary<int, GameObject>();
        }

        protected override void OnInitialize()
        {
            this.MessageManager.AddListener<GameObjectLoadedMessage>(msg => OnGameObjectLoaded((GameObjectLoadedMessage)msg));
            this.MessageManager.AddListener<ScoreChangedMessage>(msg => OnScoreChanged((ScoreChangedMessage)msg));
        }

        private void OnGameObjectLoaded(GameObjectLoadedMessage msg)
        {
            if (msg.GameObject.HasComponent<ScoreComponent>())
            {
                var scoreComponent = msg.GameObject.GetComponent<ScoreComponent>();
                _scoreMeters.Add(scoreComponent.PlayerIndex, msg.GameObject);
                _scores.Add(scoreComponent.PlayerIndex, 0);
            }
        }

        private void OnScoreChanged(ScoreChangedMessage msg)
        {
            _scores[msg.PlayerIndex] = msg.Score;
        }

        protected override void OnDraw(GameTime gameTime)
        {
            _batch.Begin();

            // Draw score bars
            foreach (var obj in _scoreMeters.Values)
            {
                var posComponent = obj.GetComponent<PositionComponent>();
                var hudComponent = obj.GetComponent<HudComponent>();
                var scoreComponent = obj.GetComponent<ScoreComponent>();

                // Determine how filled the meter should be and draw the filler first
                var scorePercentage = (float)_scores[scoreComponent.PlayerIndex] / _winningPoints;
                scorePercentage = scorePercentage > 1.0f ? 1.0f : scorePercentage;
                var meterBounds = posComponent.Bounds.Deflate(10);
                meterBounds.Y = (meterBounds.Y + meterBounds.Height) - (meterBounds.Height * scorePercentage);
                meterBounds.Height = meterBounds.Height * scorePercentage;

                _batch.Draw(scoreComponent.Fill,
                    new Rectangle((int)(meterBounds.X * GraphicsService.Instance.GraphicsDevice.Viewport.Width),
                        (int)(meterBounds.Y * GraphicsService.Instance.GraphicsDevice.Viewport.Height),
                        (int)(meterBounds.Width * GraphicsService.Instance.GraphicsDevice.Viewport.Width),
                        (int)(meterBounds.Height * GraphicsService.Instance.GraphicsDevice.Viewport.Height)),
                    Color.White);

                // We use Bounds instead of Position as Bounds takes the achor point into account
                _batch.Draw(hudComponent.Texture,
                    new Vector2(posComponent.Bounds.X * GraphicsService.Instance.GraphicsDevice.Viewport.Width,
                        posComponent.Bounds.Y * GraphicsService.Instance.GraphicsDevice.Viewport.Height),
                    Color.White);
            }

            _batch.End();
        }
    }
}
