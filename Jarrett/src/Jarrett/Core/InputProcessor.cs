using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Jarrett.Core
{
    class InputProcessor : Process
    {
        KeyboardState m_oldKeyState;

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            var newKeyState = Keyboard.GetState();
            CalculateKeyDownMessages(newKeyState);
            CalculateKeyUpMessages(newKeyState);
            m_oldKeyState = newKeyState;
        }

        private void CalculateKeyDownMessages(KeyboardState newKeyState)
        {
            var keys = newKeyState.GetPressedKeys().Where(key => !m_oldKeyState.IsKeyDown(key));
            foreach (var key in keys)
            {
                MessageBus.Get().QueueMessage(new KeyDownMessage(key));
            }
        }

        private void CalculateKeyUpMessages(KeyboardState newKeyState)
        {
            var keys = m_oldKeyState.GetPressedKeys().Where(key => !newKeyState.IsKeyDown(key));
            foreach (var key in keys)
            {
                MessageBus.Get().QueueMessage(new KeyUpMessage(key));
            }
        }
    }
}
