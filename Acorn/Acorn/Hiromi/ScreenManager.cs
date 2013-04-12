using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Acorn.Hiromi.Messaging;

namespace Acorn.Hiromi
{
    public class ScreenManager
    {
        private ContentManager _content;
        private Screen _currentScreen;

        public void Initialize(ContentManager content)
        {
            _content = content;
        }

        public void Update(GameTime gameTime)
        {
            MessageService.Instance.Update(gameTime);
            _currentScreen.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _currentScreen.Draw(gameTime);
        }

        // TODO: Once messages are integrated, remove this as a public method
        // (do it via sending a message)
        public void LoadScreen(Screen screen)
        {
            _currentScreen = screen;
            _currentScreen.Load(_content);
        }
    }
}
