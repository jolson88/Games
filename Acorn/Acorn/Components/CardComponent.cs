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
    public class CardComponent : GameObjectComponent
    {
        public int CardIndex { get; set; }
        public int CardValue { get; set; }

        private SpriteComponent _spriteComponent;
        private Dictionary<int, Texture2D> _cardSprites;

        public CardComponent(int cardIndex)
        {
            this.CardIndex = cardIndex;
            this.CardValue = -1;
        }

        protected override void OnLoaded()
        {
            this.GameObject.MessageManager.AddListener<CardsShuffledMessage>(OnCardsShuffled);
            this.GameObject.MessageManager.AddListener<CardSelectedMessage>(OnCardSelected);

            _spriteComponent = this.GameObject.GetComponent<SpriteComponent>();
            _cardSprites = new Dictionary<int, Texture2D>();
            _cardSprites.Add(0, ContentService.Instance.GetAsset<Texture2D>(AcornAssets.CardZero));
            _cardSprites.Add(1, ContentService.Instance.GetAsset<Texture2D>(AcornAssets.CardOne));
            _cardSprites.Add(2, ContentService.Instance.GetAsset<Texture2D>(AcornAssets.CardTwo));
        }

        private void OnCardSelected(CardSelectedMessage msg)
        {
            if (msg.CardIndex == this.CardIndex)
            {
                this.CardValue = msg.CardValue;
                _spriteComponent.Texture = _cardSprites[msg.CardValue];
            }
        }

        private void OnCardsShuffled(CardsShuffledMessage msg)
        {
            this.CardValue = -1;
            _spriteComponent.Texture = ContentService.Instance.GetAsset<Texture2D>(AcornAssets.CardBack);
        }
    }
}
