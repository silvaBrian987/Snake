using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Player
    {
        public string Name { get; set; }
        public int Points { get; set; }

        public Player(string name)
        {
            Name = name;
        }
    }
}
