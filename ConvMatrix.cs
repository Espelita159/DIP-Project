using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIP
{
    public class ConvMatrix
    {
        public int TopLeft = 1, TopMid = 1, TopRight = 1;
        public int MidLeft = 1, Pixel = 8, MidRight = 1;
        public int BottomLeft = 1, BottomMid = 1, BottomRight = 1;

        public int Factor = 16;  // This normalizes the output to avoid brightening
        public int Offset = 0;

        public void SetAll(int nVal)
        {
            TopLeft = TopMid = TopRight = MidLeft = Pixel = MidRight =
                      BottomLeft = BottomMid = BottomRight = nVal;
        }
    }
}
