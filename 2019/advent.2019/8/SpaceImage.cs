using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advent._2019._8
{
    public readonly struct SpaceImage
    {
        public int[][,] Layers { get; }

        public SpaceImage(string line, int width, int height)
        {
            Layers = new int[line.Length / (width * height)][,];
            for (int i=0; i < Layers.Length; i++) { Layers[i] = new int[height, width]; }

            var pixels = line.AsMemory();
            void Assign(ref int px)
            {
                px = int.Parse(pixels[..1].Span);
                pixels = pixels[1..];
            }
            foreach (var layer in Layers)
            {
                layer.Foreach(Assign);
            }
            if (!pixels.IsEmpty) { throw new ArgumentException("did not load all input"); }
        }

        public int IntegrityCheck()
        {
            int? minZeroCount = null;
            int[,]? minZeroLayer = null;
            foreach (var layer in Layers)
            {
                int zc = 0;
                layer.Foreach((ref int px) => { if (px == 0) { zc++; } });
                if (!minZeroCount.HasValue || zc < minZeroCount)
                {
                    minZeroCount = zc;
                    minZeroLayer = layer;
                }
            }

            int oneCount = 0, twoCount = 0;
            (minZeroLayer ?? throw new InvalidOperationException("no layer"))
                .Foreach((ref int px) =>
                {
                    if (px == 1) { oneCount++; }
                    else if (px == 2) { twoCount++; }
                });
            return oneCount * twoCount;
        }

        public static async ValueTask<SpaceImage> Parse(
            IAsyncEnumerable<string> lines,
            int width, int height)
        {
            var line = await lines.Single();
            return new SpaceImage(line, width, height);
        }
    }
}
