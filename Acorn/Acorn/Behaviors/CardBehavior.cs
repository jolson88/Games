using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acorn.Hiromi;
using Acorn.Hiromi.Behaviors;

namespace Acorn.Behaviors
{
    // TODO: Can likely remove this. Perhaps add a generic Properties lookup to GameObject.
    // So, during construction, go["CardIndex"] = 1 instead of go.AddBehavior(new CardBehavior(1))
    class CardBehavior : GameObjectBehavior
    {
        public int CardIndex { get; set; }

        public CardBehavior(int cardIndex)
        {
            this.CardIndex = cardIndex;
        }
    }
}
