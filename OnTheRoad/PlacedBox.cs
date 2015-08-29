using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTheRoad
{
    public class PlacedBox : PlacedItem
    {
        private Item[] Items = new Item[16];

        public override Item[] DemontDrop()
        {
            int nonNullItems = 0;
            for (int i = 0; i < this.Items.Length; i++)
            {
                if (this.Items[i] != null)
                {
                    nonNullItems++;
                }
            }
            Item[] allItems = new Item[nonNullItems + 1];
            int nextIndex = 0;
            for (int i = 0; i < this.Items.Length; i++)
            {
                if (this.Items[i] != null)
                {
                    allItems[nextIndex] = this.Items[i];
                    nextIndex++;
                }
            }
            allItems[nextIndex] = new Box();
            return allItems;
        }

        public override void Paint(Graphics g, PlacedItemPaintLocation location)
        {
            g.DrawImageBySize(Resources.GetTexture("OnTheRoad\\Box.png", location.PreferredSize), location.Location);
        }
    }
}
