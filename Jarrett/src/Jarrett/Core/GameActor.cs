using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarrett.Core
{
    class GameActor
    {
        Dictionary<Type, GameComponent> m_components;

        public GameActor()
        {
            m_components = new Dictionary<Type, GameComponent>();
        }

        public void AddComponent(GameComponent component)
        {
            m_components.Add(component.GetType(), component);
        }

        public GameComponent GetComponent<T>() where T : GameComponent
        {
            return m_components[typeof(T)];
        }
    }
}
