using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advent._2019._10
{
    public class AsteroidMap
    {
        private readonly HashSet<(int, int)> _asteroids = new HashSet<(int, int)>();

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

        public static IEnumerable<(int, int)> Intersects(
            int x1, int y1, int x2, int y2)
        {
            if (x1 == x2)
            {
                if (y1 == y2) { yield break; }
                int yIncrement = y2 > y1 ? 1 : -1;
                for (int y = y1 + yIncrement; y != y2; y += yIncrement)
                {
                    yield return (x1, y);
                }
                yield break;
            }
            double slope = (double)(y2 - y1) / (x2 - x1);
            int xIncrement = x2 > x1 ? 1 : -1;
            for (int x = x1 + xIncrement; x != x2; x += xIncrement)
            {
                double y = (x - x1) * slope + y1;
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
