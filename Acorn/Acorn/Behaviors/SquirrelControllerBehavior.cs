using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Hiromi.Messaging;
using Microsoft.Xna.Framework;

namespace Acorn.Behaviors
{
    public class SquirrelControllerBehavior : GameObjectBehavior
    {
        public int PlayerIndex { get; set; }
        public Vector2 IdlePosition { get; set; }

        public SquirrelControllerBehavior(int playerIndex, Vector2 idlePosition)
        {
            this.PlayerIndex = playerIndex;
            this.IdlePosition = idlePosition;
        }

        protected override void OnInitialize()
        {
            this.GameObject.IsVisible = false;
            this.GameObject.Position = this.IdlePosition;

            MessageService.Instance.AddListener<StartTurnMessage>(msg => OnStartTurn((StartTurnMessage)msg));
            MessageService.Instance.AddListener<EndTurnMessage>(msg => OnEndTurn((EndTurnMessage)msg));
        }

        private void OnStartTurn(StartTurnMessage msg)
        {
            if (msg.PlayerIndex == this.PlayerIndex)
            {
                this.GameObject.IsVisible = true;
            }
        }

        private void OnEndTurn(EndTurnMessage msg)
        {
            if (msg.PlayerIndex == this.PlayerIndex)
            {
                this.GameObject.IsVisible = false;
            }
        }
    }
}
