﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTheRoad
{
    public class Box : Item, IPlacable
    {
        public PlacedItem NewPlacedItem { get { return new PlacedBox(); } }

        public override void Paint(Graphics g, PointF prefferedLocation, float prefferedSize)
        {
            g.DrawImageBySize(Resources.GetTexture("OnTheRoad\\Box.png", prefferedSize), prefferedLocation);
        }
    }
}
