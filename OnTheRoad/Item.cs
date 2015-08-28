using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OnTheRoad
{
    public abstract class Item
    {
        public abstract void Paint(Graphics g, PointF prefferedLocation, float prefferedSize);
    }
}
