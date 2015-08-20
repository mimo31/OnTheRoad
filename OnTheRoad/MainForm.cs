using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnTheRoad
{
    public partial class MainForm : Form
    {
        public byte RoadState { get; set; }
        public Color RoadColor { get; set; } = Color.Chocolate;
        public Color LineColor { get; set; } = Color.White;
        public EventHandler Tick;
        public float Speed = 5;

        public MainForm()
        {
            InitializeComponent();
            this.UpdateTimer.Tick += UpdateGame;
            this.Tick += TickHandler;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            DrawRoad(e.Graphics);
        }

        private void DrawRoad(Graphics g)
        {
            float edgeSize = this.ClientSize.Width / (float)4;
            g.FillRectangle(new SolidBrush(RoadColor), edgeSize, 0, edgeSize * 2, this.ClientSize.Height);
            float lineWidth = edgeSize / 8;
            float lineHeight = lineWidth * 4;
            float lineX = edgeSize * 2 - lineWidth / 2;
            if (RoadState < 128)
            {
                g.FillRectangle(new SolidBrush(LineColor), lineX, 0, lineWidth, 128 - RoadState);
            }
            float drawnPixels;
            drawnPixels = 256 - RoadState;
            while (drawnPixels < this.ClientSize.Height)
            {
                g.FillRectangle(new SolidBrush(LineColor), lineX, drawnPixels, lineWidth, 128);
                drawnPixels += 256;
            }
        }

        private void TickHandler(object sender, EventArgs e)
        {
            RoadState -= (byte)Speed;
        }

        private void UpdateGame(object sender, EventArgs e)
        {
            Tick(this, new EventArgs());
            this.Refresh();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}
