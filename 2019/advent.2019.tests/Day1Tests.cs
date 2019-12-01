using advent._2019._1;
using System;
using System.Threading.Tasks;
using Xunit;

namespace advent._2019.tests
{
    public class Day1Tests
    {
        [Theory]
        [InlineData(12, 2)]
        [InlineData(14, 2)]
        [InlineData(1969, 654)]
        [InlineData(100756, 33583)]
        public void MassToFuel(ulong mass, ulong fuel)
        {
            var x = FuelCalculator.MassToFuel(mass);
            Assert.Equal(fuel, x);
        }

        [Theory]
        [InlineData("input_1_1", 3301059)]
        public async Task Day_1_1(string inputFile, ulong answer)
        {
            var lines = LineReader.Open(inputFile);
            var sum = await new FuelCalculator().SumFuel(lines);
            Assert.Equal(answer, sum);
        }
    }
}
