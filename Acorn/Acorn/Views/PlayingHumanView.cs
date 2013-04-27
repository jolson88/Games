using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Hiromi;

namespace Acorn.Views
{
    public class PlayingHumanView : HumanGameView
    {
        private GeneralInputSystem _inputSystem; // TODO: Push into controller once extracted from game component

        public PlayingHumanView(MessageManager messageManager, GameObjectManager gameObjectManager) : base(messageManager, gameObjectManager)
        {
            _inputSystem = new GeneralInputSystem(messageManager);

            // TODO: Listen for game events to play sounds
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            _inputSystem.Update(gameTime);
        }
    }
}
