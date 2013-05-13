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

        public bool IsOn
        {
            get
            {
                return _spriteComponent.Texture == _scoredAcorn;
            }
            set
            {
                if (value == true)
                {
                    _spriteComponent.Texture = _scoredAcorn;
                }
                else
                {
                    _spriteComponent.Texture = _emptyAcorn;
                }
            }
        }

        private SpriteComponent _spriteComponent;
        private Texture2D _emptyAcorn;
        private Texture2D _scoredAcorn;
        private int _playerScore;
        private int _tempScore;

        public ScoreComponent(int playerIndex, int pointNumber)
        {
            this.PlayerIndex = playerIndex;
            this.PointNumber = pointNumber;
            _playerScore = 0;
            _tempScore = 0;
        }

        protected override void OnLoaded()
        {
            _spriteComponent = this.GameObject.GetComponent<SpriteComponent>();

            this.GameObject.MessageManager.AddListener<ScoreChangedMessage>(OnScoreChanged);
            this.GameObject.MessageManager.AddListener<CardSelectedMessage>(OnCardSelected);
            this.GameObject.MessageManager.AddListener<EndTurnMessage>(OnEndTurn);

            _emptyAcorn = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.EmptyAcorn);
            _scoredAcorn = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.Acorn);
        }

        private void OnScoreChanged(ScoreChangedMessage msg)
        {
            if (this.PlayerIndex == msg.PlayerIndex)
            {
                _playerScore = msg.Score;
                _tempScore = _playerScore;

                if (this.GameObject.HasComponent<SwellComponent>())
                {
                    this.GameObject.RemoveComponent<SwellComponent>();
                }
            }
        }

        private void OnCardSelected(CardSelectedMessage msg)
        {
            if (msg.PlayerIndex == this.PlayerIndex)
            {
                _tempScore += msg.CardValue;
                if (this.PointNumber > _tempScore - msg.CardValue && this.PointNumber <= _tempScore)
                {
                    this.GameObject.AddComponent(new SwellComponent(10, TimeSpan.FromSeconds(2), true));
                }
            }
        }

        private void OnEndTurn(EndTurnMessage msg)
        {
            if (msg.PlayerIndex == this.PlayerIndex)
            {
                _tempScore = _playerScore;
                if (this.GameObject.HasComponent<SwellComponent>())
                {
                    this.GameObject.RemoveComponent<SwellComponent>();
                }
            }
        }
    }
}
