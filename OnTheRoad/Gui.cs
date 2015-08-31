using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace OnTheRoad
{
    public abstract class Gui
    {
        protected abstract float WidthHeightRatio { get; set; }
        protected abstract float MaxWindowsHeightPart { get; set; }
        protected abstract float MaxWindowsWidthPart { get; set; }

        protected event RefClickEventHandler Clicked;

        protected abstract void DrawInside(Graphics g, Point startPoint, float width);

        public void Draw(Graphics g, Size clientSize)
        {
            RectangleF guiRect = this.GetGuiRect(clientSize);
            this.DrawInside(g, new Point((int)guiRect.Location.X, (int)guiRect.Location.Y), guiRect.Width);
        }

        private RectangleF GetGuiRect(Size clientSize)
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
            return new RectangleF((clientSize.Width - finalWidth) / 2, (clientSize.Height - finalHeight) / 2, finalWidth, finalHeight);
        }

        public void Click(MainForm sender, MouseEventArgs e)
        {
            RectangleF guiRect = this.GetGuiRect(sender.ClientSize);
            if (this.Clicked != null)
            {
                RefClickEventArgs newEventArgs = new RefClickEventArgs(e.Button, (e.X - guiRect.X) / guiRect.Width, (e.Y - guiRect.Y) / guiRect.Width);
                this.Clicked(sender, newEventArgs);
            }
            if (!this.GetGuiRect(sender.ClientSize).Contains(new Point(e.X, e.Y)))
            {
                sender.CurrentGui = null;
            }
        } 
    }
}
