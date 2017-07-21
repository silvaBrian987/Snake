using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Snake
{
    class SnakeBlock : Block
    {
        public SnakeBlock Next { get; set; }
        public SnakeBlock Previous { get; set; }

        protected SnakeBlock(Point Size, Point Location, System.Drawing.Color Color) : base(Size, Location, Color)
        {
            OnCollision += (game) =>
            {
                Eat(game);
            };
        }

        internal SnakeBlock(Point Size, Point Location) : this(Size, Location, System.Drawing.Color.Red)
        {
        }

        internal SnakeBlock(Block OtherBlock) : this(OtherBlock.Size, OtherBlock.Location)
        {

        }

        internal virtual void Eat(Game game)
        {
            Next.Eat(game);
        }

        internal virtual void Move(Game game, Point NewLocation)
        {
            Next.Move(game, Location);
            Location = NewLocation;
            game.Board.AddToMatrix(this);
        }

        public override string ToString()
        {
            string aux = "";
            aux += (Previous != null ? Previous.Location.ToString() : "{N,N}");
            aux += Location.ToString();
            aux += (Next != null ? Next.Location.ToString() : "{N,N}");
            return aux;
        }
    }
}
