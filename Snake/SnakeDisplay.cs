using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake.Implementation
{
    public partial class SnakeDisplay : Form
    {
        Game game;
        Dictionary<Keys, Point> PosibleMovements;

        BufferedPanel SnakePanel;

        public SnakeDisplay()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            PosibleMovements = WorldConstants.GetPosibleMovements();

            SnakePanel = new BufferedPanel();
            //SnakePanel.Location = new Point(10, 10);
            //SnakePanel.Padding = new Padding(10, 10, 10, 10);
            //SnakePanel.Size = WorldConstants.GetDisplaySize();
            this.Size = WorldConstants.GetDisplaySize();
            this.Size = new Size(this.Size.Width + 100, this.Size.Height + 100);
            SnakePanel.BackColor = Color.Black;
            SnakePanel.Dock = DockStyle.Fill;

            Player player = new Player("Pepe");
            game = new Game(player);
            game.OnGameEnd += OnGameEnd;

            SnakePanel.Paint += DrawGame;

            Controls.Add(SnakePanel);
        }

        private void SnakeDisplay_Load(object sender, EventArgs e)
        {
            SnakeTimer.Tick += (x, y) =>
            {
                game.Board.MoveSnake();
                lblPuntos.Text = game.Player.Points.ToString();
                SnakePanel.Invalidate();
            };

            SnakeTimer.Interval = 10;
            SnakeTimer.Enabled = true;

            //RunBGMusic();
        }

        private void RunBGMusic()
        {
            System.IO.Stream str = Properties.Resources.techno;
            System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
            snd.PlayLooping();
        }

        private void DrawGame(object sender, PaintEventArgs e)
        {
            DibujarTablero(e.Graphics);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            try
            {
                game.Board.ChangeSnakeDirection(PosibleMovements[e.KeyCode]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wrong key!");
            }
        }

        private void DibujarTablero(Graphics gp)
        {
            Board board = game.Board;
            foreach (Block block in board.GetMatrix())
            {
                gp.DrawRectangle(new Pen(block.Color), new Rectangle(block.Location, new Size(block.Size.X, block.Size.Y)));
            }
        }

        private void OnGameEnd()
        {
            SnakeTimer.Enabled = false;
            MessageBox.Show("Game over");
            Application.Exit();
        }
    }
}
