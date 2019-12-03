using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace advent._2019._3
{
    public readonly struct Wire
    {
        private readonly (int, int)[] _segments;

        public Wire(string line) => _segments = ParseSegments(line);

        public IEnumerable<(int x, int y)> Points()
        {
            var (x, y) = (0, 0);
            foreach (var pt in _segments)
            {
                var (dx, dy) = pt;
                while (dx != 0)
                {
                    if (dx > 0) { x++; dx--; }
                    else { x--; dx++; }
                    yield return (x, y);
                }
                while (dy != 0)
                {
                    if (dy > 0) { y++; dy--; }
                    else { y--; dy++; }
                    yield return (x, y);
                }
            }
        }

        public static (int, int)[] ParseSegments(string line)
        {
            return line.Split(',')
                .Select(s => s[0] switch
                {
                    'R' => (int.Parse(s[1..]), 0),
                    'L' => (-1 * int.Parse(s[1..]), 0),
                    'U' => (0, int.Parse(s[1..])),
                    'D' => (0, -1 * int.Parse(s[1..])),
                    _ => throw new ArgumentException("invalid direction: " + s[0]),
                }).ToArray();
        }
    }
}
