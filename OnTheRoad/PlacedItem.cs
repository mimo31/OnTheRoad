using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace OnTheRoad
{
    public abstract class PlacedItem
    {
        public event RefClickEventHandler Clicked;

        public abstract Item[] DemontDrop();

        public abstract void Paint(Graphics g, PlacedItemPaintLocation location);

        public void Click(RefClickEventArgs e)
        {
            if (this.Clicked != null)
            {
                this.Clicked(this, e);
            }
        }
    }

    public struct PlacedItemPaintLocation
    {
        public Point Location { get; set; }
        public float PreferredSize { get; set; }

        public PlacedItemPaintLocation(Point location, float prefferedSize)
        {
            this.Location = location;
            this.PreferredSize = prefferedSize;
        }
    }
}
