using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cowboys
{
    /// <summary>
    /// The runner class contains the main method, it runs the different problems in order and returns the result
    /// </summary>
    class Runner
    {
        public static void Main(string[] args)
        {
            ControlProblem control = new ControlProblem();
            write(control, "Control:");

            LowLvlOnlyDeadProblem low = new LowLvlOnlyDeadProblem();
            write(low, "Low Level Only, Dead vs Alive:");
            
            LowLvlOnlyVisibilityProblem lowV = new LowLvlOnlyVisibilityProblem();
            write(lowV, "Low Level Only, Visibility:");
            
            HighLvlOnlyDeadProblem hListAD = new HighLvlOnlyDeadProblem();
            write(hListAD, "High Level Only, Dead vs Alive:");
            
            HighLvlOnlyVisProblem hListV = new HighLvlOnlyVisProblem();
            write(hListV, "High Level Only, Visibility:");

            Console.Read();
        }

        /// <summary>
        /// The write class writes the important information about a problem to the console
        /// </summary>
        /// <param name="p">The problem to write to the console</param>
        /// <param name="title">The title to begin the section with</param>
        private static void write(Problem p, string title)
        {
            int i = title.Length;
            string topBottom = "";
            for (int j = 0; j < i + 6; j++)
            {
                topBottom += "X";
            }
            Console.WriteLine(topBottom);
            Console.WriteLine("XX " + title + " XX");
            Console.WriteLine(topBottom);
            Console.WriteLine();
            Solution t = p.getBestNumAlive();
            Console.WriteLine("Number alive (Best): " + t.getNumAlive());
            Console.WriteLine("Visibility (Best): " + p.getBestVisibility().getVisibility());
            Console.WriteLine("Improvement: Number Alive (Best): " + (t.getNumAlive() - p.InitialBestAlive));
            Console.WriteLine("Improvement: Visibility (Best): " + (p.getBestVisibility().getVisibility() - p.InitialBestVisibility));
            if (p.Solutions.Count != 1)
            {
                Console.WriteLine("_________________________________________________________");
                Console.WriteLine("Number alive (Mean): " + p.getAvgNumAlive());
                Console.WriteLine("Visibility (Mean): " + p.getAvgVisibility());
                Console.WriteLine("Improvement: Number Alive (Mean): " + (p.getAvgNumAlive() - p.InitialAvgAlive));
                Console.WriteLine("Improvement: Visibility (Mean): " + (p.getAvgVisibility() - p.InitialAvgVis));
               /* Console.WriteLine("_________________________________________________________");
                Console.WriteLine("Number Alive (Final Set):");
                foreach (Solution s in p.Solutions)
                {
                    Console.Write(s.getNumAlive() + ", ");
                }
                Console.WriteLine();
                Console.WriteLine("Visibility (Final Set):");
                foreach (Solution s in p.Solutions)
                {
                    Console.Write(s.getVisibility() + ", ");
                }
                Console.WriteLine();*/
            }
            Console.WriteLine("=========================================================");
            Console.WriteLine();
        }
    }
}
