using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cowboys.Instance
{
    /// <summary>
    /// A World represents a single node in the problem space, and is thus a single instance of the problem.
    /// </summary>
    class World
    {
        static List<Obstacle> obstacles;
        List<Cowboy> cowboys;

        public void decideOnSeen()
        {
            for (int cb1 = 0; cb1 < cowboys.Count - 1; cb1++)
            {
                for (int cb2 = 0; cb2 < cowboys.Count; cb2++)
                {
                    bool b = false;
                    Line l = new Line(cowboys[cb1].Location, cowboys[cb2].Location);
                    for (int obstacle = 0; obstacle < obstacles.Count; obstacle++)
                    {
                       if(l.intersects(obstacles[obstacle]) && !b)
                       {
                           b = true;
                           cowboys[cb1].seen();
                           cowboys[cb2].seen();
                       }
                    }
                }
            }
        }

        public int fitnessNumAlive()
        {
            int fitness = 0;
            foreach (Cowboy cowboy in cowboys)
            {
                if (!cowboy.isDead)
                {
                    fitness++;
                }
            }
            return fitness;
        }

        public int fitnessNumSeen()
        {
            int fitness = 0;
            foreach (Cowboy cowboy in cowboys)
            {
                fitness += cowboy.NumSeen;
            }
            return fitness;
        }

        public Vector2[][] getGene(int groupSize)
        {

        }
    }
}
