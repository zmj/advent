using advent._2019._10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace advent._2019.tests
{
    public class Day10Tests
    {
        private const string example1 = @".#..#
.....
#####
....#
...##";

        [Theory]
        [InlineData(example1, 4, 2, 5)]
        [InlineData(example1, 0, 2, 6)]
        [InlineData(example1, 4, 0, 7)]
        [InlineData(example1, 3, 4, 8)]
        public async Task DetectAsteroids(string lines, int x, int y, int n)
        {
            var map = await new AsteroidMap().Load(LineReader.Split(lines));
            var count = map.Detect(x, y);
            Assert.Equal(n, count);
        }

        [Theory]
        [InlineData(3, 4, 1, 0, new[] { 2, 2 })]
        [InlineData(0, 0, 9, 6, new[] { 3, 2, 6, 4 })]
        [InlineData(0, 3, 0, 0, new[] { 0, 2, 0, 1 })]
        [InlineData(0, 0, 0, 0, new int[] { })]
        public void Intersects(int x1, int y1, int x2, int y2, int[] pointsFlat)
        {
            var points = pointsFlat.Where((_, i) => i % 2 == 0)
                .Zip(pointsFlat.Where((_, i) => i % 2 == 1)).ToArray();
            var intersects = AsteroidMap.Intersects(x1, y1, x2, y2).ToArray();
            Assert.Equal(points.Length, intersects.Length);
            for (int i = 0; i < points.Length; i++)
            {
                Assert.Equal(points[i], intersects[i]);
            }
        }

        [Theory]
        [InlineData(example1, 3, 4, 8)]
        public async Task FindBestLocation(string lines, int x, int y, int n)
        {
            var map = await new AsteroidMap().Load(LineReader.Split(lines));
            var (sx, sy, sn) = map.FindBestLocation();
            Assert.Equal(x, sx);
            Assert.Equal(y, sy);
            Assert.Equal(n, sn);
        }

        [Theory]
        [InlineData("input_10_1", 263)]
        public async Task Day_10_1(string inputFile, int answer)
        {
            var map = await new AsteroidMap().Load(LineReader.Open(inputFile));
            var (_, _, n) = map.FindBestLocation();
            Assert.Equal(answer, n);
        }
    }
}
