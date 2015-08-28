using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTheRoad
{
    public class Box : Item
    {
        public override void Paint(Graphics g, PointF prefferedLocation, float prefferedSize)
        {
            g.DrawImage(Resources.GetTexture("OnTheRoad\\Box.png", prefferedSize), prefferedLocation);
        }
    }
}
