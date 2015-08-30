using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTheRoad
{
    public abstract class PlainStorage : PlacedItem, IGui, IStorage
    {
        public abstract Item[] Items { get; set; }
        public abstract int GridWidth { get; }
        public abstract string InGUIName { get; }
        readonly Gui gui;
        public Gui Gui { get { return this.gui; } }

        public PlainStorage()
        {
            this.gui = new StorageGui(this.Items, this.GridWidth, this.InGUIName);
        }
    }

    public class StorageGui : Gui
    {
        private Item[] Items;
        private int Width;
        private int Height;
        private string Name;
        protected override float MaxWindowsHeightPart { get; set; }
        protected override float MaxWindowsWidthPart { get; set; }
        protected override float WidthHeightRatio { get; set; }

        public StorageGui(Item[] items, int width, string name)
        {
            this.Items = items;
            this.Width = width;
            this.Height = (int)Math.Ceiling(this.Items.Length / (float)this.Width);
            this.Name = name;
            this.MaxWindowsHeightPart = (this.Width + 1) * 9 / (float)1024;
            this.MaxWindowsWidthPart = this.Width / (float)64;
            this.WidthHeightRatio = this.Width / (float)(this.Height + 1);
        }

        protected override void DrawInside(Graphics g, Point startPoint, float width)
        {
            g.FillRectangle(Brushes.White, new Rectangle(startPoint, new Size((int)width, (int)(width / this.WidthHeightRatio))));
        }
    }
}
