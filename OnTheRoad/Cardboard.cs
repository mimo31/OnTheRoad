using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTheRoad
{
    public class Cardboard : Item
    {
        public override void Paint(Graphics g, PointF prefferedLocation, float prefferedSize)
        {
            g.DrawImageBySize(Resources.GetTexture("OnTheRoad\\Cardboard.png", prefferedSize), prefferedLocation);
        }

        public static Func<Random, Item> Spawner = (Random r) =>
        {
            if (r.Next(1000) == 0)
            {
                return new Cardboard();
            }
            else
            {
                return null;
            }
        };
    }
}
