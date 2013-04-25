using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cowboys.Instance
{
    class Line
    {
        private Vector2 pointA;
        private Vector2 pointB;

        public Line(Vector2 pointA, Vector2 pointB)
        {
            this.pointA = pointA;
            this.pointB = pointB;
        }

        public bool intersects(Obstacle o)
        {
            int rad = o.Radius;
            Vector2 location = o.Location;
        }
    }
}
