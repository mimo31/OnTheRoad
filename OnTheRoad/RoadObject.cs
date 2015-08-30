using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace OnTheRoad
{
    public delegate void RefClickEventHandler(object sender, RefClickEventArgs e);

    public class RefClickEventArgs : EventArgs
    {
        public MouseButtons Button { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public RefClickEventArgs(MouseButtons button, float x, float y)
        {
            this.Button = button;
            this.X = x;
            this.Y = y;
        }
    }

    public abstract class RoadObject
    {
        public abstract float Mass { get; }
        public abstract float Power { get; }
        public abstract PlacedItem[] PlacedItems { get; protected set; }
        public abstract bool IsFirst { get; }
        protected event RefClickEventHandler Clicked;
        protected abstract PointF[] RefPlacedLocations { get; }

        public void Paint(Graphics g, Point prefferedLocation, float prefferedWidth)
        {
            this.PaintCar(g, prefferedLocation, prefferedWidth);
            Point[] placedLocations = new Point[this.RefPlacedLocations.Length];
            for (int i = 0; i < this.RefPlacedLocations.Length; i++)
            {
                placedLocations[i] = new Point((int)(prefferedLocation.X + this.RefPlacedLocations[i].X * prefferedWidth), (int)(prefferedLocation.Y + this.RefPlacedLocations[i].Y * prefferedWidth));
            }
            for (int i = 0; i < this.PlacedItems.Length; i++)
            {
                if (this.PlacedItems[i] != null)
                {
                    this.PlacedItems[i].Paint(g, new PlacedItemPaintLocation(placedLocations[i], prefferedWidth / 3));
                }
            }
        }

        public abstract void PaintCar(Graphics g, Point prefferedLocation, float prefferedWidth);

        public abstract float GetHeight(float width);

        public abstract Item[] DemontDrop();

        public void Click(object sender, MouseEventArgs e, Point prefferedLocation, float prefferedWidth)
        {
            PointF clickRefLocation = new PointF((e.X - prefferedLocation.X) / prefferedWidth, (e.Y - prefferedLocation.Y) / prefferedWidth);
            RefClickEventArgs newEventArgs = new RefClickEventArgs(e.Button, clickRefLocation.X, clickRefLocation.Y);
            if (this.Clicked != null)
            {
                this.Clicked(this, newEventArgs);
            }
            RectangleF[] placedItemsRects = new RectangleF[this.PlacedItems.Length];
            for (int i = 0; i < this.RefPlacedLocations.Length; i++)
            {
                placedItemsRects[i] = new RectangleF(this.RefPlacedLocations[i], new SizeF(1 / (float)3, 1 / (float)3));
            }
            for (int i = 0; i < placedItemsRects.Length; i++)
            {
                if (this.PlacedItems[i] != null)
                {
                    if (placedItemsRects[i].Contains(clickRefLocation))
                    {
                        RefClickEventArgs placedItemClickEventArgs = new RefClickEventArgs(e.Button, (clickRefLocation.X - this.RefPlacedLocations[i].X) / 3, (clickRefLocation.Y - this.RefPlacedLocations[i].Y) / 3);
                        this.PlacedItems[i].Click(placedItemClickEventArgs);
                        if (e.Button == MouseButtons.Right && this.PlacedItems[i] is IGui)
                        {
                            (sender as MainForm).CurrentGui = (this.PlacedItems[i] as IGui).Gui;
                        }
                    }
                }
            }
        }
    }
}
