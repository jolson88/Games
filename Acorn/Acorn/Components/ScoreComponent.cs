using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Acorn.Components
{
    public class ScoreComponent : GameObjectComponent
    {
        public int PlayerIndex { get; set; }
        public int PointNumber { get; set; }

        private Texture2D _emptyAcorn;
        private Texture2D _scoredAcorn;

        public ScoreComponent(int playerIndex, int pointNumber)
        {
            this.PlayerIndex = playerIndex;
            this.PointNumber = pointNumber;
        }

        public override void Loaded()
        {
            this.GameObject.MessageManager.AddListener<ScoreChangedMessage>(msg => OnScoreChanged((ScoreChangedMessage)msg));

            _emptyAcorn = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.EmptyAcorn);
            _scoredAcorn = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.Acorn);
        }

        private void OnScoreChanged(ScoreChangedMessage msg)
        {
            var spriteComponent = this.GameObject.GetComponent<SpriteComponent>();
            if (this.PlayerIndex == msg.PlayerIndex && this.PointNumber < msg.Score)
            {
                spriteComponent.Texture = _scoredAcorn;
            }
        }
    }
}
