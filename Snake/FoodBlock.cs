﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Snake
{
    class FoodBlock : Block
    {
        public FoodBlock(Point Size, Point Location) : base(Size, Location, Color.Orange)
        {
            OnCollision += (board) =>
            {
                board.Board.DestroyBlock(this);
            };
        }

        public FoodBlock(Block OtherBlock) : this(OtherBlock.Size, OtherBlock.Location)
        {

        }
    }
}
