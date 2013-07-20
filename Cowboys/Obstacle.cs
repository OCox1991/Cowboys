using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cowboys
{
    /// <summary>
    /// An obstacle contains 4 points and can transmute these points into a series of lines for
    /// line of sight checking. Obstacles cannot handle angles above 180 degrees in the angles between
    /// sides since the way they create the lines is by drawing all possible lines are removing those that
    /// overlap.
    /// </summary>
    class Obstacle
    {
        private decimal[,] points;
        public decimal[,] Points
        {
            get { return points; }
        }
        private Problem problem;

        /// <summary>
        /// Sets up the obstacle with a semi random number of sides
        /// </summary>
        /// <param name="problem">The problem this obstacle is related to</param>
        public Obstacle(Problem problem)
        {
            this.problem = problem;
            points = new decimal[Problem.ObstSides, 2];
            //add random init code
        }

        /// <summary>
        /// An overloaded constructor that allows the specification of the points of the obstacle,
        /// as well as the problem it is related to.
        /// </summary>
        /// <param name="problem">The problem this obstacle is related to</param>
        /// <param name="points">The points that make up the vertices of this object</param>
        public Obstacle(Problem problem, decimal[,] points)
        {
            this.problem = problem;
            this.points = points;
        }

        /// <summary>
        /// Returns the obstacle as an array of lines to allow for checking of view lines
        /// </summary>
        /// <returns>The obstacle as a set of lines, with each line representing an edge of the obstacle</returns>
        public Line[] toLines()
        {
            List<Line> lines = new List<Line>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = i + 1; j < 4; j++)
                {
                    lines.Add(new Line(new decimal[] { points[i, 0], points[i, 1] }, new decimal[] { points[j, 0], points[j, 1] }));
                }
            }
            List<Line> removeList = new List<Line>();
            for (int i = 0; i < lines.Count - 1; i++)
            {
                for (int j = i + 1; j < lines.Count; j++)
                {
                    if(lines[i].intersectsNotTouches(lines[j]))
                    {
                        removeList.Add(lines[i]);
                        removeList.Add(lines[j]);
                    }
                }
            }
            foreach (Line rem in removeList)
            {
                lines.Remove(rem);
            }
            Line[] l = lines.ToArray<Line>();
            return l;
        }

        /// <summary>
        /// Returns the bottom of the shape's bounding rectangle
        /// </summary>
        /// <returns>The location of the bottom of the shape's bounding rectangle</returns>
        public decimal bottom()
        {
            decimal[] yVals = new decimal[4];
            for (int i = 0; i < 4; i++)
            {
                yVals[i] = points[i, 1];
            }
            return Enumerable.Max(yVals);
        }

        /// <summary>
        /// Returns the top of the shape's bounding rectangle
        /// </summary>
        /// <returns>The location of the top of the shape's bounding rectangle</returns>
        public decimal top()
        {
            decimal[] yVals = new decimal[4];
            for (int i = 0; i < 4; i++)
            {
                yVals[i] = points[i, 1];
            }
            return Enumerable.Min(yVals);
        }

        /// <summary>
        /// Returns the left of the shape's bounding rectangle
        /// </summary>
        /// <returns>The location of the left of the shape's bounding rectangle</returns>
        public decimal left()
        {
            decimal[] xVals = new decimal[4];
            for (int i = 0; i < 4; i++)
            {
                xVals[i] = points[i, 0];
            }
            return Enumerable.Min(xVals);
        }

        /// <summary>
        /// Returns the left of the shape's bounding rectangle
        /// </summary>
        /// <returns>The location of the right of the shape's bounding rectangle</returns>
        public decimal right()
        {
            decimal[] xVals = new decimal[4];
            for (int i = 0; i < 4; i++)
            {
                xVals[i] = points[i, 0];
            }
            decimal right = Enumerable.Max(xVals);
            return right;
        }
    }
}
