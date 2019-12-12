using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace advent._2019._12
{
    public readonly struct Position
    {
        private static readonly Regex _parser =
            new Regex(@"<x=(-?\d+), y=(-?\d+), z=(-?\d+)>", RegexOptions.Compiled);

        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public Position(string line)
        {
            var match = _parser.Match(line);
            if (!match.Success) { Throw("no match"); }
            if (match.Groups.Count != 4) { Throw("group count"); }
            X = int.Parse(match.Groups[1].Value);
            Y = int.Parse(match.Groups[2].Value);
            Z = int.Parse(match.Groups[3].Value);

            void Throw(string s) => throw new ArgumentException($"{s}: {line}");
        }

        public Position(int x, int y, int z) => (X, Y, Z) = (x, y, z);
    }
}
