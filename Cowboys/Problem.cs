using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Cowboys
{
    /// <summary>
    /// The abstract class problem deals with loading the world and contains easily overridable methods to allow
    /// the implementation of different genetic algorithms
    /// </summary>
    abstract class Problem
    {
        protected List<Obstacle> obstacles;
        public Obstacle[] Obstacles
        {
            get { return obstacles.ToArray<Obstacle>(); }
        }
        protected List<Line> lines;
        public Line[] Lines
        {
            get { return lines.ToArray<Line>(); }
        }
        protected List<Solution> solutions;
        public List<Solution> Solutions
        {
            get { return solutions; }
        }
        protected Random rand;
        //Changing these is only partially implemented, and so they shouldn't be changed.
        protected const int numSolns = 10;
        public const int NumCowboys = 10;
        public const int ObstSides = 4;

        protected int comparisons;
        protected const int targetComparisons = 100000;

        protected int initialBestVisibility;
        public int InitialBestVisibility
        {
            get { return initialBestVisibility; }
        }
        protected int initialBestAlive;
        public int InitialBestAlive
        {
            get { return initialBestAlive; }
        }
        protected decimal initialAvgAlive;
        public decimal InitialAvgAlive
        {
            get { return initialAvgAlive; }
        }
        protected decimal initialAvgVis;
        public decimal InitialAvgVis
        {
            get { return initialAvgVis; }
        }

        public Random Random
        {
            get { return rand; }
        }
        public const int SizeX = 800;
        public const int SizeY = 600;

        //P(Mutation) = 1/mutationChance
        protected const int mutationChance = 10;
        public int MutationChance
        {
            get { return mutationChance; }
        }

        /// <summary>
        /// Initialises the problem and calls the method to set up the solutions and run the algorithm
        /// </summary>
        public Problem()
        {
            rand = new Random();
            comparisons = 0;
            parseWorld();
            //convert obstacles into a list of lines
            lines = new List<Line>();
            foreach (Obstacle o in obstacles)
            {
                lines.AddRange(o.toLines());
            }
            setUpSolutions();
            run();
        }

        /// <summary>
        /// Tells the problem a comparison has been made, and increments the comparison variable.
        /// </summary>
        public void compare()
        {
            comparisons++;
        }

        /// <summary>
        /// Runs the program until a set number of comparisons have been reached. Uses the execute
        /// method which should be overriden in each subclass of Problem.
        /// </summary>
        public void run()
        {
            while (comparisons < targetComparisons)
            {
               execute();
            }
            solutions.Sort(Solution.judgeByNumAlive);
        }

        /// <summary>
        /// Set the solutions to a number of random solutions, overridden for lower level only breeding
        /// since that doesn't need more than one solution object
        /// </summary>
        protected virtual void setUpSolutions()
        {
            solutions = new List<Solution>();
            for (int i = 0; i < numSolns; i++)
            {
                solutions.Add(new Solution(this));
            }
            initialBestVisibility = getBestVisibility().getVisibility();
            initialBestAlive = getBestNumAlive().getNumAlive();
            initialAvgVis = getAvgVisibility();
            initialAvgAlive = getAvgNumAlive();
        }

        /// <summary>
        /// Parses the world from an xml file and sets it up.
        /// </summary>
        private void parseWorld()
        {
            obstacles = new List<Obstacle>();
            var xml = XDocument.Load("C:/Users/Owen/Documents/GitHub/Cowboys/Cowboys/World1.xml");
            IEnumerable<string> obstStrings = from o in xml.Descendants("Obstacle")
                                              select o.Value;
            foreach (string s in obstStrings)
            {
                decimal[,] points = new decimal[ObstSides,2];
                int numPoints = 0;
                string[] pts = s.Split(':');
                foreach (string point in pts)
                {
                    string[] stringCoOrds = point.Split(',');
                    points[numPoints, 0] = int.Parse(stringCoOrds[0]);
                    points[numPoints, 1] = int.Parse(stringCoOrds[1]);
                    numPoints++;
                }
                obstacles.Add(new Obstacle(this, points));
            }

        }

        /// <summary>
        /// Gets the average number of cowboys alive in the current solution pool
        /// </summary>
        /// <returns>The average number of cowboys alive in the current solution pool</returns>
        public decimal getAvgNumAlive()
        {
            decimal i = 0;
            foreach (Solution s in solutions)
            {
                i += s.getNumAlive();
            }
            i /= solutions.Count;
            return i;
        }

        /// <summary>
        /// Gets the average total visibility of each solution in the current solution pool
        /// </summary>
        /// <returns>The average total visibility of each solution in the current solution pool</returns>
        public decimal getAvgVisibility()
        {
            decimal i = 0;
            foreach (Solution s in solutions)
            {
                i += s.getVisibility();
            }
            i /= solutions.Count;
            return i;
        }

        /// <summary>
        /// Breeds a new solution at random from a list of possible parents
        /// </summary>
        /// <param name="solutionsList">The list of potential parent</param>
        /// <returns>A solution gotten by breeding 2 of the solutions on the list</returns>
        public virtual Solution breedNewSolution(Solution[] solutionsList)
        {
            if (Random.Next(MutationChance) == 0)
            {
                return new Solution(this);
            }
            else
            {
                Solution s1 = solutionsList[rand.Next(solutionsList.Length - 1)];
                Solution s2 = solutionsList[rand.Next(solutionsList.Length - 1)];
                while (s1 == s2)
                {
                    s2 = solutionsList[rand.Next(solutionsList.Length - 1)];
                }
                List<Cowboy> completeGenes = new List<Cowboy>();
                for (int i = 0; i < NumCowboys; i++)
                {
                    if (i % 2 == 0)
                    {
                        Cowboy c = s1.Cowboys[i];
                        completeGenes.Add(new Cowboy(this, new decimal[] { c.Location[0], c.Location[1] }));
                    }
                    else
                    {
                        Cowboy c = s2.Cowboys[i];
                        completeGenes.Add(new Cowboy(this, new decimal[] { c.Location[0], c.Location[1] }));
                    }
                }
                return new Solution(this, completeGenes.ToArray<Cowboy>());
            }
        }

        /// <summary>
        /// Gets the solution with the most cowboys alive in the current pool
        /// </summary>
        /// <returns>The solution with the most cowboys alive</returns>
        public Solution getBestNumAlive()
        {
            int numAlive = 0;
            Solution best = null;
            foreach (Solution s in solutions)
            {
                if (s.getNumAlive() >= numAlive)
                {
                    numAlive = s.getNumAlive();
                    best = s;
                }
            }
            return best;
        }

        /// <summary>
        /// Gets the solution with the best visibility in the current pool
        /// </summary>
        /// <returns>The solution with the lowest total visibility</returns>
        public Solution getBestVisibility()
        {
            int visibility = int.MaxValue;
            Solution best = null;
            foreach (Solution s in solutions)
            {
                if (s.getVisibility() <= visibility)
                {
                    visibility = s.getVisibility();
                    best = s;
                }
            }
            return best;
        }

        /// <summary>
        /// Breeds a new cowboy selecting the parents randomly from a specified list
        /// </summary>
        /// <param name="cowboysList">The list of cowboys that are potential parents for the new cowboy</param>
        /// <returns>A new cowboy bred from 2 random parents in the provided list</returns>
        public Cowboy breedNewCowboy(Cowboy[] cowboysList)
        {
            if (Random.Next(MutationChance) == 0)
            {
                return new Cowboy(this);
            }
            else
            {
                Cowboy cb1 = cowboysList[Random.Next(cowboysList.Length)];
                Cowboy cb2 = cowboysList[Random.Next(cowboysList.Length)];
                return (breedNewCowboy(cb1, cb2));
            }
        }

        /// <summary>
        /// Breeds a new cowboy with the specified parents.
        /// </summary>
        /// <param name="c1">The first parent of the new cowboy</param>
        /// <param name="c2">The second parent of the new cowboy</param>
        /// <returns>The cowboy that is the offspring of the 2 parent cowboys provided</returns>
        public Cowboy breedNewCowboy(Cowboy c1, Cowboy c2)
        {
            return new Cowboy(this, new decimal[] { c1.Location[0], c2.Location[1] });
        }

        /// <summary>
        /// Execute should do some kind of breeding action
        /// </summary>
        public abstract void execute();
    }
}
