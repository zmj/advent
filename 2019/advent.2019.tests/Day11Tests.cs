using advent._2019._11;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace advent._2019.tests
{
    public class Day11Tests
    {
        [Theory]
        [InlineData("input_11_1", 2054)]
        public async Task Day_11_1(string inputFile, int answer)
        {
            var lines = LineReader.Open(inputFile);
            var painter = await HullPainter.Create(lines);
            await painter.RunProgram();
            Assert.Equal(answer, painter.PaintedPanels);
        }

        [Theory]
        [InlineData("input_11_1", answer2)]
        public async Task Day_11_2(string inputFile, string answer)
        {
            var lines = LineReader.Open(inputFile);
            var painter = await HullPainter.Create(lines);
            await painter.RunProgram(firstPanelWhite: true);
            var x = painter.Render();
            Assert.Equal(answer, x);
        }

        private const string answer2 = @"#..#.###..####.####..##....##.#..#.###.
#.#..#..#....#.#....#..#....#.#..#.#..#
##...#..#...#..###..#..#....#.####.###.
#.#..###...#...#....####....#.#..#.#..#
#.#..#.#..#....#....#..#.#..#.#..#.#..#
#..#.#..#.####.####.#..#..##..#..#.###.
";
    }
}
