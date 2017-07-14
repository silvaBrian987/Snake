using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Snake
{
    class EmptyBlock : Block
    {
        public EmptyBlock(Point Size, Point Location) : base (Size, Location, Color.Transparent)
        {
            
        }

        public EmptyBlock(Block OtherBlock) : this(OtherBlock.Size, OtherBlock.Location)
        {

        }
    }
}
