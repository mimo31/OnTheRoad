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
            this.MaxWindowsHeightPart = (this.Height + 1) / (float)18;
            this.MaxWindowsWidthPart = this.Width / (float)32;
            this.WidthHeightRatio = this.Width / (float)(this.Height + 1);
            base.Clicked += this.Click;
        }

        public void Click(object sender, RefClickEventArgs e)
        {
            float slotSize = 1 / (float)this.Width;
            if (e.X > 0 && e.X < 1 && e.Y > slotSize && e.Y < (this.Height + 1) / (float)this.Width)
            {
                float inSlotLocationX = e.X % slotSize;
                float inSlotLocationY = e.Y % slotSize;
                if (inSlotLocationX > slotSize / 16 && inSlotLocationX < slotSize - slotSize / 16 && inSlotLocationY > slotSize / 16 && inSlotLocationY < slotSize - slotSize / 16)
                {
                    int slotX = (int)Math.Floor(e.X / slotSize);
                    int slotY = (int)Math.Floor(e.Y / slotSize) - 1;
                    int slotsInLastRow = this.Items.Length % this.Width;
                    if (!(slotY == this.Height - 1 && slotX > slotsInLastRow - 1 && slotsInLastRow != 0))
                    {
                        int slotIndex = slotX + slotY * this.Width;
                        MainForm form = sender as MainForm;
                        Item oldHeldItem = form.HeldItem;
                        form.HeldItem = this.Items[slotIndex];
                        this.Items[slotIndex] = oldHeldItem;
                    }
                }
            }
        }

        protected override void DrawInside(Graphics g, Point startPoint, float width)
        {
            g.FillRectangle(Brushes.White, new Rectangle(startPoint, new Size((int)width, (int)(width / this.WidthHeightRatio))));
            int itemsDrawn = 0;
            float slotSize = width / this.Width;
            for (int i = 0; i < this.Height; i++)
            {
                for (int j = 0; j < this.Width; j++)
                {
                    if (itemsDrawn < this.Items.Length)
                    {
                        PointF slotUpLeftCorner = new PointF(startPoint.X + j * slotSize, startPoint.Y + (i + 1) * slotSize);
                        g.FillRectangle(Brushes.Green, slotUpLeftCorner.X + slotSize / 16, slotUpLeftCorner.Y + slotSize / 16, slotSize - slotSize / 8, slotSize - slotSize / 8);
                        int itemIndex = i * this.Width + j;
                        if (this.Items[itemIndex] != null)
                        {
                            this.Items[itemIndex].Paint(g, new PointF(slotUpLeftCorner.X + slotSize / 8, slotUpLeftCorner.Y + slotSize / 8), slotSize - slotSize / 4);
                        }
                        itemsDrawn++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public static float GetSlotSize(Size clientSize)
        {
            float maxWidth = clientSize.Width / (float)32;
            float maxHeight = clientSize.Height / (float)18;
            if (maxWidth > maxHeight)
            {
                return maxHeight;
            }
            else
            {
                return maxWidth;
            }
        }
    }
}
