using System;
using System.Collections.Generic;
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

        public int FindMinIntersectionDistance(params Wire[] wires)
        {
            int minDistance = int.MaxValue;
            foreach (var (x,y) in FindIntersections(wires))
            {
                var distance = Math.Abs(x) + Math.Abs(y);
                if (distance < minDistance) { minDistance = distance; }
            }
            return minDistance == int.MaxValue ?
                throw new Exception("no intersections") :
                minDistance;
        }

        public IEnumerable<(int, int)> FindIntersections(
            params Wire[] wires)
        {
            var points = new Dictionary<(int, int), int>();
            for (int i=0; i < wires.Length; i++)
            {
                foreach (var pt in wires[i].Points())
                {
                    if (points.TryAdd(pt, i)) { continue; }
                    if (points[pt] != i) { yield return pt; }
                }
            }
        }
    }
}
