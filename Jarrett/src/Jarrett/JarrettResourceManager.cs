using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jarrett.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Jarrett
{
    class JarrettResourceManager : IResourceManager
    {
        ContentManager m_content;

        public JarrettResourceManager(ContentManager content)
        {
            m_content = content;
        }

        public void Initialize()
        {
            m_content.RootDirectory = "Content";
        }

        public void LoadContent()
        {
            // Eventually, we can preload our resources here
        }

        public void UnloadContent()
        {
            // We can unload our content here
        }

        public T Load<T>(string assetName)
        {
            return m_content.Load<T>(assetName);
        }
    }
}
