using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Snake
{
    class Board
    {
        Random random = new Random();
        Game game;
        Block[,] Matrix;
        public SnakeBlock Snake { get; private set; }
        public Point Direction { get; private set; }
        Dictionary<Type, Action<Game, Block>> Collisionables = new Dictionary<Type, Action<Game, Block>>();
        Dictionary<bool, Func<int, int, int[]>> MatrixFinderActions = new Dictionary<bool, Func<int, int, int[]>>();
        Dictionary<bool, Func<Point, Point>> ChangeSnakeDirectionActionSelector = new Dictionary<bool, Func<Point, Point>>();

        public Board(Game game)
        {
            this.game = game;

            DefineIfReplacements();

            InitializeBoard();
            InitializeSnake();
            GenerateFood();
            Direction = WorldConstants.GetPosibleMovements()[System.Windows.Forms.Keys.Up];
        }

        private void DefineIfReplacements()
        {
            Collisionables.Add(typeof(EmptyBlock), (gameAux, otherBlock) => { });
            Collisionables.Add(typeof(FoodBlock), (gameAux, otherBlock) =>
            {
                Console.WriteLine("Snake was on " + Snake + " when eated!");
                otherBlock.Collided(game);
                Snake.Collided(game);
                GenerateFood();
            });
            Collisionables.Add(typeof(SnakeBlock), (gameAux, otherBlock) =>
            {
                Console.WriteLine("Snake was on " + Snake + " when collisioned with " + otherBlock + "!");
                game.GameEnds();
            });

            MatrixFinderActions.Add(true, (x, y) => { return new int[] { x, y }; });
            //MatrixFinderActions.Add(false, (x, y) => { return new int[] { x, y }; });

            ChangeSnakeDirectionActionSelector.Add(true, (newDirection) => { return newDirection; });
            ChangeSnakeDirectionActionSelector.Add(false, (newDirection) => { return Direction; });
        }

        private void GenerateFood()
        {
            int posX = random.Next(Matrix.GetLength(0));
            int posY = random.Next(Matrix.GetLength(1));

            Matrix[posX, posY] = new FoodBlock(Matrix[posX, posY]);
            Console.WriteLine("New food created at " + Matrix[posX, posY]);
        }

        private void InitializeBoard()
        {
            int xSize = WorldConstants.GAME_DISPLAY_SIZE.X / WorldConstants.BLOCK_SIZE.X;
            int ySize = WorldConstants.GAME_DISPLAY_SIZE.Y / WorldConstants.BLOCK_SIZE.Y;

            Matrix = new Block[xSize, ySize];

            int auxX = WorldConstants.INITIAL_LOCATION.X;
            int auxY = WorldConstants.INITIAL_LOCATION.Y;

            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < ySize; j++)
                {
                    Matrix[i, j] = new EmptyBlock(WorldConstants.BLOCK_SIZE, new Point(auxX, auxY));
                    auxY += WorldConstants.BLOCK_SIZE.Y;
                }
                auxX += WorldConstants.BLOCK_SIZE.X;
                auxY = WorldConstants.INITIAL_LOCATION.Y;
            }
        }

        private void InitializeSnake()
        {
            int auxX = Matrix.GetLength(0) / 2;
            int auxY = Matrix.GetLength(1) / 2;

            Snake = new SnakeBlock(Matrix[auxX, auxY]);
            SnakeBlock Next = Snake;
            for (int i = 0; i < 6; i++)
            {
                auxY += i;
                Next.Next = new SnakeBlock(Matrix[auxX, auxY]);
                Next.Next.Previous = Next;
                Next = Next.Next;
            }
            Next.Next = new EmptySnakeBlock(Matrix[auxX, auxY + 1]);
            Next.Next.Previous = Next;

            AddToMatrix(Snake, new int[] { auxX, auxY });
        }

        public void MoveSnake()
        {
            SnakeCollided();
            Snake.Move(game, new Point(Snake.Location.X + Direction.X, Snake.Location.Y + Direction.Y));
        }

        private void SnakeCollided()
        {
            Point point = Utils.SumPoints(Snake.Location, Direction);
            Block NextBlock = GetBlockOnMatrix(point);
            try
            {
                Action<Game, Block> collisionAction;
                Collisionables.TryGetValue(NextBlock.GetType(), out collisionAction);
                collisionAction.Invoke(game, NextBlock);
            }
            catch
            {

            }
        }

        public Block[,] GetMatrix()
        {
            return Matrix;
        }

        public Block GetBlockOnMatrix(int X, int Y)
        {
            Block block = null;
            try
            {
                block = Matrix[X, Y];
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("There is no block in [" + X + "," + Y + "]");
            }
            return block;
        }

        public Block GetBlockOnMatrix(Point point)
        {
            return GetBlockOnMatrix(point.X / WorldConstants.BLOCK_SIZE.X, point.Y / WorldConstants.BLOCK_SIZE.Y);
        }

        public void ChangeSnakeDirection(Point NewDirection)
        {
            Block NextBlock = GetBlockOnMatrix(Utils.SumPoints(Snake.Location, NewDirection));
            Direction = ChangeSnakeDirectionActionSelector[!Snake.Next.Equals(NextBlock)].Invoke(NewDirection);
        }

        public void AddToMatrix(Block block)
        {
            int[] pos = GetPositionOnMatrix(block);
            AddToMatrix(block, pos);
        }

        public void AddToMatrix(Block block, int[] pos)
        {
            try
            {
                Matrix[pos[0], pos[1]] = block;
            }catch(IndexOutOfRangeException e)
            {
                game.GameEnds();
            }
        }

        public int[] GetPositionOnMatrix(Block block)
        {
            Func<int, int, int[]> funcPos = (a, b) => { return new int[] { a, b }; };
            int[] fakePos = new int[] { -1, -1 };
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    //pos = MatrixFinderActions[Matrix[i, j].Equals(block)].Invoke(i, j);
                    //bool boolResult = Matrix[i, j].Equals(block);
                    //MatrixFinderActions.TryGetValue(boolResult, out funcPos);
                    //pos = funcPos.Invoke(i, j);
                    try
                    {
                        return MatrixFinderActions[Matrix[i, j].Equals(block)].Invoke(i, j);
                    }
                    catch (KeyNotFoundException e)
                    {

                    }
                }
            }
            //throw new NullReferenceException("There is no block like block " + block + " in the matrix");
            return fakePos;
        }

        public void DestroyBlock(Block block)
        {
            Console.WriteLine(block + " destroyed!");
            int[] blockPos = GetPositionOnMatrix(block);
            Matrix[blockPos[0], blockPos[1]] = new EmptyBlock(block);
        }
    }
}
