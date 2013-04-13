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
        public Sprite Sprite { get { return _sprite; } set { _sprite = value; CalculateBounds(); } }
        public Vector2 Position { get { return _position; } set { _position = value; CalculateBounds(); } }
        public Rectangle Bounds { get; set; }
        
        protected ProcessManager ProcessManager { get; set; }

        private Sprite _sprite;
        private Vector2 _position;
        private Dictionary<Type, BehaviorComponent> _components;       

        protected GameObject()
        {
            _components = new Dictionary<Type, BehaviorComponent>();
            this.Position = Vector2.Zero;
            this.ProcessManager = new ProcessManager();
            this.Bounds = new Rectangle(0, 0, 0, 0);
            this.OnInitialize();
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

        private void CalculateBounds()
        {
            // Remember: position is in screen coordinates. Bounding box should be in pixel coordinate. 
            // Also remember to account for "center" offset of sprite if present.
            var xOffset = this.Sprite != null ? this.Sprite.Center.X : 0;
            var yOffset = this.Sprite != null ? this.Sprite.Center.Y : 0;
            var width = this.Sprite != null ? this.Sprite.Texture.Width : 0;
            var height = this.Sprite != null ? this.Sprite.Texture.Height : 0;
            this.Bounds = new Rectangle((int)(this.Position.X * GraphicsService.Instance.GraphicsDevice.Viewport.Width - xOffset), 
                (int)(this.Position.Y * GraphicsService.Instance.GraphicsDevice.Viewport.Height - yOffset), 
                width, 
                height);
        }

        protected virtual void OnInitialize() { }
    }
}
