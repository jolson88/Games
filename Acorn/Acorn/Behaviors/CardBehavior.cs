using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acorn.Hiromi;
using Acorn.Hiromi.Behaviors;

namespace Acorn.Behaviors
{
    class CardBehavior : GameObjectBehavior
    {
        public int CardIndex { get; set; }

        public CardBehavior(int cardIndex)
        {
            this.CardIndex = cardIndex;
        }
    }
}
