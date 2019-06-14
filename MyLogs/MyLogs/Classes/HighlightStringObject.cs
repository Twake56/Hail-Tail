using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HailTail.Classes
{
    class HighlightStringObject
    {
        int ForegroundColor { get; set; } = 0x0;
        int BackgroundColor { get; set; } = 0x0;
        string HighlightString { get; set; } = null;
    }
}
