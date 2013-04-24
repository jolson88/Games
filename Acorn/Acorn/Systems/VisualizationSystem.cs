using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hiromi;
using Hiromi.Components;

namespace Acorn.Systems
{
    public class VisualizationSystem : GameSystem
    {
        private LabelComponent _statusLabel;
        private DelayProcess _textDelay;

        protected override void OnInitialize()
        {
            this.MessageManager.AddListener<StartTurnMessage>(msg => OnStartTurn((StartTurnMessage)msg));
            this.MessageManager.AddListener<EndTurnMessage>(msg => OnEndTurn((EndTurnMessage)msg));

            _statusLabel = new LabelComponent(string.Empty, ContentService.Instance.GetAsset<SpriteFont>(AcornAssets.TitleText), new Color(30, 30, 30));
            var status = new GameObject();
            status.AddComponent(new PositionComponent(new Vector2(0.5f, 0.1f), 0, 0, HorizontalAnchor.Center, VerticalAnchor.Center));
            status.AddComponent(_statusLabel);
            this.GameObjectManager.AddGameObject(status);
        }

        private void OnStartTurn(StartTurnMessage msg)
        {
            string text = (msg.PlayerIndex == 0) ? "Red Player's Turn" : "Blue Player's Turn";
            _statusLabel.Text = text;

            _textDelay = new DelayProcess(TimeSpan.FromSeconds(2), new ActionProcess(() => _statusLabel.Text = string.Empty));
            this.ProcessManager.AttachProcess(_textDelay);
        }

        private void OnEndTurn(EndTurnMessage msg)
        {
            if (_textDelay != null && _textDelay.IsAlive)
            {
                this.ProcessManager.RemoveProcess(_textDelay);
            }
        }
    }
}
