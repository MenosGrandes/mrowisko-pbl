﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map
{                    
    /// <summary>
    /// Enum which tell us what kind of node we use.
    /// </summary>
    public enum NodeType
    {
        FullNode = 0,
        TopLeft = 1,
        TopRight = 2,
        BottomLeft = 3,
        BottomRight = 4
    }
}
