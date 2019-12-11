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

        const string example2 = @".#..##.###...#######
##.############..##.
.#.######.########.#
.###.#######.####.#.
#####.##.#.##.###.##
..#####..#.#########
####################
#.####....###.#.#.##
##.#################
#####.##.###..####..
..######..##.#######
####.##.####...##..#
.#####..#.######.###
##...#.##########...
#.##########.#######
.####.#.###.###.#.##
....##.##.###..#####
.#.#.###########.###
#.#.#.#####.####.###
###.##.####.##.#..##";

        [Theory]
        [InlineData(1, 11, 12)]
        [InlineData(2, 12, 1)]
        [InlineData(3, 12, 2)]
        [InlineData(10, 12, 8)]
        [InlineData(20, 16, 0)]
        [InlineData(50, 16, 9)]
        [InlineData(100, 10, 16)]
        [InlineData(199, 9, 6)]
        [InlineData(200, 8, 2)]
        [InlineData(201, 10, 9)]
        [InlineData(299, 11, 1)]
        public async Task FireLaserFrom(int n, int x, int y)
        {
            var map = await new AsteroidMap().Load(LineReader.Split(example2));
            var destroyed = map.FireLaserFrom(11, 13);
            var nth = destroyed.ElementAt(n - 1);
            Assert.Equal((x, y), nth);
        }

        [Theory]
        [InlineData("input_10_1", 200, 1110)]
        public async Task Day_10_2(string inputFile, int n, int answer)
        {
            var map = await new AsteroidMap().Load(LineReader.Open(inputFile));
            var (x, y, _) = map.FindBestLocation();
            var destroyed = map.FireLaserFrom(x, y);
            var (nx, ny) = destroyed.ElementAt(n - 1);
            Assert.Equal(answer, 100 * nx + ny);
        }
    }
}
