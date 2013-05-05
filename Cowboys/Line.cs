using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cowboys
{
    class Line
    {
        private decimal[] point1;
        private decimal[] point2;
        public decimal[] Point1
        {
            get { return point1; }
        }
        public decimal[] Point2
        {
            get { return point2; }
        }
        private decimal X1
        {
            get { return point1[0]; }
        }
        private decimal X2
        {
            get { return point2[0]; }
        }
        private decimal Y1
        {
            get { return point1[1]; }
        }
        private decimal Y2
        {
            get { return point2[1]; }
        }
        public decimal A
        {
            get { return Y2 - Y1; }
        }
        public decimal B
        {
            get { return X1 - X2; }
        }
        public decimal C
        {
            get { return A * X1 + B * Y1; }
        }
        /// <summary>
        /// Initialises the Line with 2 points
        /// </summary>
        /// <param name="point1">The first point of the line</param>
        /// <param name="point2">The second point of the line</param>
        public Line(decimal[] point1, decimal[] point2)
        {
            this.point1 = point1;
            this.point2 = point2;
        }

        /// <summary>
        /// Method to check if 2 finite length lines intersect
        /// </summary>
        /// <param name="l">The line to check for intersection with</param>
        /// <returns>True if the lines intersect false otherwise</returns>
        public bool intersects(Line l)
        {
            bool intersect = false;
            decimal det = A * l.B - l.A * B;
            if (det != 0)//lines are not parallel
            {
                //get where the lines intersect
                decimal intersectX = (l.B * C - B * l.C) / det;
                decimal intersectY = (A * l.C - l.A * C) / det;
                
                //check if this point is on both lines
                bool xOnThisLine = Math.Min(X1, X2) <= intersectX
                    && intersectX <= Math.Max(X1, X2);
                bool xOnL = Math.Min(l.X1, l.X2) <= intersectX
                    && intersectX <= Math.Max(l.X1, l.X2);

                bool yOnThisLine = Math.Min(Y1, Y2) <= intersectY
                    && intersectY <= Math.Max(Y1, Y2);
                bool yOnL = Math.Min(l.Y1, l.Y2) <= intersectY
                    && intersectY <= Math.Max(l.Y1, l.Y2);

                //Update the flag to be returned
                intersect = xOnThisLine && xOnL && yOnThisLine && yOnL;
            }
            return intersect;
        }
    }
}
