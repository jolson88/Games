using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Components;

namespace Acorn.Components
{
    public class CardComponent : IComponent
    {
        public int CardIndex { get; set; }

        public CardComponent(int cardIndex)
        {
            this.CardIndex = cardIndex;
        }
    }
}
