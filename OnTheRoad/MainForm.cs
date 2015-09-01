using System;
using System.Collections.Generic;
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
        public float CatchableItemsState { get; set; }
        public Color RoadColor { get; set; } = Color.Gray;
        public Color LineColor { get; set; } = Color.White;
        public float ViewPosition { get; set; }
        public float Speed { get; set; } = 5;
        public List<RoadObject> RoadObjects { get; set; } = new List<RoadObject>();
        public List<Func<Random, Item>> Spawners { get; set; } = new List<Func<Random, Item>>();
        public List<CatchableItem> CatchableItems { get; set; } = new List<CatchableItem>();
        public bool MovingUp { get; set; }
        public bool MovingDown { get; set; }
        public float TotalCarHeight { get; set; }
        public Random R = new Random();
        public Gui CurrentGui { get; set; }
        public Item[] Inventory { get; set; }
        public Item HeldItem { get; set; }
        public Point CursorLocation { get; set; }

        public MainForm()
        {
            InitializeComponent();
            Pickup pickup = new Pickup();
            PlacedBox box = new PlacedBox();
            box.Items[6] = new Cardboard();
            pickup.PlacedItems[0] = box;
            RoadObjects.Add(pickup);
            TotalCarHeight = pickup.GetHeight(192);
            this.InitializeSpawners();
            this.Inventory = new Item[8];
            this.CursorLocation = new Point(0, 0);
        }

        private void InitializeSpawners()
        {
            Spawners.Add(Cardboard.Spawner);
        }

        private Item[] GetMaxFourSpawnedItems()
        {
            List<Item> spawnedItems = new List<Item>();
            foreach (Func<Random, Item> spawner in this.Spawners)
            {
                Item newItem = spawner(this.R);
                if (newItem != null)
                {
                    spawnedItems.Add(newItem);
                }
            }
            if (spawnedItems.Count > 4)
            {
                return this.Get4RandomElements(spawnedItems);
            }
            else
            {
                return spawnedItems.ToArray();
            }
        }

        private Item[] Get4RandomElements(IEnumerable<Item> array)
        {
            return array.OrderBy(item => this.R.NextDouble()).Take(4).ToArray();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.DoubleBuffered = true;
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            this.DrawRoad(e.Graphics);
            this.DrawCar(e.Graphics);
            this.DrawCatchableItems(e.Graphics);
            if (this.CurrentGui != null)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(127, 0, 0, 0)), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
                this.CurrentGui.Draw(e.Graphics, this.ClientSize);
            }
            this.DrawInventory(e.Graphics);
            this.DrawHeldItem(e.Graphics);
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

        private void DrawHeldItem(Graphics g)
        {
            if (this.HeldItem != null)
            {
                float slotSize = StorageGui.GetSlotSize(this.ClientSize);
                this.HeldItem.Paint(g, new PointF(this.CursorLocation.X - (slotSize - slotSize / 4) / 2, this.CursorLocation.Y - (slotSize - slotSize / 4) / 2), slotSize - slotSize / 4);
            }
        }

        public float ToScreenSize(float size)
        {
            return this.ClientSize.Width / (float)1024 * size;
        }

        public float ToGameSize(float size)
        {
            return 1024 * size / this.ClientSize.Width;
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

        private void DrawCatchableItems(Graphics g)
        {
            int minDisplayedY = (int)Math.Ceiling(this.ViewPosition - this.CatchableItemsState) / 64;
            int maxDisplayedY = (int)Math.Floor(this.ViewPosition - this.CatchableItemsState + this.ToGameSize(this.ClientSize.Height)) / 64 + 1;
            foreach (CatchableItem catchableItem in this.CatchableItems)
            {
                if (catchableItem.PositionY >= minDisplayedY && catchableItem.PositionY <= maxDisplayedY)
                {
                    PointF location = new PointF(ToScreenSize(768 + catchableItem.PositionX * 64), ToScreenSize(catchableItem.PositionY * 64 - 64 + this.CatchableItemsState - this.ViewPosition));
                    catchableItem.Item.Paint(g, location, ToScreenSize(64));
                }
            }
        }

        private void DrawInventory(Graphics g)
        {
            float slotSize = StorageGui.GetSlotSize(this.ClientSize);
            float upSide = this.ClientSize.Height / (float)2 - slotSize * 4;
            g.FillRectangle(Brushes.Khaki, 0, upSide - slotSize / 8, slotSize + slotSize / 8, slotSize * 8 + slotSize / 4);
            g.FillRectangle(Brushes.White, 0, upSide, slotSize, slotSize * 8);
            for (int i = 0; i < 8; i++)
            {
                g.FillRectangle(Brushes.Green, slotSize / 16, upSide + i * slotSize + slotSize / 16, slotSize - slotSize / 8, slotSize - slotSize / 8);
                if (this.Inventory[i] != null)
                {
                    this.Inventory[i].Paint(g, new PointF(slotSize / 8, upSide + i * slotSize + slotSize / 8), slotSize - slotSize / 4);
                }
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                this.MovingUp = true;
            }
            else if (e.KeyCode == Keys.Down)
            {
                this.MovingDown = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.CurrentGui = null;
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                this.MovingUp = false;
            }
            else if (e.KeyCode == Keys.Down)
            {
                this.MovingDown = false;
            }
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            this.RoadState -= (byte)this.Speed;
            float stepSize = this.ClientSize.Height / (float)this.ClientSize.Width * 16;
            if (this.MovingUp)
            {
                if (this.ViewPosition - stepSize < 0)
                {
                    this.ViewPosition = 0;
                }
                else
                {
                    this.ViewPosition -= stepSize;
                }
            }
            else if (this.MovingDown)
            {
                if (this.ViewPosition + stepSize > 256 + this.TotalCarHeight)
                {
                    this.ViewPosition = 256 + this.TotalCarHeight;
                }
                else
                {
                    this.ViewPosition += stepSize;
                }
            }
            this.CatchableItemsState += this.Speed;
            if (this.CatchableItemsState >= 64)
            {
                int moveByAmount = (int)Math.Floor(this.CatchableItemsState / 64);
                this.CatchableItemsState -= moveByAmount * 64;
                int maxY = (int)Math.Ceiling((TotalCarHeight + 256 + this.ToGameSize(this.ClientSize.Height)) / 64);
                for (int i = 0; i < this.CatchableItems.Count; i++)
                {
                    int newY = this.CatchableItems[i].PositionY + moveByAmount;
                    if (newY > maxY)
                    {
                        this.CatchableItems.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        this.CatchableItems[i] = new CatchableItem(this.CatchableItems[i].Item, this.CatchableItems[i].PositionX, newY);
                    }
                }
                for (int i = 0; i < moveByAmount; i++)
                {
                    int[] Ys = Enumerable.Range(0, 4).OrderBy(item => this.R.NextDouble()).ToArray();
                    Item[] items = this.GetMaxFourSpawnedItems();
                    CatchableItem[] catchableItems = new CatchableItem[items.Length];
                    for (int j = 0; j < catchableItems.Length; j++)
                    {
                        catchableItems[j] = new CatchableItem(items[j], (byte)Ys[j], i);
                    }
                    this.CatchableItems.AddRange(catchableItems);
                }
            }
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            float slotSize = StorageGui.GetSlotSize(this.ClientSize);
            float inventoryUpSide = this.ClientSize.Height / 2 - slotSize * 4;
            if (new Rectangle(0, (int)(inventoryUpSide - slotSize / 16), (int)(slotSize + slotSize / 16), (int)(slotSize * 8 + slotSize / 8)).Contains(e.Location))
            {
                if (e.X >= slotSize / 16 && e.X < slotSize - slotSize / 16 && e.Y >= inventoryUpSide && e.Y < this.ClientSize.Height + slotSize * 4)
                {
                    float inSlotLocationY = (e.Y - inventoryUpSide) % slotSize;
                    if (inSlotLocationY >= slotSize / 16 && inSlotLocationY < slotSize - slotSize / 16)
                    {
                        int slotIndex = (int)Math.Floor((e.Y - inventoryUpSide) / slotSize);
                        Item oldHeldItem = this.HeldItem;
                        this.HeldItem = this.Inventory[slotIndex];
                        this.Inventory[slotIndex] = oldHeldItem;
                    }
                }
            }
            else
            {
                if (this.CurrentGui != null)
                {
                    this.CurrentGui.Click(this, e);
                }
                else
                {
                    float objectPreferredLeftEdge = ToScreenSize(544);
                    if (e.X >= objectPreferredLeftEdge && e.X < this.ToScreenSize(736))
                    {
                        float gameSizeYLocation = this.ViewPosition + ToGameSize(e.Y);
                        float pixelsReached = 128;
                        for (int i = 0; i < this.RoadObjects.Count; i++)
                        {
                            pixelsReached += this.RoadObjects[i].GetHeight(192);
                            if (pixelsReached > gameSizeYLocation)
                            {
                                this.RoadObjects[i].Click(this, e, new Point((int)objectPreferredLeftEdge, (int)this.ToScreenSize(pixelsReached - this.RoadObjects[i].GetHeight(192) - this.ViewPosition)), this.ToScreenSize(192));
                                break;
                            }
                        }
                    }
                    else if (e.X >= this.ClientSize.Width * 3 / (float)4)
                    {
                        int catchableItemX = (int)Math.Floor((e.X - this.ClientSize.Width * 3 / (float)4) / (this.ClientSize.Width / (float)16));
                        int catchableItemY = (int)Math.Floor((this.ViewPosition + this.ToGameSize(e.Y) + 64 - this.CatchableItemsState) / 64);
                        bool found = false;
                        for (int i = 0; i < this.CatchableItems.Count; i++)
                        {
                            if (this.CatchableItems[i].PositionX == catchableItemX && this.CatchableItems[i].PositionY == catchableItemY)
                            {
                                Item oldHeldItem = this.HeldItem;
                                this.HeldItem = this.CatchableItems[i].Item;
                                if (oldHeldItem != null)
                                {
                                    CatchableItem newCatchableItem = new CatchableItem(oldHeldItem, (byte)catchableItemX, catchableItemY);
                                    this.CatchableItems[i] = newCatchableItem;
                                }
                                else
                                {
                                    this.CatchableItems.RemoveAt(i);
                                    found = true;
                                    break;
                                }
                            }
                        }
                        if (!found && this.HeldItem != null)
                        {
                            CatchableItem newCatchableItem = new CatchableItem(this.HeldItem, (byte)catchableItemX, catchableItemY);
                            this.CatchableItems.Add(newCatchableItem);
                            this.HeldItem = null;
                        }
                    }
                }
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            this.CursorLocation = e.Location;
        }
    }
}
