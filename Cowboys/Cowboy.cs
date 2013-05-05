using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cowboys
{
    class Cowboy
    {
        private decimal[] location;
        private Problem problem;
        private int numSeen;

        /// <summary>
        /// Returns the location of the cowboy as an array, uses decimal primitive to avoid floating point errors.
        /// </summary>
        public decimal[] Location
        {
            get { return location; }
        }
        /// <summary>
        /// Checks how many other cowboys the cowboy can see
        /// </summary>
        public int NumSeen
        {
            get { return numSeen; }
        }
        /// <summary>
        /// Checks if the cowboy is dead, that is, if the number of creatures that have seen it is greater than 0
        /// </summary>
        public bool isDead
        {
            get { return NumSeen > 0; }
        }

        /// <summary>
        /// Sets up the cowboy with a random location
        /// </summary>
        /// <param name="problem">The problem the cowboy is a potential solution to</param>
        public Cowboy(Problem problem)
        {
            this.problem = problem;
            Random r = problem.Random;
            location = new decimal[] { r.Next(problem.SizeX) + r.NextDouble(), r.Next(problem.SizeY) + r.NextDouble() };
            checkLocation();
        }

        /// <summary>
        /// An overloaded constructor for the cowboy that allows its location to be specified.
        /// </summary>
        /// <param name="problem">The problem the cowboy is a potential solution to</param>
        /// <param name="location">The desired location for the cowboy</param>
        public Cowboy(Problem problem, decimal[] location)
        {
            this.problem = problem;
            this.location = location;
            checkLocation();
        }

        /// <summary>
        /// Checks the cowboy is not inside an obstacle, and if it is moves it to the edge of that obstacles bounding square
        /// </summary>
        private void checkLocation()
        {

        }

        /// <summary>
        /// Informs the cowboy it can see another, and therefore increments the numseen counter
        /// </summary>
        public void seen()
        {
            numSeen++;
        }
    }
}
