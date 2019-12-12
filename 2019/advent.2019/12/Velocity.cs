using System;
using System.Collections.Generic;
using System.Text;

namespace advent._2019._12
{
    public readonly struct Velocity
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public Velocity(int x, int y, int z) => (X, Y, Z) = (x, y, z);
    }
}
