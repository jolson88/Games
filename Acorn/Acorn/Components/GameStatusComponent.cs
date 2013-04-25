﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Hiromi;
using Hiromi.Components;

namespace Acorn.Components
{
    public class GameStatusComponent : GameObjectComponent
    {
        private DelayProcess _textDelay;
        private LabelComponent _statusLabel;

        public override void Loaded()
        {
            this.GameObject.MessageManager.AddListener<StartTurnMessage>(msg => OnStartTurn((StartTurnMessage)msg));
            this.GameObject.MessageManager.AddListener<EndTurnMessage>(msg => OnEndTurn((EndTurnMessage)msg));

            _statusLabel = this.GameObject.GetComponent<LabelComponent>();
        }

        private void OnStartTurn(StartTurnMessage msg)
        {
            string text = (msg.PlayerIndex == 0) ? "Red Player's Turn" : "Blue Player's Turn";
            _statusLabel.Text = text;

            _textDelay = new DelayProcess(TimeSpan.FromSeconds(2), new ActionProcess(() => _statusLabel.Text = string.Empty));
            this.GameObject.ProcessManager.AttachProcess(_textDelay);
        }

        private void OnEndTurn(EndTurnMessage msg)
        {
            if (_textDelay != null && _textDelay.IsAlive)
            {
                this.GameObject.ProcessManager.RemoveProcess(_textDelay);
            }
        }
    }
}