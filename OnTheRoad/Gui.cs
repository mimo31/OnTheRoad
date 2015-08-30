using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace OnTheRoad
{
    public abstract class Gui
    {
        protected abstract float WidthHeightRatio { get; set; }
        protected abstract float MaxWindowsHeightPart { get; set; }
        protected abstract float MaxWindowsWidthPart { get; set; }

        protected abstract void DrawInside(Graphics g, Point startPoint, float width);

        public void Draw(Graphics g, Size clientSize)
        {
            float maxWidth = this.MaxWindowsWidthPart * clientSize.Width;
            float maxHeight = this.MaxWindowsHeightPart * clientSize.Height;
            float finalWidth;
            float finalHeight;
            if (WidthHeightRatio > maxWidth / maxHeight)
            {
                finalWidth = maxWidth;
                finalHeight = finalWidth / this.WidthHeightRatio;
            }
            else
            {
                finalHeight = maxHeight;
                finalWidth = finalHeight * this.WidthHeightRatio;
            }
            this.DrawInside(g, new Point((int)((clientSize.Width - finalWidth) / 2), (int)((clientSize.Height - finalHeight) / 2)), finalWidth);
        }
    }
}
