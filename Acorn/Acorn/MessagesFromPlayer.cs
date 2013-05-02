using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;

namespace Acorn
{
    /// <summary>
    /// Message sent to logic to request the selection of a card.
    /// </summary>
    public class CardSelectionRequestMessage : Message
    {
        public int CardIndex { get; set; }
        public int PlayerIndex { get; set; }

        public CardSelectionRequestMessage(int playerIndex, int cardIndex)
        {
            this.PlayerIndex = playerIndex;
            this.CardIndex = cardIndex;
        }

        public override string ToString()
        {
            return string.Format("[Player] Player {0} requested to selected Card {1}", this.PlayerIndex, this.CardIndex);
        }
    }

    public class CardShuffleRequestMessage : Message
    {
        public int PlayerIndex { get; set; }

        public CardShuffleRequestMessage(int playerIndex)
        {
            this.PlayerIndex = playerIndex;
        }
    }

    /// <summary>
    /// Message sent to logic to request an ending of the turn. This will keep any points accumulated so far.
    /// </summary>
    public class StopRequestMessage : Message
    {
        public int PlayerIndex { get; set; }

        public StopRequestMessage(int playerIndex)
        {
            this.PlayerIndex = playerIndex;
        }

        public override string ToString()
        {
            return string.Format("[Player] Player {0} requested to cash in points", this.PlayerIndex);
        }
    }

    public class EndTurnConfirmationMessage : Message
    {
        public int PlayerIndex { get; set; }

        public EndTurnConfirmationMessage(int playerIndex)
        {
            this.PlayerIndex = playerIndex;
        }
    }
}
