using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTheRoad
{
    public struct CatchableItem
    {
        public Item Item { get; set; }
        public int PositionY { get; set; }
        public byte PositionX { get; set; }

        public CatchableItem(Item item, byte positionX, int positionY)
        {
            this.Item = item;
            this.PositionX = positionX;
            this.PositionY = positionY;
        }
    }
}
