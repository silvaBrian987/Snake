using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

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

            Player player = new Player("Pepe");
            game = new Game(player);
            game.OnGameEnd += OnGameEnd;

            this.Size = WorldConstants.GetDisplaySize();
            this.Size = new Size(this.Size.Width + 100, this.Size.Height + 100);
            SnakePanel.BackColor = Color.Black;
            SnakePanel.Size = WorldConstants.GetDisplaySize();
            CentrarSnakePanel();
            //SnakePanel.Size = new Size(game.Board.GetMatrix().GetLength(0) * WorldConstants.BLOCK_SIZE.X, game.Board.GetMatrix().GetLength(1) * WorldConstants.BLOCK_SIZE.Y);
            //SnakePanel.Dock = DockStyle.
            //SnakePanel.Anchor = AnchorStyles.None;

            SnakePanel.Paint += DrawGame;

            Controls.Add(SnakePanel);
        }

        private void CentrarSnakePanel()
        {
            Point centerOfDisplay = new Point(this.Size.Width / 2, this.Size.Height / 2);
            Point centerOfSnakePanel = new Point(SnakePanel.Size.Width / 2, SnakePanel.Height / 2);
            SnakePanel.Location = new Point(centerOfDisplay.X - centerOfSnakePanel.X, centerOfDisplay.Y - centerOfSnakePanel.Y);
        }

        private void SnakeDisplay_Load(object sender, EventArgs e)
        {
            SnakeTimer.Tick += (x, y) =>
            {
                game.Board.MoveSnake();
                lblPuntos.Text = game.Player.Points.ToString();
                SnakePanel.Invalidate();
            };

            SnakeTimer.Interval = 100;
            SnakeTimer.Enabled = true;

            //RunBGMusic();
        }

        private void RunBGMusic()
        {
            //System.IO.Stream str = Properties.Resources.bgmusic;
            //System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            //player.Stream = Properties.Resources.bgmusic;
            //player.PlayLooping();
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

                //   gp.DrawRectangle(new Pen(block.Color), new Rectangle(block.Location, new Size(block.Size.X, block.Size.Y)));
                gp.DrawRectangle(new Pen(block.Color), block.Location.X * block.Size.X, block.Location.Y * block.Size.Y, block.Size.X, block.Size.Y);
            }
        }

        private void OnGameEnd()
        {
            SnakeTimer.Enabled = false;
            MessageBox.Show(this, "***YOU DIED***" + Environment.NewLine + game.Player.Points + " food eaten", "GAME OVER");
            Application.Exit();
        }

        private void SnakeDisplay_Resize(object sender, EventArgs e)
        {
            CentrarSnakePanel();
        }
    }
}
