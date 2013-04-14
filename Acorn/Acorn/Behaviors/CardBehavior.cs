using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Behaviors;
using Hiromi.Messaging;

namespace Acorn.Behaviors
{
    class CardBehavior : GameObjectBehavior
    {
        public int CardIndex { get; set; }

        public CardBehavior(int cardIndex)
        {
            this.CardIndex = cardIndex;
        }

        protected override void OnInitialize()
        {
            MessageService.Instance.AddListener<CardSelectedMessage>(msg => OnCardSelected((CardSelectedMessage)msg));
            MessageService.Instance.AddListener<CardsShuffledMessage>(msg => OnCardsShuffled());
        }

        private void OnCardSelected(CardSelectedMessage msg)
        {
            if (msg.CardIndex == this.CardIndex)
            {
                switch (msg.CardValue)
                {
                    case 0:
                        this.GameObject.Sprite = AcornResourceManager.GetCardZeroSprite();
                        break;

                    case 1:
                        this.GameObject.Sprite = AcornResourceManager.GetCardOneSprite();
                        break;

                    case 2:
                        this.GameObject.Sprite = AcornResourceManager.GetCardTwoSprite();
                        break;

                    default:
                        throw new Exception("Unknown card value!");
                }
            }
        }

        private void OnCardsShuffled()
        {
            this.GameObject.Sprite = AcornResourceManager.GetCardBackSprite();
        }
    }
}
