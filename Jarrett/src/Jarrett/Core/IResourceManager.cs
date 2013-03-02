using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarrett.Core
{
    interface IResourceManager
    {
        void Initialize();
        void LoadContent();
        void UnloadContent();
        T Load<T>(string assetName);
    }
}
