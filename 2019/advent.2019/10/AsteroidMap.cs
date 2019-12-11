using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advent._2019._10
{
    public class AsteroidMap
    {
        private readonly HashSet<(int x, int y)> _asteroids = new HashSet<(int, int)>();

        public int Detect(int x, int y)
        {
            int count = 0;
            foreach (var (ax, ay) in _asteroids)
            {
                if (x == ax && y == ay) { continue; }
                if (Intersects(x, y, ax, ay)
                    .None(i => _asteroids.Contains(i)))
                {
                    count++;
                }
            }
            return count;
        }

        public IEnumerable<(int, int)> FireLaserFrom(int x, int y)
        {
            _asteroids.Remove((x, y));
            var asteroidsByDir = _asteroids
                .GroupBy(a => new Direction(x, y, a.x, a.y))
                .Select(g => (dir: g.Key, asteroids: g.OrderBy(Distance).ToArray().AsMemory()))
                .OrderBy(g => g.dir)
                .ToArray();
            while (asteroidsByDir.Any(g => !g.asteroids.IsEmpty))
            {
                for (int i = 0; i < asteroidsByDir.Length; i++)
                {
                    var (dir, asteroids) = asteroidsByDir[i];
                    if (asteroids.IsEmpty) { continue; }
                    yield return asteroids.Span[0];
                    asteroidsByDir[i] = (dir, asteroids[1..]);
                }
            }

            int Distance((int x, int y) p) => Math.Abs(p.x - x) + Math.Abs(p.y - y);
        }

        private readonly struct Direction : IEquatable<Direction>, IComparable<Direction>
        {
            public double? slope { get; }
            public int xIncrement { get; }
            public int xLimit { get; }
            public int yIncrement { get; }
            public int yLimit { get; }

            public Direction(int x1, int y1, int x2, int y2)
            {
                slope = (x1 != x2) ?
                    (double)(y2 - y1) / (x2 - x1) :
                    (double?)null;
                xIncrement = x2 > x1 ? 1 : -1;
                xLimit = x2 > x1 ? x2 : 0;
                yIncrement = y2 > y1 ? 1 : -1;
                yLimit = y2 > y1 ? y2 : 0;
            }

            public bool Equals(Direction other) =>
                slope == other.slope &&
                xIncrement == other.xIncrement &&
                yIncrement == other.yIncrement;

            public int CompareTo(Direction other)
            {
                if (slope == null && other.slope == null)
                {
                    return yIncrement.CompareTo(other.yIncrement);
                }
                else if (slope == null)
                {
                    return yIncrement < 0 ? -1 :
                        other.xIncrement > 0 ? 1 : -1;
                }
                else if (other.slope == null)
                {
                    return -1 * other.CompareTo(this);
                }
                var (q1, q2) = (Quadrant(this), Quadrant(other));
                return q1 == q2 ?
                    slope.Value.CompareTo(other.slope.Value) :
                    q1.CompareTo(q2);

                static int Quadrant(Direction d) =>
                    (d.xIncrement, d.yIncrement) switch
                    {
                        (1, -1) => 1,
                        (1, 1) => 2,
                        (-1, 1) => 3,
                        (-1, -1) => 4,
                        _ => throw new ArgumentException(d.ToString())
                    };
            }
        }


        public static IEnumerable<(int, int)> Intersects(
            int x1, int y1, int x2, int y2)
        {
            var d = new Direction(x1, y1, x2, y2);
            if (d.slope == null)
            {
                if (y1 != y2)
                {
                    for (int y = y1 + d.yIncrement; y != y2; y += d.yIncrement)
                    {
                        yield return (x1, y);
                    }
                }
                yield break;
            }
            for (int x = x1 + d.xIncrement; x != x2; x += d.xIncrement)
            {
                double y = (x - x1) * d.slope.Value + y1;
                if (IsInt(y)) { yield return (x, (int)y); }
            }
            bool IsInt(double d) => Math.Abs(d - (int)d) < double.Epsilon;
        }

        public (int x, int y, int n) FindBestLocation()
        {
            int? maxDetected = null;
            (int x, int y)? maxDetectedXY = null;
            foreach (var (ax, ay) in _asteroids)
            {
                int detected = Detect(ax, ay);
                if (maxDetected == null || detected > maxDetected)
                {
                    maxDetected = detected;
                    maxDetectedXY = (ax, ay);
                }
            }
            return (maxDetectedXY.Value.x, maxDetectedXY.Value.y, maxDetected.Value);
        }

        public async ValueTask<AsteroidMap> Load(IAsyncEnumerable<string> lines)
        {
            int y = 0;
            await foreach (var line in lines)
            {
                for (int x = 0; x < line.Length; x++)
                {
                    switch (line[x])
                    {
                        case '.': break;
                        case '#': _asteroids.Add((x, y)); break;
                        default: throw new ArgumentException($"unknown map char {line[x]}");
                    }
                }
                y++;
            }
            return this;
        }
    }
}
