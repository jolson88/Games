using Hiromi;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunPhysics.Screens
{
    class MainScreen : Screen
    {
        protected override void OnLoad()
        {
            var biohazard = new GameObject()
            {
                Sprite = new Sprite(ContentService.Instance.Content.Load<Texture2D>("Biohazard")),
                Position = new Vector2(0.2f, 0.1f),
            };

            GameObjectService.Instance.AddGameObject(biohazard);
        }
    }
}
