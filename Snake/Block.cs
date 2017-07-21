using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Snake
{
    abstract class Block
    {
        public delegate void CollisionHandler(Game game);
        public event CollisionHandler OnCollision;

        public Point Size { get; }
        public Point Location { get;   set; }
        public System.Drawing.Color Color { get; }

        Dictionary<int, bool> CheckPositionCondicionalReplacer = new Dictionary<int, bool>();

        public Block(Point Size, Point Location, System.Drawing.Color Color)
        {
            this.Size = Size;
            this.Location = Location;
            this.Color = Color;

            OnCollision += (game) =>
            {

            };

            CheckPositionCondicionalReplacer.Add(0, true);
        }

        public override string ToString()
        {
            return GetType().Name + Location + Size;
        }

        //public override bool Equals(object obj)
        //{
        //    Block otherBlock = (Block)obj;
        //    //return (Location.X == otherBlock.Location.X && Location.Y == otherBlock.Location.Y);
        //    try
        //    {
        //        return CheckPositionCondicionalReplacer[Location.X - otherBlock.Location.X] && CheckPositionCondicionalReplacer[Location.Y - otherBlock.Location.Y];
        //    }
        //    catch (KeyNotFoundException e)
        //    {
        //        return false;
        //    }
        //}

        internal void Collided(Game game)
        {
            OnCollision(game);
        }
    }
}
