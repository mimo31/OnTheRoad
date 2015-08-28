using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace OnTheRoad
{
    public partial class MainForm : Form
    {
        public byte RoadState { get; set; }
        public Color RoadColor { get; set; } = Color.Gray;
        public Color LineColor { get; set; } = Color.White;
        public float ViewPosition { get; set; }
        public EventHandler Tick;
        public float Speed = 5;
        public List<RoadObject> RoadObjects = new List<RoadObject>();
        public List<Func<Item>> Spawners = new List<Func<Item>>();
        public List<Item>[] CatchableItems = new List<Item>[4];

        public MainForm()
        {
            InitializeComponent();
            this.UpdateTimer.Tick += UpdateGame;
            this.Tick += TickHandler;
            Pickup pickup = new Pickup();
            pickup.PlacedBlocks[0] = new PlacedBox();
            RoadObjects.Add(pickup);
            for (int i = 0; i < 4; i++)
            {
                this.CatchableItems[i] = new List<Item>();
            }
            this.InitializeSpawners();
        }

        private void InitializeSpawners()
        {

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
            float pixelsUsed = -ToScreenSize(this.ViewPosition - 128);
            float roadScreenSize = ToScreenSize(192);
            float objectPreferredLeftEdge = ToScreenSize(544);
            for (int i = 0; i < this.RoadObjects.Count; i++)
            {
                if (pixelsUsed >= this.ClientSize.Height)
                {
                    break;
                }
                float objectHeight = this.RoadObjects[i].GetHeight(roadScreenSize);
                if (pixelsUsed + objectHeight >= 0)
                {
                    this.RoadObjects[i].Paint(g, new Point((int)objectPreferredLeftEdge, (int)pixelsUsed), roadScreenSize);
                }
                pixelsUsed += objectHeight;
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
