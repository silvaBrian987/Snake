using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Game
    {
        public delegate void GameEndHandler();
        public event GameEndHandler OnGameEnd;

        public Player Player { get; }
        public Board Board { get; }
        public Game(Player player)
        {
            Player = player;
            Board = new Board(this);
            Board.InitializeSnake();
        }

        internal void GameEnds()
        {
            OnGameEnd();
        }
    }
}
