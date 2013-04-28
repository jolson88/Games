using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Hiromi;
using Acorn;

namespace Acorn.Views
{
    public class PlayingHumanView : HumanGameView
    {
        private int[] _playerIndices;
        private List<PlayerController> _playerControllers;

        // Multiple indices allow this one view to have multiple players play with it (local multiplayer)
        public PlayingHumanView(params int[] playerIndices)
        {
            _playerControllers = new List<PlayerController>();
            _playerIndices = playerIndices;
        }

        protected override void OnInitialize()
        {
            for (int i = 0; i < _playerIndices.Length; i++)
            {
                _playerControllers.Add(new PlayerController(_playerIndices[i], this.MessageManager));
            }
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (var controller in _playerControllers)
            {
                controller.Update(gameTime);
            }
        }
    }
}
