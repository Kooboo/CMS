using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Drawing
{
    [Flags]
    public enum Corner
    {
        None = 0,
        All = 0xFF,
        TopLeft = 0x01,
        TopRight = 0x02,
        BottomLeft = 4,
        BottomRight = 8
    }
}
