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
    public class ScoreComponent : GameObjectComponent
    {
        public int PlayerIndex { get; set; }
        public int PointNumber { get; set; }       

        public ScoreComponent(int playerIndex, int pointNumber)
        {
            this.PlayerIndex = playerIndex;
            this.PointNumber = pointNumber;
        }
    }
}
