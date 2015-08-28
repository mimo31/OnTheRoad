using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTheRoad
{
    public class Pickup : RoadObject
    {
        const string ResourceName = "OnTheRoad\\Car.png";

        public Pickup()
        {
            base.PlacedBlocks = new PlacedItem[4];
        }

        public override Item[] DemontDrop()
        {
            return null;
        }

        public override void Paint(Graphics g, Point preferredLocation, float prefferedWidth)
        {
            Bitmap bitmap = Resources.GetTexture(ResourceName, prefferedWidth);
            g.DrawImage(bitmap, preferredLocation);
            float[,] placedBlocksRefLocations = new float[,] { { 0.15f, 1.4f }, { 0.5f, 1.4f }, { 0.15f, 1.8f }, { 0.5f, 1.8f } };
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    placedBlocksRefLocations[i, j] = (int)(placedBlocksRefLocations[i, j] * prefferedWidth);
                }
            }
            for (int i = 0; i < 4; i++)
            {
                if (base.PlacedBlocks[i] != null)
                {
                    Point location = new Point(preferredLocation.X + (int)placedBlocksRefLocations[i, 0], preferredLocation.Y + (int)placedBlocksRefLocations[i, 1]);
                    base.PlacedBlocks[i].Paint(g, new PlacedItemPaintLocation(location, prefferedWidth / 3));
                }
                
            }
        }

        public override float GetHeight(float width)
        {
            return Resources.GetTexture(ResourceName, width).Height * 1.1f;
        }
    }
}
