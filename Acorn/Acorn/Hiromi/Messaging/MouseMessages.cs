﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acorn.Hiromi.Messaging
{
    public class PointerEnterMessage : Message
    {
        public int GameObjectId { get; set; }
        public PointerEnterMessage(int gameObjectId) { this.GameObjectId = gameObjectId; }
    }

    public class PointerExitMessage : Message
    {
        public int GameObjectId { get; set; }
        public PointerExitMessage(int gameObjectId) { this.GameObjectId = gameObjectId; }
    }

    public class PointerPressMessage : Message
    {
        public int GameObjectId { get; set; }
        public PointerPressMessage(int gameObjectId) { this.GameObjectId = gameObjectId; }
    }

    public class PointerReleaseMessage : Message
    {
        public int GameObjectId { get; set; }
        public PointerReleaseMessage(int gameObjectId) { this.GameObjectId = gameObjectId; }
    }
}
