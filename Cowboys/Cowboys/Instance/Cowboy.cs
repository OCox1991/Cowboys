using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cowboys.Instance
{
    class Cowboy
    {
        private Vector2 location;
        private int numSeen;

        public Vector2 Location
        {
            get { return location; }
        }

        public int NumSeen
        {
            get { return numSeen; }
        }

        public bool isDead
        {
            get { return numSeen > 0; }
        }

        public Cowboy(Vector2 location)
        {
            this.location = location;
            numSeen = 0;
        }

        public void seen()
        {
            numSeen++;
        }
    }
}
