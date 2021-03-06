﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Snake
{
    class WorldConstants
    {
        public static Point GAME_DISPLAY_SIZE = new Point(800, 600);
        public static Point BLOCK_SIZE = new Point(10, 10);
        //public static Point INITIAL_LOCATION = new Point(10, 10);
        public static Point INITIAL_LOCATION = new Point(0, 0);

        public static System.Drawing.Size GetDisplaySize()
        {
            System.Drawing.Size display = new System.Drawing.Size();
            //display.Height = GAME_DISPLAY_SIZE.Y + (BLOCK_SIZE.Y * 2);
            //display.Width = GAME_DISPLAY_SIZE.X + (BLOCK_SIZE.X * 2);
            display.Height = GAME_DISPLAY_SIZE.Y;
            display.Width = GAME_DISPLAY_SIZE.X;
            return display;
        }

        public static Dictionary<Keys, Point> GetPosibleMovements()
        {
            Dictionary<Keys, Point> Movements = new Dictionary<Keys, Point>();

            Movements.Add(Keys.Up, new Point(0, -1));
            Movements.Add(Keys.Down, new Point(0, 1));
            Movements.Add(Keys.Right, new Point(1, 0));
            Movements.Add(Keys.Left, new Point(-1, 0));

            return Movements;
        }
    }
}
