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
        public float ViewPosition { get; set; }
        public EventHandler Tick;
        public float Speed = 5;
        public Image CarImage;
        public readonly string RootDirecory;

        public MainForm()
        {
            InitializeComponent();
            RootDirecory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\OnTheRoad";
            this.UpdateTimer.Tick += UpdateGame;
            this.Tick += TickHandler;
            CarImage = new Bitmap(RootDirecory + "\\Textures\\Car.png");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.DoubleBuffered = true;
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            DrawRoad(e.Graphics);
            DrawCar(e.Graphics);
        }

        private void DrawRoad(Graphics g)
        {
            float edgeSize = this.ClientSize.Width / (float)4;
            g.FillRectangle(new SolidBrush(RoadColor), edgeSize, 0, edgeSize * 2, this.ClientSize.Height);
            float lineHeight = ToScreenSize(128);
            float lineWidth = ToScreenSize(128 / 4);
            float lineX = edgeSize * 2 - lineWidth / 2;
            byte stateToDraw = (byte)(RoadState + ViewPosition);
            if (stateToDraw < 128)
            {
                g.FillRectangle(new SolidBrush(LineColor), lineX, 0, lineWidth, lineHeight - ToScreenSize(stateToDraw));
            }
            float drawnPixels;
            drawnPixels = ToScreenSize(256 - stateToDraw);
            while (drawnPixels < this.ClientSize.Height)
            {
                g.FillRectangle(new SolidBrush(LineColor), lineX, drawnPixels, lineWidth, lineHeight);
                drawnPixels += lineHeight * 2;
            }
        }

        public float ToScreenSize(float size)
        {
            return this.ClientSize.Width / (float)1024 * size;
        }

        private void DrawCar(Graphics g)
        {
            float newBitmapHeight = 192 / (float)CarImage.Width * CarImage.Height;
            if (!(ViewPosition + 128 > this.ClientSize.Height) && (128 + newBitmapHeight > ViewPosition))
            {
                g.DrawImage(CarImage, new RectangleF(ToScreenSize(544), ToScreenSize(128 - ViewPosition), ToScreenSize(192), ToScreenSize(newBitmapHeight)));
            }
        }

        private void TickHandler(object sender, EventArgs e)
        {
            this.RoadState -= (byte)this.Speed;
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
