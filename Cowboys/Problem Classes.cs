using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cowboys
{
    /// <summary>
    /// The control problem is the control for this data. It contains 100 random solutions instead of the
    /// normal 10. When executed it sets the number of comparisons to equal to the target amount.
    /// </summary>
    class ControlProblem : Problem
    {
        /// <summary>
        /// Sets up the solutions, overrides the default Problem method
        /// </summary>
        protected override void setUpSolutions()
        {
            solutions = new List<Solution>();
            for (int i = 0; i < 100; i++)
            {
                solutions.Add(new Solution(this));
            }
        }

        /// <summary>
        /// Sets the number of comparisons to the targer amount
        /// </summary>
        public override void execute()
        {
            comparisons = targetComparisons;
        }
    }

    /// <summary>
    /// The HighLvlOnlyVisProblem breeds at the Solution level, and uses the number of cowboys still alive in
    /// each solution to find the fitness function.
    /// </summary>
    class HighLvlOnlyDeadProblem : Problem
    {
        /// <summary>
        /// Override of the abstract execute method in Problem. Judges the solutions by number
        /// of cowboys alive, then kills half and repopulates using the survivors
        /// </summary>
        public override void execute()
        {
            solutions.Sort(Solution.judgeByNumAlive);
            solutions.RemoveRange(numSolns / 2, numSolns / 2);
            Solution[] remaining = solutions.ToArray<Solution>();
            while (solutions.Count < numSolns)
            {
                solutions.Add(breedNewSolution(remaining));
            }
        }
    }

    /// <summary>
    /// The HighLvlOnlyVisProblem breeds at the Solution level, and uses the total visibility of the
    /// cowboys to work out the fitness function.
    /// </summary>
    class HighLvlOnlyVisProblem : Problem
    {
        /// <summary>
        /// Override of the abstract execute method in Problem. Judges the solutions total visibility
        /// then kills half and repopulates using the survivors
        /// </summary>
        public override void execute()
        {
            solutions.Sort(Solution.judgeByTotalVisibility);
            solutions.RemoveRange(numSolns / 2, numSolns / 2);
            Solution[] remaining = solutions.ToArray<Solution>();
            while (solutions.Count < numSolns)
            {
                solutions.Add(breedNewSolution(remaining));
            }
        }
    }

    /// <summary>
    /// The LowLvlOnlyProblem breeds at the Cowboy level, it uses the status of each cowboy to decide how to sort it
    /// </summary>
    class LowLvlOnlyDeadProblem : Problem
    {
        /// <summary>
        /// Override of the set up solutions method in Problem. Used since the low level approach only needs one solution
        /// </summary>
        protected override void setUpSolutions()
        {
            solutions = new List<Solution>();
            solutions.Add(new Solution(this));
            initialBestVisibility = solutions[0].getVisibility();
            initialBestAlive = solutions[0].getNumAlive();
        }

        /// <summary>
        /// Overrides the execute method in Problem, eliminates the worst performing cowboys and
        /// breeds new ones using whether the cowboy is dead or alive as the fitness function.
        /// </summary>
        public override void execute()
        {
            Cowboy[] cowboys = solutions[solutions.Count - 1].Cowboys;
            List<Cowboy> sortingList = new List<Cowboy>();
            sortingList.AddRange(cowboys);
            sortingList.Sort(Cowboy.sortAlive);
            sortingList.RemoveRange(NumCowboys / 2, NumCowboys / 2);
            Cowboy[] remaining = sortingList.ToArray<Cowboy>();
            while (sortingList.Count < NumCowboys)
            {
                sortingList.Add(breedNewCowboy(remaining));
            }
            solutions[0] = new Solution(this, sortingList.ToArray<Cowboy>());
        }
    }

    /// <summary>
    /// The LowLvlOnlyProblem breeds at the Cowboy level, it uses the visibility of each cowboy to decide how to sort it
    /// </summary>
    class LowLvlOnlyVisibilityProblem : Problem
    {
        /// <summary>
        /// Override of the set up solutions method in Problem. Used since the low level approach only needs one solution
        /// </summary>
        protected override void setUpSolutions()
        {
            solutions = new List<Solution>();
            solutions.Add(new Solution(this));
            initialBestVisibility = solutions[0].getVisibility();
            initialBestAlive = solutions[0].getNumAlive();
        }

        /// <summary>
        /// Overrides the execute method in Problem, eliminates the worst performing cowboys and
        /// breeds new ones using the visibility as a fitness function
        /// </summary>
        public override void execute()
        {
            Cowboy[] cowboys = solutions[solutions.Count - 1].Cowboys;
            List<Cowboy> sortingList = new List<Cowboy>();
            sortingList.AddRange(cowboys);
            sortingList.Sort(Cowboy.sortByNumSeen);
            sortingList.RemoveRange(NumCowboys / 2, NumCowboys / 2);
            Cowboy[] remaining = sortingList.ToArray<Cowboy>();
            while (sortingList.Count < NumCowboys)
            {
                
                sortingList.Add(breedNewCowboy(remaining));
            }
            solutions[0] = new Solution(this, sortingList.ToArray<Cowboy>());
        }
    }
}
