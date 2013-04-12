using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Acorn.Hiromi.Processing;

namespace Acorn.Hiromi
{
    public class GameObject
    {
        public int Id { get; set; }
        public Sprite Sprite { get; set; }
        public Vector2 Position { get; set; }

        protected ProcessManager ProcessManager { get; set; }
        private Dictionary<Type, BehaviorComponent> _components;       

        protected GameObject()
        {
            _components = new Dictionary<Type, BehaviorComponent>();
            this.Position = Vector2.Zero;
            this.ProcessManager = new ProcessManager();
        }

        public void AddComponent(BehaviorComponent component)
        {
            component.GameObject = this;
            _components.Add(component.GetType(), component);
        }

        public BehaviorComponent GetComponent<T>() where T : BehaviorComponent
        {
            return _components[typeof(T)];
        }

        public void Update(GameTime gameTime)
        {
            this.ProcessManager.Update(gameTime);
            foreach (var component in _components.Values)
            {
                component.Update(gameTime);
            }
        }
    }
}
