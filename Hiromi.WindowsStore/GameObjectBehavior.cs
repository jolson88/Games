using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Hiromi
{
    public enum BehaviorState
    {
        Uninitialized = 0,
        Initialized
    }

    public class GameObjectBehavior
    {
        public GameObject GameObject { get; set; }
        private BehaviorState _state = BehaviorState.Uninitialized;

        public void Update(GameTime gameTime)
        {
            if (_state == BehaviorState.Uninitialized)
            {
                OnInitialize();
                _state = BehaviorState.Initialized;
            }

            OnUpdate(gameTime);
        }

        protected virtual void OnInitialize() { }
        protected virtual void OnUpdate(GameTime gameTime) { }
    }
}
