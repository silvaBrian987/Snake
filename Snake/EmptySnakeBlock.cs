using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class EmptySnakeBlock : SnakeBlock
    {
        public EmptySnakeBlock(Point Size, Point Location) : base(Size, Location, Color.Green)
        {

        }

        public EmptySnakeBlock(Block OtherBlock) : base(OtherBlock)
        {

        }

        internal override void Eat(Game game)
        {
            Console.WriteLine(this + " eated!");
            int[] pos = game.Board.GetPositionOnMatrix(Previous);

            Previous.Next = new SnakeBlock(this);
            Previous.Next.Next = this;
            Previous.Next.Previous = Previous;
            Previous = Previous.Next;

            game.Board.AddToMatrix(Previous, pos);
            game.Player.Points++;
        }

        internal override void Move(Game game, Point NewLocation)
        {
            Console.WriteLine(this + " moved to " + NewLocation);
            Location = NewLocation;
        }
    }
}
