using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;

namespace Acorn
{
    public enum EndTurnReason
    {
        LostPoints,
        WonPoints,
        WonGame
    }

    /// <summary>
    /// Message from game logic telling what card was selected and how much it is worth.
    /// </summary>
    public class CardSelectedMessage : Message
    {
        public int CardIndex { get; set; }
        public int CardValue { get; set; }
        public int PlayerIndex { get; set; }

        public CardSelectedMessage(int cardIndex, int cardValue, int playerIndex)
        {
            this.CardIndex = cardIndex;
            this.CardValue = cardValue;
            this.PlayerIndex = playerIndex;
        }

        public override string ToString()
        {
            return string.Format("[Logic] Card {0} selected and worth {1} points", this.CardIndex, this.CardValue);
        }
    }

    /// <summary>
    /// Message from game logic when cards have been shuffled and flipped back over.
    /// </summary>
    public class CardsShuffledMessage : Message
    {
        public override string ToString()
        {
            return "[Logic] Cards shuffled";
        }
    }

    /// <summary>
    /// Message from game logic at the end of a player's turn.
    /// </summary>
    public class EndTurnMessage : Message
    {
        public int PlayerIndex { get; set; }
        public EndTurnReason Reason { get; set; }

        public EndTurnMessage(int playerIndex, EndTurnReason reason)
        {
            this.PlayerIndex = playerIndex;
            this.Reason = reason;
        }

        public override string ToString()
        {
            return string.Format("[Logic] Player {0}'s turn is over", this.PlayerIndex);
        }
    }

    /// <summary>
    /// Message from game logic at the start of a player's turn.
    /// </summary>
    public class StartTurnMessage : Message
    {
        public int PlayerIndex { get; set; }

        public StartTurnMessage(int playerIndex)
        {
            this.PlayerIndex = playerIndex;
        }

        public override string ToString()
        {
            return string.Format("[Logic] Player {0}'s Turn", this.PlayerIndex);
        }
    }

    /// <summary>
    /// Message from game logic when a player's score has changed.
    /// </summary>
    public class ScoreChangedMessage : Message
    {
        public int PlayerIndex { get; set; }
        public int Score { get; set; }

        public ScoreChangedMessage(int playerIndex, int score)
        {
            this.PlayerIndex = playerIndex;
            this.Score = score;
        }

        public override string ToString()
        {
            return string.Format("[Logic] Player {0} now has {1} points", this.PlayerIndex, this.Score);
        }
    }

    /// <summary>
    /// Message from game logic when the game is over.
    /// </summary>
    public class GameOverMessage : Message
    {
        public int WinningPlayerIndex { get; set; }

        public GameOverMessage(int winningPlayerIndex)
        {
            this.WinningPlayerIndex = winningPlayerIndex;
        }

        public override string ToString()
        {
            return string.Format("[Logic] Player {0} has on the game", this.WinningPlayerIndex);
        }
    }

}
