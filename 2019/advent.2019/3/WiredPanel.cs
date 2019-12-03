using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advent._2019._3
{
    public readonly struct WiredPanel
    {
        public async ValueTask<int> FindIntersectionDistance(
            IAsyncEnumerable<string> lines)
        {
            var wires = await lines.Select(l => new Wire(l)).ToArray();
            return FindMinIntersectionDistance(wires);
        }

        public async ValueTask<int> FindMinSignalDelay(
            IAsyncEnumerable<string> lines)
        {
            var wires = await lines.Select(l => new Wire(l)).ToArray();
            return FindMinSignalDelay(wires);
        }

        public int FindMinIntersectionDistance(params Wire[] wires)
        {
            int minDistance = int.MaxValue;
            foreach (var (x,y) in FindIntersectionPoints(wires))
            {
                var distance = Math.Abs(x) + Math.Abs(y);
                if (distance < minDistance) { minDistance = distance; }
            }
            return minDistance == int.MaxValue ?
                throw new Exception("no intersections") :
                minDistance;
        }

        public int FindMinSignalDelay(params Wire[] wires)
        {
            int minDelay = int.MaxValue;
            foreach (var intersection in FindIntersections(wires))
            {
                var delay = intersection.WireLengths.Sum();
                if (delay < minDelay) { minDelay = delay; }
            }
            return minDelay == int.MaxValue ?
                throw new Exception("no intersections") :
                minDelay;
        }

        public IEnumerable<(int, int)> FindIntersectionPoints(
            params Wire[] wires) =>
            FindIntersections(wires).Select(i => (i.X, i.Y));

        public IEnumerable<Intersection> FindIntersections(
            params Wire[] wires)
        {
            var points = new Dictionary<(int, int), int[]>();
            for (int i = 0; i < wires.Length; i++)
            {
                int length = 0;
                foreach (var pt in wires[i].Points())
                {
                    length++;
                    var wireLengths = new int[wires.Length];
                    wireLengths[i] = length;
                    if (points.TryAdd(pt, wireLengths)) { continue; }

                    wireLengths = points[pt];
                    if (wireLengths[i] != 0) { continue; } // assume 2 wires
                    wireLengths[i] = length;
                    yield return new Intersection(pt.x, pt.y, wireLengths);
                }
            }
        }

        public readonly struct Intersection
        {
            public int X { get; }
            public int Y { get; }
            public int[] WireLengths { get; }
            public Intersection(int x, int y, int[] wl) =>
                (X, Y, WireLengths) = (x, y, wl);
        }
    }
}
