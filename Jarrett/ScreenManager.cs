using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Jarrett
{
    public class ScreenManager
    {
        private GraphicsDevice _graphicsDevice;
        private ContentManager _content;
        private Screen _currentScreen;

        public void Initialize(GraphicsDevice device, ContentManager content)
        {
            _graphicsDevice = device;
            _content = content;
        }

        public void Update(GameTime gameTime)
        {
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
            _currentScreen.Load(_graphicsDevice, _content);
        }
    }
}
