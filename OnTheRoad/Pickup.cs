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
        public override PlacedItem[] PlacedItems { get; protected set; }
        public override bool IsFirst { get { return true; } }
        public override float Mass { get { return 100; } }
        public override float Power { get { return 500; } }
        private readonly PointF[] refPlacedLocations = new PointF[] { new PointF(0.15f, 1.4f), new PointF(0.5f, 1.4f ), new PointF(0.15f, 1.8f), new PointF(0.5f, 1.8f) };
        protected override PointF[] RefPlacedLocations { get { return refPlacedLocations; } }

        public Pickup()
        {
            this.PlacedItems = new PlacedItem[4];
        }

        public override Item[] DemontDrop()
        {
            return null;
        }

        public override void PaintCar(Graphics g, Point preferredLocation, float prefferedWidth)
        {
            g.DrawImageBySize(Resources.GetTexture(ResourceName, prefferedWidth), preferredLocation);
        }

        public override float GetHeight(float width)
        {
            return Resources.GetTexture(ResourceName, width).Height * 1.1f;
        }
    }
}
