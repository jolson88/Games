using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acorn.Hiromi.Messaging
{
    public class OffScreenMessage : Message
    {
        public int GameObjectId { get; set; }

        public OffScreenMessage(int gameObjectId)
        {
            this.GameObjectId = gameObjectId;
        }
    }
}
