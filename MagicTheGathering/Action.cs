﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTheGathering
{
    abstract class MagicCardAction : Ability
    {
        CardReference cause;
        
        public abstract void Run();
    }
}
