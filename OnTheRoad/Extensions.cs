using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OnTheRoad
{
    public static class Extensions
    {
        public static void DrawImageBySize(this Graphics g, Image image, PointF location)
        {
            g.DrawImage(image, new RectangleF(location, image.Size));
        }
    }
}
