using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Components;
using Hiromi.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Acorn.Components;

namespace Acorn.Systems
{
    public class HudSystem : GameSystem
    {
        private int _winningPoints;
        private Dictionary<int, int> _scores;
        private Texture2D _emptyAcorn;
        private Texture2D _scoredAcorn;

        public HudSystem(int winningPoints)
        {
            _winningPoints = winningPoints;
            _scores = new Dictionary<int, int>();
        }

        protected override void OnInitialize()
        {
            this.MessageManager.AddListener<ScoreChangedMessage>(msg => OnScoreChanged((ScoreChangedMessage)msg));

            _emptyAcorn = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.EmptyAcorn);
            _scoredAcorn = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.Acorn);

            // Generate player one's score acorns
            for (int pointNumber = 0; pointNumber < _winningPoints; pointNumber++)
            {
                var obj = new GameObject(depth:-100);
                obj.AddComponent(new PositionComponent(new Vector2(0.05f, (0.07f * pointNumber) + 0.07f), _emptyAcorn.Width, _emptyAcorn.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
                obj.AddComponent(new SpriteComponent(_emptyAcorn));
                obj.AddComponent(new ScoreComponent(0, pointNumber));
                this.GameObjectManager.AddGameObject(obj);
            }

            // Generate player two's score acorns
            for (int pointNumber = 0; pointNumber < _winningPoints; pointNumber++)
            {
                var obj = new GameObject(depth: -100);
                obj.AddComponent(new PositionComponent(new Vector2(0.95f, (0.07f * pointNumber) + 0.07f), _emptyAcorn.Width, _emptyAcorn.Height, HorizontalAnchor.Center, VerticalAnchor.Center));
                obj.AddComponent(new SpriteComponent(_emptyAcorn));
                obj.AddComponent(new ScoreComponent(1, pointNumber));
                this.GameObjectManager.AddGameObject(obj);
            }
        }

        private void OnScoreChanged(ScoreChangedMessage msg)
        {
            _scores[msg.PlayerIndex] = msg.Score;

            foreach (var obj in this.GameObjects.Values)
            {
                var spriteComponent = obj.GetComponent<SpriteComponent>();
                var scoreComponent = obj.GetComponent<ScoreComponent>();

                if (scoreComponent.PlayerIndex == msg.PlayerIndex && scoreComponent.PointNumber < msg.Score)
                {
                    spriteComponent.Texture = _scoredAcorn;
                }
            }
        }

        protected override bool IsGameObjectForSystem(GameObject obj)
        {
            return obj.HasComponent<SpriteComponent>() && obj.HasComponent<ScoreComponent>();
        }
    }
}
