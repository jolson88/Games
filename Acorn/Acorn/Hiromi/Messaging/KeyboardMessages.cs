﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Acorn.Hiromi.Messaging
{
    public class KeyDownMessage : Message
    {
        public Keys Key { get; set; }
        public KeyDownMessage(Keys key) { this.Key = key; }
    }


    public class KeyUpMessage : Message
    {
        public Keys Key { get; set; }
        public KeyUpMessage(Keys key) { this.Key = key; }
    }

}