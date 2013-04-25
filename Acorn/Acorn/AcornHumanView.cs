using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Hiromi;

namespace Acorn
{
    public class AcornHumanView : HumanGameView
    {
        private GeneralInputSystem _inputSystem; // TODO: Turn into controller

        public AcornHumanView(MessageManager messageManager, GameObjectManager gameObjectManager) : base(messageManager, gameObjectManager)
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
