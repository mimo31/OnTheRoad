using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OnTheRoad
{
    public abstract class RoadObject
    {
        public float Mass { get; set; }
        public float Power { get; set; }
        public PlacedItemPaintLocation[] FreeSpace { get; set; }
        public PlacedItem[] PlacedBlocks { get; set; }
        public bool IsFirst { get; set; }

        public abstract void Paint(Graphics g, Point prefferedLocation, float prefferedWidth);

        public abstract float GetHeight(float width);

        public abstract Item[] DemontDrop();
    }
}
