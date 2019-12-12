using System;
using System.Collections.Generic;
using System.Text;

namespace advent._2019._12
{
    public readonly struct Moon
    {
        public Position Position { get; }
        public Velocity Velocity { get; }

        public Moon(Position p, Velocity v)
        {
            Position = p;
            Velocity = v;
        }

        public int Energy()
        {
            checked
            {
                var potential = Math.Abs(Position.X) +
                    Math.Abs(Position.Y) +
                    Math.Abs(Position.Z);
                var kinetic = Math.Abs(Velocity.X) +
                    Math.Abs(Velocity.Y) +
                    Math.Abs(Velocity.Z);
                return potential * kinetic;
            }
        }

        public Moon Move() => new Moon(
            new Position(
                Position.X + Velocity.X,
                Position.Y + Velocity.Y,
                Position.Z + Velocity.Z),
            Velocity);

        public Moon AttractTo(Moon other)
        {
            return new Moon(
                Position,
                new Velocity(
                    Velocity.X + Incr(Position.X, other.Position.X),
                    Velocity.Y + Incr(Position.Y, other.Position.Y),
                    Velocity.Z + Incr(Position.Z, other.Position.Z)));
            static int Incr(int pSelf, int pOther) =>
                pSelf == pOther ? 0 :
                    pSelf > pOther ? -1 : 1;
        }
    }
}
