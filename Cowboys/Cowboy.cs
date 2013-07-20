using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cowboys
{
    /// <summary>
    /// A Cowboy is a point in space, it can keep track of how many other cowboys can see it and from
    /// this work out if it is alive or dead
    /// </summary>
    class Cowboy
    {
        private decimal[] location; //location as a 2 element array
        private Problem problem; //problem associated with this cowboy
        private int numSeen; //the number of other cowboys that can see this cowboy

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
            location = new decimal[] { (decimal)(r.Next(Problem.SizeX) + r.NextDouble()), (decimal)(r.Next(Problem.SizeY) + r.NextDouble()) };
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
        /// uses the PNPoly algorithm found at: http://www.ecse.rpi.edu/~wrf/Research/Short_Notes/pnpoly.html#The%20C%20Code.
        /// </summary>
        private void checkLocation()
        {
            Obstacle[] obstacles = problem.Obstacles;
            foreach (Obstacle o in obstacles)
            {
                if(location[0] < o.right() && location[0] > o.left() && location[1] < o.bottom() && location[1] > o.top())
                {
                    int i, j = 0;
                    bool c = false;
                    int nvert = 4;
                    decimal[] vertx = new decimal[4];
                    decimal[] verty = new decimal[4];
                    decimal[,] points = o.Points;
                    for (int ptToExamine = 0; ptToExamine < 4; ptToExamine++)
                    {
                        vertx[ptToExamine] = points[ptToExamine, 0];
                        verty[ptToExamine] = points[ptToExamine, 1];
                    }
                    decimal testx = location[0];
                    decimal testy = location[1];
                    for (i = 0, j = nvert - 1; i < nvert; j = i++)
                    {
                        if (((verty[i] > testy) != (verty[j] > testy)) &&
                            (testx < (vertx[j] - vertx[i]) * (testy - verty[i]) / (verty[j] - verty[i]) + vertx[i]))
                        {
                            c = !c;
                        }
                    }
                    if (c) //then move the cowboy to a location not inside an obstacle
                    {
                        Random r = problem.Random;
                        switch (r.Next(4))
                        {
                            case 0: location[0] = o.left() - 1; break;
                            case 1: location[0] = o.right() + 1; break;
                            case 2: location[1] = o.top() - 1; break;
                            case 3: location[1] = o.bottom() + 1; break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Informs the cowboy it can see another, and therefore increments the numseen counter
        /// </summary>
        public void seen()
        {
            numSeen++;
        }

        /// <summary>
        /// Resets the number of times the cowboys has been seen to 0
        /// </summary>
        public void resetSeen()
        {
            numSeen = 0;
        }

        /// <summary>
        /// Returns the better cowboy based on the visibility of the cowboy, with better
        /// cowboys having lower visibility.
        /// </summary>
        /// <param name="posCowboy">The cowboy that if better will get the return value 1</param>
        /// <param name="negCowboy">The cowboy that if better will get the return value -1</param>
        /// <returns>1 if posCowboy is better, -1 if negCowboy is better and 0 if they are equal</returns>
        public static int sortByNumSeen(Cowboy negCowboy, Cowboy posCowboy)
        {
            posCowboy.problem.compare();
            if (posCowboy.NumSeen < negCowboy.NumSeen)
            {
                return 1;
            }
            else if (posCowboy.NumSeen > negCowboy.NumSeen)
            {
                return -1;
            }
            else //if posCowboy.NumSeen == negCowboy.NumSeen
            {
                return 0;
            }
        }

        /// <summary>
        /// Sorts the cowboys by location, with top left being placed first
        /// </summary>
        /// <param name="negCowboy">The cowboy which will elicit a -1 response if it is found to be more NW than the other</param>
        /// <param name="posCowboy">The cowboy which will elicit a 0 response if it is found to be more NW than the other</param>
        /// <returns>-1 or 1 depending on which cowboy is the furthest NW or 0 if they are equal</returns>
        public static int sortByLocation(Cowboy negCowboy, Cowboy posCowboy)
        {
            negCowboy.problem.compare();
            decimal[] negLoc = negCowboy.Location;
            decimal[] posLoc = posCowboy.Location;
            if (negLoc[1] < posLoc[1])
            {
                return -1;
            }
            else if (negLoc[1] > posLoc[1])
            {
                return 1;
            }
            else
            {
                if (negLoc[0] < posLoc[0])
                {
                    return -1;
                }
                else if (negLoc[0] > posLoc[0])
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Sorts the cowboys by if they are alive or not
        /// </summary>
        /// <param name="negCowboy">The cowboy that gives a -1 response if it is alive and the other isn't</param>
        /// <param name="posCowboy">The cowboy that gives a 1 response if it is alive and the other isn't</param>
        /// <returns>-1 or 1 if one cowboy is alive and the other isnt 0 otherwise</returns>
        public static int sortAlive(Cowboy negCowboy, Cowboy posCowboy)
        {
            negCowboy.problem.compare();
            int ret = 0;
            if ((negCowboy.isDead && posCowboy.isDead) || (!negCowboy.isDead && !posCowboy.isDead))
            {
            }
            else if (negCowboy.isDead && !posCowboy.isDead)
            {
                ret = 1;
            }
            else if (posCowboy.isDead && !negCowboy.isDead)
            {
                ret = -1;
            }
            return ret;
        }
    }
}
