using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiromi.Entities;

namespace Mulgrew.Screens.Components
{
    class JewelComponent : IComponent
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public JewelKind Kind { get; set; }

        public JewelComponent(int column, int row, JewelKind kind)
        {
            this.Column = column;
            this.Row = row;
            this.Kind = kind;
        }
    }
}
