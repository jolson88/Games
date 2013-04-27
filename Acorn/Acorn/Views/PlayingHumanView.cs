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

        protected override void OnInitialize()
        {
            _inputSystem = new GeneralInputSystem(this.MessageManager);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            _inputSystem.Update(gameTime);
        }
    }
}
