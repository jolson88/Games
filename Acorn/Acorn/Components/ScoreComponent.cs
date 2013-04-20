using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Acorn.Components
{
    public class ScoreComponent : IComponent
    {
        public int PlayerIndex { get; set; }
        public Texture2D Fill { get; set; }

        public ScoreComponent(int playerIndex, Color fillColor)
        {
            this.PlayerIndex = playerIndex;
            this.Fill = new Texture2D(GraphicsService.Instance.GraphicsDevice, 1, 1);
            this.Fill.SetData(new Color[] { fillColor });
        }
    }
}
