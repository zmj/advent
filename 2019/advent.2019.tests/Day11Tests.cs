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
    }
}
