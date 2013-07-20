using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cowboys
{
    /// <summary>
    /// A solution is a set of 10 cowboys, and is responsible for checking which of them can see each other.
    /// </summary>
    class Solution
    {
        private Cowboy[] cowboys;
        public Cowboy[] Cowboys
        {
            get { return cowboys; }
        }

        //The problem associates with this solution, used to find the location of the obstacles
        //informed each time a comparison is made
        private Problem problem;

        /// <summary>
        /// Initialises the solution and associates it with a specified problem.
        /// </summary>
        /// <param name="problem">The problem to associate this solution with</param>
        public Solution(Problem problem)
        {
            this.problem = problem;
            cowboys = new Cowboy[Problem.NumCowboys];
            for (int i = 0; i < Problem.NumCowboys; i++)
            {
                cowboys[i] = new Cowboy(problem);
            }
            checkCowboys();
        }

        /// <summary>
        /// Initialises the solution providing it the list of cowboys and the problem to
        /// be associated with
        /// </summary>
        /// <param name="problem">The problem to associate this solution with</param>
        /// <param name="cowboys">The list of cowboys to use for this solution</param>
        public Solution(Problem problem, Cowboy[] cowboys)
        {
            this.problem = problem;
            this.cowboys = cowboys;
            checkCowboys();
        }

        /// <summary>
        /// Checks which cowboys can see each other
        /// </summary>
        public void checkCowboys()
        {
            foreach (Cowboy c in cowboys)
            {
                c.resetSeen();
            }
            for (int i = 0; i < cowboys.Length - 1; i++) //only check each pair once
            {
                for (int j = i + 1; j < cowboys.Length; j++)
                {
                    Cowboy cb1 = cowboys[i];
                    Cowboy cb2 = cowboys[j];
                    Line l = new Line(cb1.Location, cb2.Location);
                    int k = 0;
                    Line[] lines = problem.Lines;
                    bool intersects = false;
                    while (!intersects && k < lines.Length)
                    {
                        if(l.intersects(lines[k]))
                        {
                            intersects = true;
                        }
                        else
                        {
                            k++;
                        }
                    }
                    if(!intersects) //If the line between them hasn't intersected any obstacle lines
                    {
                        cb1.seen();
                        cb2.seen();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the count of the number of cowboys in the solution still alive
        /// </summary>
        /// <returns>The number of cowboys alive in this solution</returns>
        public int getNumAlive()
        {
            int alive = 0;
            foreach (Cowboy c in cowboys)
            {
                if (!c.isDead)
                {
                    alive++;
                }
            }
            return alive;
        }

        /// <summary>
        /// Judges 2 solutions by how many cowboys they have alive in them
        /// </summary>
        /// <param name="negSoln">The solution which if better elicits a -1 response</param>
        /// <param name="posSoln">The solution which if better elicits a 1 response</param>
        /// <returns>-1 if the negative solution is better, 1 if the positive solution is better, 0 if they are equal</returns>
        public static int judgeByNumAlive(Solution negSoln, Solution posSoln)
        {
            //tell the problem we are making a comparison
            posSoln.problem.compare();
            //first check if they are null
            if (posSoln == null && negSoln == null)
            {
                return 0;
            }
            else if (posSoln == null)
            {
                return -1;
            }
            else if (negSoln == null)
            {
                return 1;
            }
            else
            {
                //Here we want to select the one with the maximum number of living cowboys
                int posAlive = posSoln.getNumAlive();
                int negAlive = negSoln.getNumAlive();
                if (posAlive > negAlive)
                {
                    return 1;
                }
                else if (posAlive < negAlive)
                {
                    return -1;
                }
                else //if posAlive == negAlive
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Judges 2 solutions their total visibility, with lower being better
        /// </summary>
        /// <param name="negSoln">The solution which if better elicits a -1 response</param>
        /// <param name="posSoln">The solution which if better elicits a 1 response</param>
        /// <returns>-1 if the negative solution is better, 1 if the positive solution is better, 0 if they are equal</returns>
        public static int judgeByTotalVisibility(Solution negSoln, Solution posSoln)
        {
            posSoln.problem.compare();
            if (posSoln == null && negSoln == null)
            {
                return 0;
            }
            else if (posSoln == null)
            {
                return -1;
            }
            else if (negSoln == null)
            {
                return 1;
            }
            else
            {
                //In this case we want the lowest value for the number of cowboys that can
                //see each other.
                int numSeenPos = posSoln.getVisibility();
                int numSeenNeg = negSoln.getVisibility();
                if (numSeenPos > numSeenNeg)
                {
                    return -1;
                }
                else if (numSeenPos < numSeenNeg)
                {
                    return 1;
                }
                else //If numSeenPos == numSeenNeg
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Gets the total visibility of this solution
        /// </summary>
        /// <returns>The sum of all the visibilities of the cowboys in this solution</returns>
        public int getVisibility()
        {
            int i = 0;
            foreach (Cowboy c in cowboys)
            {
                i += c.NumSeen;
            }
            return i;
        }
    }
}
